// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace AzureMcp.Areas.DataFactory.Options.Pipeline;

public class GetPipelineRunOptions : BaseDataFactoryOptions
{
    [JsonPropertyName(DataFactoryOptionDefinitions.RunId)]
    public string? RunId { get; set; }
}