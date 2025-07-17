using AzureMcp.Areas.DataFactory.Commands.Dataset;
using AzureMcp.Areas.DataFactory.Commands.LinkedService;
using AzureMcp.Areas.DataFactory.Commands.Pipeline;
using AzureMcp.Areas.DataFactory.Services;
using AzureMcp.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AzureMcp.Areas.DataFactory;

public class DataFactorySetup : IAreaSetup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Register DataFactory service
        services.AddSingleton<IDataFactoryService, DataFactoryService>();
    }

    public void RegisterCommands(CommandGroup rootGroup, ILoggerFactory loggerFactory)
    {
        // Create DataFactory command group
        var dataFactory = new CommandGroup("datafactory", "Data Factory operations - Commands for managing Azure Data Factory resources.");
        rootGroup.AddSubGroup(dataFactory);

        // Create DataFactory subgroups
        var pipelines = new CommandGroup("pipeline", "Pipeline operations - Commands for managing Data Factory pipelines.");
        dataFactory.AddSubGroup(pipelines);

        var pipelineRuns = new CommandGroup("pipeline-run", "Pipeline run operations - Commands for managing pipeline runs.");
        dataFactory.AddSubGroup(pipelineRuns);

        var datasets = new CommandGroup("dataset", "Dataset operations - Commands for managing Data Factory datasets.");
        dataFactory.AddSubGroup(datasets);

        var linkedServices = new CommandGroup("linkedservice", "Linked service operations - Commands for managing Data Factory linked services.");
        dataFactory.AddSubGroup(linkedServices);

        // Register Pipeline commands
        pipelines.AddCommand("list", new ListPipelinesCommand(
            loggerFactory.CreateLogger<ListPipelinesCommand>()));
        pipelines.AddCommand("run", new RunPipelineCommand(
            loggerFactory.CreateLogger<RunPipelineCommand>()));

        // Register Pipeline Run commands
        pipelineRuns.AddCommand("get", new GetPipelineRunCommand(
            loggerFactory.CreateLogger<GetPipelineRunCommand>()));

        // Register Dataset commands
        datasets.AddCommand("list", new ListDatasetsCommand(
            loggerFactory.CreateLogger<ListDatasetsCommand>()));

        // Register Linked Service commands
        linkedServices.AddCommand("list", new ListLinkedServicesCommand(
            loggerFactory.CreateLogger<ListLinkedServicesCommand>()));
    }
}