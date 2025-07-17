// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace AzureMcp.Areas.DataFactory.Models;

/// <summary>
/// Represents a Data Factory pipeline run.
/// </summary>
public class PipelineRunModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the pipeline run.
    /// </summary>
    [JsonPropertyName("runId")]
    public required string RunId { get; set; }

    /// <summary>
    /// Gets or sets the name of the pipeline being run.
    /// </summary>
    [JsonPropertyName("pipelineName")]
    public required string PipelineName { get; set; }

    /// <summary>
    /// Gets or sets the current status of the pipeline run (e.g., InProgress, Succeeded, Failed).
    /// </summary>
    [JsonPropertyName("status")]
    public required string Status { get; set; }

    /// <summary>
    /// Gets or sets the start time of the pipeline run.
    /// </summary>
    [JsonPropertyName("runStart")]
    public DateTime? RunStart { get; set; }

    /// <summary>
    /// Gets or sets the end time of the pipeline run.
    /// </summary>
    [JsonPropertyName("runEnd")]
    public DateTime? RunEnd { get; set; }

    /// <summary>
    /// Gets or sets the duration of the pipeline run in milliseconds.
    /// </summary>
    [JsonPropertyName("durationInMs")]
    public long? DurationInMs { get; set; }

    /// <summary>
    /// Gets or sets the parameters used for this pipeline run.
    /// </summary>
    [JsonPropertyName("parameters")]
    public Dictionary<string, string>? Parameters { get; set; }

    /// <summary>
    /// Gets or sets any message associated with the pipeline run (e.g., error messages).
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}