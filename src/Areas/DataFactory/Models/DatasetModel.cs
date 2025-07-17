// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace AzureMcp.Areas.DataFactory.Models;

/// <summary>
/// Represents a Data Factory dataset.
/// </summary>
public class DatasetModel
{
    /// <summary>
    /// Gets or sets the name of the dataset.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the type of the dataset (e.g., AzureBlob, AzureSqlTable).
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    /// <summary>
    /// Gets or sets the name of the linked service associated with this dataset.
    /// </summary>
    [JsonPropertyName("linkedServiceName")]
    public string? LinkedServiceName { get; set; }

    /// <summary>
    /// Gets or sets the folder path for file-based datasets.
    /// </summary>
    [JsonPropertyName("folderPath")]
    public string? FolderPath { get; set; }

    /// <summary>
    /// Gets or sets the schema definition of the dataset.
    /// </summary>
    [JsonPropertyName("schema")]
    public object? Schema { get; set; }
}