// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json;
using Azure.ResourceManager.DataFactory;
using Azure.ResourceManager.DataFactory.Models;
using AzureMcp.Areas.DataFactory.Commands;
using AzureMcp.Areas.DataFactory.Models;
using AzureMcp.Options;
using AzureMcp.Services.Azure;
using AzureMcp.Services.Azure.Subscription;
using AzureMcp.Services.Azure.Tenant;
using AzureMcp.Services.Caching;
using Microsoft.Extensions.Logging;

namespace AzureMcp.Areas.DataFactory.Services;

public class DataFactoryService : BaseAzureService, IDataFactoryService
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ICacheService _cacheService;
    private readonly ILogger<DataFactoryService>? _logger;
    private const string CacheGroup = "datafactory";
    private static readonly TimeSpan s_cacheDuration = TimeSpan.FromMinutes(15);

    public DataFactoryService(
        ISubscriptionService subscriptionService,
        ITenantService tenantService,
        ICacheService cacheService,
        ILoggerFactory? loggerFactory = null)
        : base(tenantService, loggerFactory)
    {
        _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        _logger = loggerFactory?.CreateLogger<DataFactoryService>();
    }

    public async Task<List<PipelineModel>> ListPipelinesAsync(
        string factoryName,
        string resourceGroupName,
        string subscriptionId,
        string? tenant = null,
        RetryPolicyOptions? retryPolicy = null)
    {
        ValidateRequiredParameters(subscriptionId, resourceGroupName, factoryName);

        var cacheKey = $"pipelines_{subscriptionId}_{resourceGroupName}_{factoryName}";
        var cached = await _cacheService.GetAsync<List<PipelineModel>>(CacheGroup, cacheKey, s_cacheDuration);
        if (cached != null)
        {
            return cached;
        }

        var armClient = await CreateArmClientAsync(tenant, retryPolicy);
        var subscription = await _subscriptionService.GetSubscription(subscriptionId, tenant, retryPolicy);
        
        var factoryId = DataFactoryResource.CreateResourceIdentifier(
            subscription.Id,
            resourceGroupName,
            factoryName);

        var factory = armClient.GetDataFactoryResource(factoryId);
        var pipelines = factory.GetDataFactoryPipelines();

        var result = new List<PipelineModel>();
        await foreach (var pipeline in pipelines)
        {
            result.Add(MapToPipelineModel(pipeline));
        }

        await _cacheService.SetAsync(CacheGroup, cacheKey, result);
        return result;
    }

    public async Task<PipelineRunModel> RunPipelineAsync(
        string factoryName,
        string resourceGroupName,
        string pipelineName,
        Dictionary<string, object>? parameters,
        string subscriptionId,
        string? tenant = null,
        RetryPolicyOptions? retryPolicy = null)
    {
        ValidateRequiredParameters(subscriptionId, resourceGroupName, factoryName, pipelineName);

        var armClient = await CreateArmClientAsync(tenant, retryPolicy);
        var subscription = await _subscriptionService.GetSubscription(subscriptionId, tenant, retryPolicy);
        
        var factoryId = DataFactoryResource.CreateResourceIdentifier(
            subscription.Id,
            resourceGroupName,
            factoryName);

        var factory = armClient.GetDataFactoryResource(factoryId);
        var pipeline = await factory.GetDataFactoryPipelineAsync(pipelineName);

        // Convert parameters to dictionary format for CreateRunAsync
        IDictionary<string, BinaryData>? runParameters = null;
        if (parameters != null && parameters.Count > 0)
        {
            runParameters = new Dictionary<string, BinaryData>();
            foreach (var param in parameters)
            {
                var json = JsonSerializer.Serialize(param.Value, param.Value?.GetType() ?? typeof(object), DataFactoryJsonContext.Default);
                runParameters[param.Key] = BinaryData.FromString(json);
            }
        }

        var runResponse = await pipeline.Value.CreateRunAsync(runParameters);
        var runId = runResponse.Value.RunId.ToString();

        // Get the run details
        var run = await factory.GetPipelineRunAsync(runId);
        return MapToPipelineRunModel(run.Value);
    }

    public async Task<PipelineRunModel> GetPipelineRunAsync(
        string factoryName,
        string resourceGroupName,
        string runId,
        string subscriptionId,
        string? tenant = null,
        RetryPolicyOptions? retryPolicy = null)
    {
        ValidateRequiredParameters(subscriptionId, resourceGroupName, factoryName, runId);

        var armClient = await CreateArmClientAsync(tenant, retryPolicy);
        var subscription = await _subscriptionService.GetSubscription(subscriptionId, tenant, retryPolicy);
        
        var factoryId = DataFactoryResource.CreateResourceIdentifier(
            subscription.Id,
            resourceGroupName,
            factoryName);

        var factory = armClient.GetDataFactoryResource(factoryId);
        var run = await factory.GetPipelineRunAsync(runId);
        
        return MapToPipelineRunModel(run.Value);
    }

    public async Task<List<DatasetModel>> ListDatasetsAsync(
        string factoryName,
        string resourceGroupName,
        string subscriptionId,
        string? tenant = null,
        RetryPolicyOptions? retryPolicy = null)
    {
        ValidateRequiredParameters(subscriptionId, resourceGroupName, factoryName);

        var cacheKey = $"datasets_{subscriptionId}_{resourceGroupName}_{factoryName}";
        var cached = await _cacheService.GetAsync<List<DatasetModel>>(CacheGroup, cacheKey, s_cacheDuration);
        if (cached != null)
        {
            return cached;
        }

        var armClient = await CreateArmClientAsync(tenant, retryPolicy);
        var subscription = await _subscriptionService.GetSubscription(subscriptionId, tenant, retryPolicy);
        
        var factoryId = DataFactoryResource.CreateResourceIdentifier(
            subscription.Id,
            resourceGroupName,
            factoryName);

        var factory = armClient.GetDataFactoryResource(factoryId);
        var datasets = factory.GetDataFactoryDatasets();

        var result = new List<DatasetModel>();
        await foreach (var dataset in datasets)
        {
            result.Add(MapToDatasetModel(dataset));
        }

        await _cacheService.SetAsync(CacheGroup, cacheKey, result);
        return result;
    }

    public async Task<List<LinkedServiceModel>> ListLinkedServicesAsync(
        string factoryName,
        string resourceGroupName,
        string subscriptionId,
        string? tenant = null,
        RetryPolicyOptions? retryPolicy = null)
    {
        ValidateRequiredParameters(subscriptionId, resourceGroupName, factoryName);

        var cacheKey = $"linkedservices_{subscriptionId}_{resourceGroupName}_{factoryName}";
        var cached = await _cacheService.GetAsync<List<LinkedServiceModel>>(CacheGroup, cacheKey, s_cacheDuration);
        if (cached != null)
        {
            return cached;
        }

        var armClient = await CreateArmClientAsync(tenant, retryPolicy);
        var subscription = await _subscriptionService.GetSubscription(subscriptionId, tenant, retryPolicy);
        
        var factoryId = DataFactoryResource.CreateResourceIdentifier(
            subscription.Id,
            resourceGroupName,
            factoryName);

        var factory = armClient.GetDataFactoryResource(factoryId);
        var linkedServices = factory.GetDataFactoryLinkedServices();

        var result = new List<LinkedServiceModel>();
        await foreach (var linkedService in linkedServices)
        {
            result.Add(MapToLinkedServiceModel(linkedService));
        }

        await _cacheService.SetAsync(CacheGroup, cacheKey, result);
        return result;
    }

    private static PipelineModel MapToPipelineModel(DataFactoryPipelineResource pipeline)
    {
        var pipelineData = pipeline.Data;
        var model = new PipelineModel
        {
            Name = pipeline.Data.Name,
            Description = pipelineData.Description
        };

        // Extract parameters if available
        if (pipelineData.Parameters != null && pipelineData.Parameters.Count > 0)
        {
            model.Parameters = new Dictionary<string, object>();
            foreach (var param in pipelineData.Parameters)
            {
                // Use JsonSerializer to deserialize the default value if it exists
                if (param.Value.DefaultValue != null)
                {
                    try
                    {
                        var json = param.Value.DefaultValue.ToString();
                        model.Parameters[param.Key] = JsonSerializer.Deserialize(json ?? string.Empty, typeof(object), DataFactoryJsonContext.Default) ?? string.Empty;
                    }
                    catch
                    {
                        // If deserialization fails, use the raw value
                        model.Parameters[param.Key] = param.Value.DefaultValue.ToString() ?? string.Empty;
                    }
                }
                else
                {
                    model.Parameters[param.Key] = new { Type = param.Value.ParameterType.ToString() };
                }
            }
        }

        return model;
    }

    private static PipelineRunModel MapToPipelineRunModel(DataFactoryPipelineRunInfo run)
    {
        var model = new PipelineRunModel
        {
            RunId = run.RunId?.ToString() ?? "Unknown",
            PipelineName = run.PipelineName ?? "Unknown",
            Status = run.Status ?? "Unknown",
            RunStart = run.LastUpdatedOn?.DateTime,
            RunEnd = null, // RunEnd is not available in DataFactoryPipelineRunInfo
            Message = run.Message
        };

        // Calculate duration based on LastUpdatedOn since RunEnd is not available
        if (run.LastUpdatedOn.HasValue)
        {
            // If status is not in a terminal state, calculate current duration
            if (run.Status != "Succeeded" && run.Status != "Failed" && run.Status != "Cancelled")
            {
                model.DurationInMs = (long)(DateTimeOffset.UtcNow - run.LastUpdatedOn.Value).TotalMilliseconds;
            }
            else
            {
                // For completed runs, we can't calculate exact duration without RunEnd
                // So we'll leave it as null
                model.DurationInMs = null;
            }
        }

        // Extract parameters
        if (run.Parameters != null && run.Parameters.Count > 0)
        {
            model.Parameters = new Dictionary<string, string>();
            foreach (var param in run.Parameters)
            {
                model.Parameters[param.Key] = param.Value?.ToString() ?? string.Empty;
            }
        }

        return model;
    }

    private static DatasetModel MapToDatasetModel(DataFactoryDatasetResource dataset)
    {
        var datasetData = dataset.Data;
        var model = new DatasetModel
        {
            Name = dataset.Data.Name,
            Type = "Dataset" // DatasetType is not directly available, using generic type
        };

        // Extract linked service name
        if (datasetData.Properties.LinkedServiceName != null)
        {
            model.LinkedServiceName = datasetData.Properties.LinkedServiceName.ReferenceName;
        }

        // TypeProperties is not directly available in the new SDK
        // FolderPath would need to be extracted from the dataset's specific properties

        // Schema information would need to be extracted differently in the new SDK
        // For now, leaving it null

        return model;
    }

    private static LinkedServiceModel MapToLinkedServiceModel(DataFactoryLinkedServiceResource linkedService)
    {
        var linkedServiceData = linkedService.Data;
        var model = new LinkedServiceModel
        {
            Name = linkedService.Data.Name,
            Type = "LinkedService", // LinkedServiceType is not directly available
            Description = null // Description is not available on DataFactoryLinkedServiceData
        };

        // Extract integration runtime reference
        if (linkedServiceData.Properties.ConnectVia != null)
        {
            model.ConnectVia = linkedServiceData.Properties.ConnectVia.ReferenceName;
        }

        return model;
    }

    private static new void ValidateRequiredParameters(params string?[] parameters)
    {
        foreach (var param in parameters)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                throw new ArgumentException("Required parameter is missing or empty");
            }
        }
    }
}