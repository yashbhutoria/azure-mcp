// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json;
using AzureMcp.Areas.DataFactory.Models;
using AzureMcp.Areas.DataFactory.Options;
using AzureMcp.Areas.DataFactory.Options.Pipeline;
using AzureMcp.Areas.DataFactory.Services;
using Microsoft.Extensions.Logging;

namespace AzureMcp.Areas.DataFactory.Commands.Pipeline;

public sealed class RunPipelineCommand(ILogger<RunPipelineCommand> logger)
    : BaseDataFactoryCommand<RunPipelineOptions>
{
    private const string CommandTitle = "Run Data Factory Pipeline";
    private readonly ILogger<RunPipelineCommand> _logger = logger;
    private readonly Option<string> _pipelineOption = DataFactoryOptionDefinitions.Pipeline;
    private readonly Option<string> _parametersOption = DataFactoryOptionDefinitions.ParametersOption;

    public override string Name => "run";

    public override string Description =>
        """
        Run a pipeline in an Azure Data Factory. This command triggers the execution of a specified pipeline.
        You can optionally pass parameters to the pipeline as a JSON string. The command returns the run ID and initial status.
        Use this command to start data processing workflows, ETL operations, or other pipeline activities in your Data Factory.
        """;

    public override string Title => CommandTitle;

    protected override void RegisterOptions(Command command)
    {
        base.RegisterOptions(command);
        command.AddOption(_pipelineOption);
        command.AddOption(_parametersOption);
    }

    protected override RunPipelineOptions BindOptions(ParseResult parseResult)
    {
        var options = base.BindOptions(parseResult);
        options.PipelineName = parseResult.GetValueForOption(_pipelineOption);
        options.Parameters = parseResult.GetValueForOption(_parametersOption);
        return options;
    }

    [McpServerTool(
        Destructive = false,
        ReadOnly = false,
        Title = CommandTitle)]
    public override async Task<CommandResponse> ExecuteAsync(CommandContext context, ParseResult parseResult)
    {
        var options = BindOptions(parseResult);

        try
        {
            if (!Validate(parseResult.CommandResult, context.Response).IsValid)
            {
                return context.Response;
            }

            // Parse parameters if provided
            Dictionary<string, object>? parameters = null;
            if (!string.IsNullOrWhiteSpace(options.Parameters))
            {
                try
                {
                    parameters = JsonSerializer.Deserialize<Dictionary<string, object>>(
                        options.Parameters,
                        DataFactoryJsonContext.Default.DictionaryStringObject);
                }
                catch (JsonException ex)
                {
                    context.Response.Status = 400;
                    context.Response.Message = $"Invalid JSON parameters: {ex.Message}";
                    return context.Response;
                }
            }

            var dataFactoryService = context.GetService<IDataFactoryService>();
            var runResult = await dataFactoryService.RunPipelineAsync(
                options.FactoryName!,
                options.ResourceGroup!,
                options.PipelineName!,
                parameters,
                options.Subscription!,
                options.Tenant,
                options.RetryPolicy);

            context.Response.Results = ResponseResult.Create(
                new RunPipelineCommandResult(runResult),
                DataFactoryJsonContext.Default.RunPipelineCommandResult);

            context.Response.Message = $"Pipeline '{options.PipelineName}' started successfully. Run ID: {runResult.RunId}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running pipeline. Factory: {Factory}, Pipeline: {Pipeline}",
                options.FactoryName, options.PipelineName);
            HandleException(context, ex);
        }

        return context.Response;
    }

    public record RunPipelineCommandResult(PipelineRunModel PipelineRun);
}
