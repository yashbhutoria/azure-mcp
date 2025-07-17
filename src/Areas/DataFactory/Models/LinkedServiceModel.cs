// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace AzureMcp.Areas.DataFactory.Models;

/// <summary>
/// Represents a Data Factory linked service.
/// </summary>
public class LinkedServiceModel
{
    /// <summary>
    /// Gets or sets the name of the linked service.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the type of the linked service (e.g., AzureBlobStorage, AzureSqlDatabase).
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    /// <summary>
    /// Gets or sets the description of the linked service.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the integration runtime reference for the linked service.
    /// </summary>
    [JsonPropertyName("connectVia")]
    public string? ConnectVia { get; set; }
}