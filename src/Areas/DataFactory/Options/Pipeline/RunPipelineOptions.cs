// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace AzureMcp.Areas.DataFactory.Options.Pipeline;

public class RunPipelineOptions : BaseDataFactoryOptions
{
    [JsonPropertyName(DataFactoryOptionDefinitions.PipelineName)]
    public string? PipelineName { get; set; }

    [JsonPropertyName(DataFactoryOptionDefinitions.Parameters)]
    public string? Parameters { get; set; }
}
