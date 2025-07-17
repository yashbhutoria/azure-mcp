// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ResourceManager;
using Azure.ResourceManager.DataFactory;
using AzureMcp.Areas.DataFactory.Services;
using AzureMcp.Services.Azure.Subscription;
using AzureMcp.Services.Azure.Tenant;
using AzureMcp.Services.Caching;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace AzureMcp.Tests.Areas.DataFactory.UnitTests;

[Trait("Area", "DataFactory")]
public class DataFactoryServiceTests
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ITenantService _tenantService;
    private readonly ICacheService _cacheService;
    private readonly ILoggerFactory _loggerFactory;
    private readonly DataFactoryService _service;

    public DataFactoryServiceTests()
    {
        _subscriptionService = Substitute.For<ISubscriptionService>();
        _tenantService = Substitute.For<ITenantService>();
        _cacheService = Substitute.For<ICacheService>();
        _loggerFactory = Substitute.For<ILoggerFactory>();

        _service = new DataFactoryService(
            _subscriptionService,
            _tenantService,
            _cacheService,
            _loggerFactory);
    }

    [Fact]
    public async Task ListPipelinesAsync_WithCachedData_ReturnsCachedResult()
    {
        // Arrange
        var factoryName = "test-factory";
        var resourceGroup = "test-rg";
        var subscriptionId = "sub123";
        var cachedPipelines = new List<AzureMcp.Areas.DataFactory.Models.PipelineModel>
        {
            new() { Name = "CachedPipeline", Description = "From cache" }
        };

        _cacheService.GetAsync<List<AzureMcp.Areas.DataFactory.Models.PipelineModel>>(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<TimeSpan>())
            .Returns(cachedPipelines);

        // Act
        var result = await _service.ListPipelinesAsync(
            factoryName,
            resourceGroup,
            subscriptionId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("CachedPipeline", result[0].Name);

        // Verify cache was checked
        await _cacheService.Received(1).GetAsync<List<AzureMcp.Areas.DataFactory.Models.PipelineModel>>(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<TimeSpan>());
    }

    [Fact]
    public async Task ListPipelinesAsync_WithNullParameters_ThrowsException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.ListPipelinesAsync(null!, "rg", "sub"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.ListPipelinesAsync("factory", null!, "sub"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.ListPipelinesAsync("factory", "rg", null!));
    }

    [Fact]
    public async Task RunPipelineAsync_WithValidParameters_ReturnsRunId()
    {
        // Arrange
        var factoryName = "test-factory";
        var resourceGroup = "test-rg";
        var subscriptionId = "sub123";
        var pipelineName = "test-pipeline";
        var parameters = new Dictionary<string, object>
        {
            { "param1", "value1" },
            { "param2", 123 }
        };

        var mockSubscription = Substitute.For<Azure.ResourceManager.Resources.SubscriptionResource>();
        var mockArmClient = Substitute.For<ArmClient>();
        var mockDataFactoryResource = Substitute.For<DataFactoryResource>();
        var mockPipelineResource = Substitute.For<DataFactoryPipelineResource>();
        var mockRunResponse = Substitute.For<Azure.Response<DataFactoryCreateRunResult>>();
        var mockCreateRunResult = new DataFactoryCreateRunResult();

        _subscriptionService.GetSubscriptionResourceAsync(subscriptionId)
            .Returns(mockSubscription);
        
        _tenantService.GetArmClientAsync(subscriptionId)
            .Returns(mockArmClient);

        mockArmClient.GetDataFactoryResource(
                Arg.Any<Azure.Core.ResourceIdentifier>())
            .Returns(mockDataFactoryResource);

        mockDataFactoryResource.GetDataFactoryPipeline(pipelineName)
            .Returns(mockPipelineResource);

        mockRunResponse.Value.Returns(mockCreateRunResult);
        mockPipelineResource.CreateRunAsync(
                Azure.WaitUntil.Completed,
                Arg.Any<Azure.ResourceManager.DataFactory.Models.RunFilterContent>())
            .Returns(mockRunResponse);

        // Act
        var result = await _service.RunPipelineAsync(
            factoryName,
            resourceGroup,
            subscriptionId,
            pipelineName,
            parameters);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task RunPipelineAsync_WithNullParameters_ThrowsException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.RunPipelineAsync(null!, "rg", "sub", "pipeline"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.RunPipelineAsync("factory", null!, "sub", "pipeline"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.RunPipelineAsync("factory", "rg", null!, "pipeline"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.RunPipelineAsync("factory", "rg", "sub", null!));
    }

    [Fact]
    public async Task GetPipelineRunAsync_WithValidRunId_ReturnsRunDetails()
    {
        // Arrange
        var factoryName = "test-factory";
        var resourceGroup = "test-rg";
        var subscriptionId = "sub123";
        var runId = Guid.NewGuid().ToString();

        var mockSubscription = Substitute.For<Azure.ResourceManager.Resources.SubscriptionResource>();
        var mockArmClient = Substitute.For<ArmClient>();
        var mockDataFactoryResource = Substitute.For<DataFactoryResource>();
        var mockPipelineRun = Substitute.For<Azure.ResourceManager.DataFactory.Models.PipelineRun>();

        _subscriptionService.GetSubscriptionResourceAsync(subscriptionId)
            .Returns(mockSubscription);
        
        _tenantService.GetArmClientAsync(subscriptionId)
            .Returns(mockArmClient);

        mockArmClient.GetDataFactoryResource(
                Arg.Any<Azure.Core.ResourceIdentifier>())
            .Returns(mockDataFactoryResource);

        mockDataFactoryResource.GetPipelineRunAsync(runId)
            .Returns(Azure.Response.FromValue(mockPipelineRun, Substitute.For<Azure.Response>()));

        // Act
        var result = await _service.GetPipelineRunAsync(
            factoryName,
            resourceGroup,
            subscriptionId,
            runId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetPipelineRunAsync_WithNullParameters_ThrowsException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.GetPipelineRunAsync(null!, "rg", "sub", "runId"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.GetPipelineRunAsync("factory", null!, "sub", "runId"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.GetPipelineRunAsync("factory", "rg", null!, "runId"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.GetPipelineRunAsync("factory", "rg", "sub", null!));
    }

    [Fact]
    public async Task ListDatasetsAsync_WithCachedData_ReturnsCachedResult()
    {
        // Arrange
        var factoryName = "test-factory";
        var resourceGroup = "test-rg";
        var subscriptionId = "sub123";
        var cachedDatasets = new List<AzureMcp.Areas.DataFactory.Models.DatasetModel>
        {
            new() { Name = "CachedDataset", Type = "Dataset" }
        };

        _cacheService.GetAsync<List<AzureMcp.Areas.DataFactory.Models.DatasetModel>>(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<TimeSpan>())
            .Returns(cachedDatasets);

        // Act
        var result = await _service.ListDatasetsAsync(
            factoryName,
            resourceGroup,
            subscriptionId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("CachedDataset", result[0].Name);
    }

    [Fact]
    public async Task ListDatasetsAsync_WithNullParameters_ThrowsException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.ListDatasetsAsync(null!, "rg", "sub"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.ListDatasetsAsync("factory", null!, "sub"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.ListDatasetsAsync("factory", "rg", null!));
    }

    [Fact]
    public async Task ListLinkedServicesAsync_WithCachedData_ReturnsCachedResult()
    {
        // Arrange
        var factoryName = "test-factory";
        var resourceGroup = "test-rg";
        var subscriptionId = "sub123";
        var cachedLinkedServices = new List<AzureMcp.Areas.DataFactory.Models.LinkedServiceModel>
        {
            new() { Name = "CachedLinkedService", Type = "LinkedService" }
        };

        _cacheService.GetAsync<List<AzureMcp.Areas.DataFactory.Models.LinkedServiceModel>>(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<TimeSpan>())
            .Returns(cachedLinkedServices);

        // Act
        var result = await _service.ListLinkedServicesAsync(
            factoryName,
            resourceGroup,
            subscriptionId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("CachedLinkedService", result[0].Name);
    }

    [Fact]
    public async Task ListLinkedServicesAsync_WithNullParameters_ThrowsException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.ListLinkedServicesAsync(null!, "rg", "sub"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.ListLinkedServicesAsync("factory", null!, "sub"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.ListLinkedServicesAsync("factory", "rg", null!));
    }
}