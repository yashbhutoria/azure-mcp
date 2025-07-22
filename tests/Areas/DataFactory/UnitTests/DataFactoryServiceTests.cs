// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

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
    public async Task RunPipelineAsync_WithNullParameters_ThrowsException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.RunPipelineAsync(null!, "rg", "pipeline", null, "sub"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.RunPipelineAsync("factory", null!, "pipeline", null, "sub"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.RunPipelineAsync("factory", "rg", null!, null, "sub"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.RunPipelineAsync("factory", "rg", "pipeline", null, null!));
    }

    [Fact]
    public async Task GetPipelineRunAsync_WithNullParameters_ThrowsException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.GetPipelineRunAsync(null!, "rg", "runId", "sub"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.GetPipelineRunAsync("factory", null!, "runId", "sub"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.GetPipelineRunAsync("factory", "rg", null!, "sub"));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.GetPipelineRunAsync("factory", "rg", "runId", null!));
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
