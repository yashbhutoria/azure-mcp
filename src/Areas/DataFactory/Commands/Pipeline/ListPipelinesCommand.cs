// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using AzureMcp.Areas.DataFactory.Models;
using AzureMcp.Areas.DataFactory.Options.Pipeline;
using AzureMcp.Areas.DataFactory.Services;
using Microsoft.Extensions.Logging;

namespace AzureMcp.Areas.DataFactory.Commands.Pipeline;

public sealed class ListPipelinesCommand(ILogger<ListPipelinesCommand> logger)
    : BaseDataFactoryCommand<ListPipelinesOptions>
{
    private const string CommandTitle = "List Data Factory Pipelines";
    private readonly ILogger<ListPipelinesCommand> _logger = logger;

    public override string Name => "list";

    public override string Description =>
        """
        List all pipelines in an Azure Data Factory. This command retrieves and displays all pipelines available in the specified Data Factory.
        Results include pipeline names, descriptions, and parameters. You must specify a factory name, resource group, and subscription ID.
        Use this command to explore your Data Factory resources or to verify pipeline existence before running operations.
        """;

    public override string Title => CommandTitle;

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
            var pipelines = await dataFactoryService.ListPipelinesAsync(
                options.FactoryName!,
                options.ResourceGroup!,
                options.Subscription!,
                options.Tenant,
                options.RetryPolicy);

            context.Response.Results = pipelines?.Count > 0
                ? ResponseResult.Create(new ListPipelinesCommandResult(pipelines), DataFactoryJsonContext.Default.ListPipelinesCommandResult)
                : null;

            if (pipelines?.Count == 0)
            {
                context.Response.Message = $"No pipelines found in Data Factory '{options.FactoryName}'.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing pipelines. Factory: {Factory}, ResourceGroup: {ResourceGroup}",
                options.FactoryName, options.ResourceGroup);
            HandleException(context, ex);
        }

        return context.Response;
    }

    public record ListPipelinesCommandResult(List<PipelineModel> Pipelines);
}
