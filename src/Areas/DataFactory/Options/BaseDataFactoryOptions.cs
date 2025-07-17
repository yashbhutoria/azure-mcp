// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using AzureMcp.Options;

namespace AzureMcp.Areas.DataFactory.Options;

public class BaseDataFactoryOptions : SubscriptionOptions
{
    [JsonPropertyName(DataFactoryOptionDefinitions.FactoryName)]
    public string? FactoryName { get; set; }
}