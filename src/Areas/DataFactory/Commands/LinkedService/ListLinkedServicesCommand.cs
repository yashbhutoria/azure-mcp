// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using AzureMcp.Areas.DataFactory.Models;
using AzureMcp.Areas.DataFactory.Options.LinkedService;
using AzureMcp.Areas.DataFactory.Services;
using Microsoft.Extensions.Logging;

namespace AzureMcp.Areas.DataFactory.Commands.LinkedService;

public sealed class ListLinkedServicesCommand(ILogger<ListLinkedServicesCommand> logger)
    : BaseDataFactoryCommand<ListLinkedServicesOptions>
{
    private const string CommandTitle = "List Data Factory Linked Services";
    private readonly ILogger<ListLinkedServicesCommand> _logger = logger;

    public override string Name => "list";

    public override string Description =>
        """
        List all linked services in an Azure Data Factory. This command retrieves and displays all linked services available in the specified Data Factory.
        Results include service names, types, and integration runtime references. Linked services define connections to external data stores and compute resources.
        Use this command to explore your Data Factory connections or to verify linked service configuration before dataset and pipeline operations.
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
            var linkedServices = await dataFactoryService.ListLinkedServicesAsync(
                options.FactoryName!,
                options.ResourceGroup!,
                options.Subscription!,
                options.Tenant,
                options.RetryPolicy);

            context.Response.Results = linkedServices?.Count > 0
                ? ResponseResult.Create(new ListLinkedServicesCommandResult(linkedServices), DataFactoryJsonContext.Default.ListLinkedServicesCommandResult)
                : null;

            if (linkedServices?.Count == 0)
            {
                context.Response.Message = $"No linked services found in Data Factory '{options.FactoryName}'.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing linked services. Factory: {Factory}, ResourceGroup: {ResourceGroup}",
                options.FactoryName, options.ResourceGroup);
            HandleException(context, ex);
        }

        return context.Response;
    }

    public record ListLinkedServicesCommandResult(List<LinkedServiceModel> LinkedServices);
}
