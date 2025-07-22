// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using AzureMcp.Areas.DataFactory.Models;
using AzureMcp.Areas.DataFactory.Options;
using AzureMcp.Areas.DataFactory.Options.Pipeline;
using AzureMcp.Areas.DataFactory.Services;
using Microsoft.Extensions.Logging;

namespace AzureMcp.Areas.DataFactory.Commands.Pipeline;

public sealed class GetPipelineRunCommand(ILogger<GetPipelineRunCommand> logger)
    : BaseDataFactoryCommand<GetPipelineRunOptions>
{
    private const string CommandTitle = "Get Pipeline Run Status";
    private readonly ILogger<GetPipelineRunCommand> _logger = logger;
    private readonly Option<string> _runIdOption = DataFactoryOptionDefinitions.RunIdOption;

    public override string Name => "get";

    public override string Description =>
        """
        Get the status and details of a pipeline run in an Azure Data Factory. This command retrieves information about a specific pipeline run,
        including its current status, start time, duration, and any parameters used. Use the run ID returned from the 'run' command.
        This is useful for monitoring pipeline execution progress and debugging failed runs.
        """;

    public override string Title => CommandTitle;

    protected override void RegisterOptions(Command command)
    {
        base.RegisterOptions(command);
        command.AddOption(_runIdOption);
    }

    protected override GetPipelineRunOptions BindOptions(ParseResult parseResult)
    {
        var options = base.BindOptions(parseResult);
        options.RunId = parseResult.GetValueForOption(_runIdOption);
        return options;
    }

    [McpServerTool(
        Destructive = false,
        ReadOnly = true,
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

            var dataFactoryService = context.GetService<IDataFactoryService>();
            var runResult = await dataFactoryService.GetPipelineRunAsync(
                options.FactoryName!,
                options.ResourceGroup!,
                options.RunId!,
                options.Subscription!,
                options.Tenant,
                options.RetryPolicy);

            context.Response.Results = ResponseResult.Create(
                new GetPipelineRunCommandResult(runResult),
                DataFactoryJsonContext.Default.GetPipelineRunCommandResult);

            // Add informative message based on status
            var statusMessage = runResult.Status switch
            {
                "InProgress" => $"Pipeline run is currently in progress. Duration: {FormatDuration(runResult.DurationInMs)}",
                "Succeeded" => $"Pipeline run completed successfully. Duration: {FormatDuration(runResult.DurationInMs)}",
                "Failed" => $"Pipeline run failed. {runResult.Message}",
                "Cancelled" => "Pipeline run was cancelled.",
                _ => $"Pipeline run status: {runResult.Status}"
            };

            context.Response.Message = statusMessage;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pipeline run. Factory: {Factory}, RunId: {RunId}",
                options.FactoryName, options.RunId);
            HandleException(context, ex);
        }

        return context.Response;
    }

    private static string FormatDuration(long? durationMs)
    {
        if (!durationMs.HasValue)
            return "unknown";

        var duration = TimeSpan.FromMilliseconds(durationMs.Value);
        if (duration.TotalHours >= 1)
            return $"{duration.Hours}h {duration.Minutes}m {duration.Seconds}s";
        if (duration.TotalMinutes >= 1)
            return $"{duration.Minutes}m {duration.Seconds}s";
        return $"{duration.Seconds}s";
    }

    public record GetPipelineRunCommandResult(PipelineRunModel PipelineRun);
}
