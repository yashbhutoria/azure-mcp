// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace AzureMcp.Areas.DataFactory.Models;

/// <summary>
/// Represents a Data Factory pipeline.
/// </summary>
public class PipelineModel
{
    /// <summary>
    /// Gets or sets the name of the pipeline.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the pipeline.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the parameters defined for the pipeline.
    /// </summary>
    [JsonPropertyName("parameters")]
    public Dictionary<string, object>? Parameters { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the last pipeline run.
    /// </summary>
    [JsonPropertyName("lastRun")]
    public DateTime? LastRun { get; set; }

    /// <summary>
    /// Gets or sets the status of the last pipeline run.
    /// </summary>
    [JsonPropertyName("lastRunStatus")]
    public string? LastRunStatus { get; set; }
}
