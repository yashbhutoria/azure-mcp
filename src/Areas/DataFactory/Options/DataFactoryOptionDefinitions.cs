// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace AzureMcp.Areas.DataFactory.Options;

public static class DataFactoryOptionDefinitions
{
    public const string FactoryName = "factory-name";
    public const string PipelineName = "pipeline-name";
    public const string RunId = "run-id";
    public const string Parameters = "parameters";
    public const string Status = "status";
    public const string LastUpdatedAfter = "last-updated-after";
    public const string LastUpdatedBefore = "last-updated-before";
    public const string MaxResults = "max-results";

    public static readonly Option<string> Factory = new(
        [$"--{FactoryName}", "-f"],
        "The name of the Azure Data Factory. This is the unique name you chose for your data factory (e.g., 'mydatafactory')."
    )
    {
        IsRequired = true,
        ArgumentHelpName = "factory-name"
    };

    public static readonly Option<string> Pipeline = new(
        [$"--{PipelineName}", "-p"],
        "The name of the pipeline in the Data Factory."
    )
    {
        IsRequired = true,
        ArgumentHelpName = "pipeline-name"
    };

    public static readonly Option<string> RunIdOption = new(
        [$"--{RunId}"],
        "The unique identifier of the pipeline run."
    )
    {
        IsRequired = true,
        ArgumentHelpName = "run-id"
    };

    public static readonly Option<string> ParametersOption = new(
        [$"--{Parameters}"],
        "Pipeline parameters as a JSON string (e.g., '{\"inputPath\": \"/data/input\", \"outputPath\": \"/data/output\"}')."
    )
    {
        ArgumentHelpName = "json"
    };

    public static readonly Option<string> StatusOption = new(
        [$"--{Status}"],
        "Filter by pipeline run status (InProgress, Succeeded, Failed, Cancelled)."
    )
    {
        ArgumentHelpName = "status"
    };

    public static readonly Option<DateTime?> LastUpdatedAfterOption = new(
        [$"--{LastUpdatedAfter}"],
        "Filter pipeline runs updated after this date (ISO 8601 format)."
    )
    {
        ArgumentHelpName = "datetime"
    };

    public static readonly Option<DateTime?> LastUpdatedBeforeOption = new(
        [$"--{LastUpdatedBefore}"],
        "Filter pipeline runs updated before this date (ISO 8601 format)."
    )
    {
        ArgumentHelpName = "datetime"
    };

    public static readonly Option<int?> MaxResultsOption = new(
        [$"--{MaxResults}"],
        "Maximum number of results to return (default: 50)."
    )
    {
        ArgumentHelpName = "count"
    };
}