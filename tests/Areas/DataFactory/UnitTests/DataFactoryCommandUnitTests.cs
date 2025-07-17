// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using AzureMcp.Areas.DataFactory.Commands;
using AzureMcp.Areas.DataFactory.Models;
using AzureMcp.Areas.DataFactory.Options;
using AzureMcp.Areas.DataFactory.Services;
using AzureMcp.Tests.Helpers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace AzureMcp.Tests.Areas.DataFactory.UnitTests;

[Trait("Area", "DataFactory")]
public class DataFactoryCommandUnitTests
{
    private readonly IDataFactoryService _dataFactoryService;
    private readonly ILoggerFactory _loggerFactory;

    public DataFactoryCommandUnitTests()
    {
        _dataFactoryService = Substitute.For<IDataFactoryService>();
        _loggerFactory = Substitute.For<ILoggerFactory>();
    }

    [Fact]
    public async Task PipelineListCommand_WithValidOptions_ReturnsSuccess()
    {
        // Arrange
        var options = new DataFactoryOptions
        {
            Factory = "test-factory",
            ResourceGroup = "test-rg",
            Subscription = "sub123"
        };

        var expectedPipelines = new List<PipelineModel>
        {
            new() { Name = "Pipeline1", Description = "Test pipeline 1" },
            new() { Name = "Pipeline2", Description = "Test pipeline 2" }
        };

        _dataFactoryService.ListPipelinesAsync(
            options.Factory,
            options.ResourceGroup,
            options.Subscription)
            .Returns(expectedPipelines);

        var command = new PipelineListCommand(_dataFactoryService, _loggerFactory);

        // Act
        var result = await command.ExecuteAsync(options);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Content);
        
        var response = AssertHelper.ParseContent<PipelineListResponse>(result.Content);
        Assert.Equal(2, response.Pipelines.Count);
        Assert.Equal("Pipeline1", response.Pipelines[0].Name);
        Assert.Equal("Pipeline2", response.Pipelines[1].Name);
    }

    [Fact]
    public async Task PipelineRunCommand_WithValidOptions_ReturnsRunId()
    {
        // Arrange
        var options = new PipelineRunOptions
        {
            Factory = "test-factory",
            ResourceGroup = "test-rg",
            Subscription = "sub123",
            Pipeline = "test-pipeline",
            Parameters = "{\"param1\": \"value1\"}"
        };

        var expectedRunId = "run-123";

        _dataFactoryService.RunPipelineAsync(
            options.Factory,
            options.ResourceGroup,
            options.Subscription,
            options.Pipeline,
            Arg.Any<Dictionary<string, object>>())
            .Returns(expectedRunId);

        var command = new PipelineRunCommand(_dataFactoryService, _loggerFactory);

        // Act
        var result = await command.ExecuteAsync(options);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Content);
        
        var response = AssertHelper.ParseContent<PipelineRunResponse>(result.Content);
        Assert.Equal(expectedRunId, response.RunId);
    }

    [Fact]
    public async Task PipelineRunGetCommand_WithValidOptions_ReturnsRunDetails()
    {
        // Arrange
        var options = new PipelineRunGetOptions
        {
            Factory = "test-factory",
            ResourceGroup = "test-rg",
            Subscription = "sub123",
            RunId = "run-123"
        };

        var expectedRun = new PipelineRunModel
        {
            RunId = "run-123",
            PipelineName = "test-pipeline",
            Status = "InProgress",
            Message = "Running",
            RunStart = DateTimeOffset.UtcNow.DateTime,
            RunEnd = null,
            DurationInMs = null
        };

        _dataFactoryService.GetPipelineRunAsync(
            options.Factory,
            options.ResourceGroup,
            options.Subscription,
            options.RunId)
            .Returns(expectedRun);

        var command = new PipelineRunGetCommand(_dataFactoryService, _loggerFactory);

        // Act
        var result = await command.ExecuteAsync(options);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Content);
        
        var response = AssertHelper.ParseContent<PipelineRunModel>(result.Content);
        Assert.Equal("run-123", response.RunId);
        Assert.Equal("InProgress", response.Status);
    }

    [Fact]
    public async Task DatasetListCommand_WithValidOptions_ReturnsSuccess()
    {
        // Arrange
        var options = new DataFactoryOptions
        {
            Factory = "test-factory",
            ResourceGroup = "test-rg",
            Subscription = "sub123"
        };

        var expectedDatasets = new List<DatasetModel>
        {
            new() { Name = "Dataset1", Type = "Dataset", Schema = null },
            new() { Name = "Dataset2", Type = "Dataset", Schema = null }
        };

        _dataFactoryService.ListDatasetsAsync(
            options.Factory,
            options.ResourceGroup,
            options.Subscription)
            .Returns(expectedDatasets);

        var command = new DatasetListCommand(_dataFactoryService, _loggerFactory);

        // Act
        var result = await command.ExecuteAsync(options);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Content);
        
        var response = AssertHelper.ParseContent<DatasetListResponse>(result.Content);
        Assert.Equal(2, response.Datasets.Count);
        Assert.Equal("Dataset1", response.Datasets[0].Name);
        Assert.Equal("Dataset2", response.Datasets[1].Name);
    }

    [Fact]
    public async Task LinkedServiceListCommand_WithValidOptions_ReturnsSuccess()
    {
        // Arrange
        var options = new DataFactoryOptions
        {
            Factory = "test-factory",
            ResourceGroup = "test-rg",
            Subscription = "sub123"
        };

        var expectedLinkedServices = new List<LinkedServiceModel>
        {
            new() { Name = "LinkedService1", Type = "LinkedService" },
            new() { Name = "LinkedService2", Type = "LinkedService" }
        };

        _dataFactoryService.ListLinkedServicesAsync(
            options.Factory,
            options.ResourceGroup,
            options.Subscription)
            .Returns(expectedLinkedServices);

        var command = new LinkedServiceListCommand(_dataFactoryService, _loggerFactory);

        // Act
        var result = await command.ExecuteAsync(options);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Content);
        
        var response = AssertHelper.ParseContent<LinkedServiceListResponse>(result.Content);
        Assert.Equal(2, response.LinkedServices.Count);
        Assert.Equal("LinkedService1", response.LinkedServices[0].Name);
        Assert.Equal("LinkedService2", response.LinkedServices[1].Name);
    }

    [Fact]
    public async Task PipelineRunCommand_WithInvalidJson_ReturnsError()
    {
        // Arrange
        var options = new PipelineRunOptions
        {
            Factory = "test-factory",
            ResourceGroup = "test-rg",
            Subscription = "sub123",
            Pipeline = "test-pipeline",
            Parameters = "invalid-json"
        };

        var command = new PipelineRunCommand(_dataFactoryService, _loggerFactory);

        // Act
        var result = await command.ExecuteAsync(options);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Contains("Failed to parse parameters", result.GetError());
    }
}