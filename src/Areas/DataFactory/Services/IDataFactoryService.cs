// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using AzureMcp.Areas.DataFactory.Models;
using AzureMcp.Options;

namespace AzureMcp.Areas.DataFactory.Services;

/// <summary>
/// Service for interacting with Azure Data Factory resources.
/// </summary>
public interface IDataFactoryService
{
    /// <summary>
    /// Lists all pipelines in the specified Data Factory.
    /// </summary>
    /// <param name="factoryName">The name of the Data Factory.</param>
    /// <param name="resourceGroupName">The resource group containing the Data Factory.</param>
    /// <param name="subscriptionId">The Azure subscription ID.</param>
    /// <param name="tenant">Optional tenant ID for authentication.</param>
    /// <param name="retryPolicy">Optional retry policy for the operation.</param>
    /// <returns>A list of pipeline models.</returns>
    Task<List<PipelineModel>> ListPipelinesAsync(
        string factoryName,
        string resourceGroupName,
        string subscriptionId,
        string? tenant = null,
        RetryPolicyOptions? retryPolicy = null);

    /// <summary>
    /// Runs a pipeline in the specified Data Factory.
    /// </summary>
    /// <param name="factoryName">The name of the Data Factory.</param>
    /// <param name="resourceGroupName">The resource group containing the Data Factory.</param>
    /// <param name="pipelineName">The name of the pipeline to run.</param>
    /// <param name="parameters">Optional parameters to pass to the pipeline.</param>
    /// <param name="subscriptionId">The Azure subscription ID.</param>
    /// <param name="tenant">Optional tenant ID for authentication.</param>
    /// <param name="retryPolicy">Optional retry policy for the operation.</param>
    /// <returns>A pipeline run model containing run details.</returns>
    Task<PipelineRunModel> RunPipelineAsync(
        string factoryName,
        string resourceGroupName,
        string pipelineName,
        Dictionary<string, object>? parameters,
        string subscriptionId,
        string? tenant = null,
        RetryPolicyOptions? retryPolicy = null);

    /// <summary>
    /// Gets the status and details of a specific pipeline run.
    /// </summary>
    /// <param name="factoryName">The name of the Data Factory.</param>
    /// <param name="resourceGroupName">The resource group containing the Data Factory.</param>
    /// <param name="runId">The unique identifier of the pipeline run.</param>
    /// <param name="subscriptionId">The Azure subscription ID.</param>
    /// <param name="tenant">Optional tenant ID for authentication.</param>
    /// <param name="retryPolicy">Optional retry policy for the operation.</param>
    /// <returns>A pipeline run model containing run details.</returns>
    Task<PipelineRunModel> GetPipelineRunAsync(
        string factoryName,
        string resourceGroupName,
        string runId,
        string subscriptionId,
        string? tenant = null,
        RetryPolicyOptions? retryPolicy = null);

    /// <summary>
    /// Lists all datasets in the specified Data Factory.
    /// </summary>
    /// <param name="factoryName">The name of the Data Factory.</param>
    /// <param name="resourceGroupName">The resource group containing the Data Factory.</param>
    /// <param name="subscriptionId">The Azure subscription ID.</param>
    /// <param name="tenant">Optional tenant ID for authentication.</param>
    /// <param name="retryPolicy">Optional retry policy for the operation.</param>
    /// <returns>A list of dataset models.</returns>
    Task<List<DatasetModel>> ListDatasetsAsync(
        string factoryName,
        string resourceGroupName,
        string subscriptionId,
        string? tenant = null,
        RetryPolicyOptions? retryPolicy = null);

    /// <summary>
    /// Lists all linked services in the specified Data Factory.
    /// </summary>
    /// <param name="factoryName">The name of the Data Factory.</param>
    /// <param name="resourceGroupName">The resource group containing the Data Factory.</param>
    /// <param name="subscriptionId">The Azure subscription ID.</param>
    /// <param name="tenant">Optional tenant ID for authentication.</param>
    /// <param name="retryPolicy">Optional retry policy for the operation.</param>
    /// <returns>A list of linked service models.</returns>
    Task<List<LinkedServiceModel>> ListLinkedServicesAsync(
        string factoryName,
        string resourceGroupName,
        string subscriptionId,
        string? tenant = null,
        RetryPolicyOptions? retryPolicy = null);
}