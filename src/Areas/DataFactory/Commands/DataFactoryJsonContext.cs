// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using AzureMcp.Areas.DataFactory.Commands.Dataset;
using AzureMcp.Areas.DataFactory.Commands.LinkedService;
using AzureMcp.Areas.DataFactory.Commands.Pipeline;
using AzureMcp.Areas.DataFactory.Models;

namespace AzureMcp.Areas.DataFactory.Commands;

[JsonSerializable(typeof(ListPipelinesCommand.ListPipelinesCommandResult))]
[JsonSerializable(typeof(RunPipelineCommand.RunPipelineCommandResult))]
[JsonSerializable(typeof(GetPipelineRunCommand.GetPipelineRunCommandResult))]
[JsonSerializable(typeof(ListDatasetsCommand.ListDatasetsCommandResult))]
[JsonSerializable(typeof(ListLinkedServicesCommand.ListLinkedServicesCommandResult))]
[JsonSerializable(typeof(List<PipelineModel>))]
[JsonSerializable(typeof(PipelineRunModel))]
[JsonSerializable(typeof(List<DatasetModel>))]
[JsonSerializable(typeof(List<LinkedServiceModel>))]
[JsonSerializable(typeof(Dictionary<string, object>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
public partial class DataFactoryJsonContext : JsonSerializerContext
{
}