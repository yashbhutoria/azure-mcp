// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using AzureMcp.Areas.DataFactory.Models;
using AzureMcp.Areas.DataFactory.Options.Dataset;
using AzureMcp.Areas.DataFactory.Services;
using Microsoft.Extensions.Logging;

namespace AzureMcp.Areas.DataFactory.Commands.Dataset;

public sealed class ListDatasetsCommand(ILogger<ListDatasetsCommand> logger)
    : BaseDataFactoryCommand<ListDatasetsOptions>
{
    private const string CommandTitle = "List Data Factory Datasets";
    private readonly ILogger<ListDatasetsCommand> _logger = logger;

    public override string Name => "list";

    public override string Description =>
        """
        List all datasets in an Azure Data Factory. This command retrieves and displays all datasets available in the specified Data Factory.
        Results include dataset names, types, linked service references, and schema information. You must specify a factory name, resource group, and subscription ID.
        Use this command to explore your Data Factory datasets or to verify dataset configuration before pipeline operations.
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
            var datasets = await dataFactoryService.ListDatasetsAsync(
                options.FactoryName!,
                options.ResourceGroup!,
                options.Subscription!,
                options.Tenant,
                options.RetryPolicy);

            context.Response.Results = datasets?.Count > 0
                ? ResponseResult.Create(new ListDatasetsCommandResult(datasets), DataFactoryJsonContext.Default.ListDatasetsCommandResult)
                : null;

            if (datasets?.Count == 0)
            {
                context.Response.Message = $"No datasets found in Data Factory '{options.FactoryName}'.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing datasets. Factory: {Factory}, ResourceGroup: {ResourceGroup}",
                options.FactoryName, options.ResourceGroup);
            HandleException(context, ex);
        }

        return context.Response;
    }

    public record ListDatasetsCommandResult(List<DatasetModel> Datasets);
}