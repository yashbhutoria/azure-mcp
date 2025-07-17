// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.CommandLine.Parsing;
using AzureMcp.Areas.DataFactory.Commands.Pipeline;
using AzureMcp.Areas.DataFactory.Models;
using AzureMcp.Areas.DataFactory.Services;
using AzureMcp.Models.Command;
using AzureMcp.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace AzureMcp.Tests.Areas.DataFactory.UnitTests.Pipeline;

[Trait("Area", "DataFactory")]
public class ListPipelinesCommandTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDataFactoryService _dataFactoryService;
    private readonly ILogger<ListPipelinesCommand> _logger;
    private readonly ListPipelinesCommand _command;
    private readonly CommandContext _context;
    private readonly Parser _parser;

    public ListPipelinesCommandTests()
    {
        _dataFactoryService = Substitute.For<IDataFactoryService>();
        _logger = Substitute.For<ILogger<ListPipelinesCommand>>();

        var collection = new ServiceCollection().AddSingleton(_dataFactoryService);

        _serviceProvider = collection.BuildServiceProvider();
        _command = new(_logger);
        _context = new(_serviceProvider);
        _parser = new(_command.GetCommand());
    }

    [Fact]
    public async Task ExecuteAsync_WithValidFactory_ReturnsPipelines()
    {
        // Arrange
        var subscriptionId = "sub123";
        var resourceGroup = "test-rg";
        var factoryName = "test-factory";
        var expectedPipelines = new List<PipelineModel>
        {
            new() { Name = "Pipeline1", Description = "Test pipeline 1" },
            new() { Name = "Pipeline2", Description = "Test pipeline 2" }
        };

        _dataFactoryService.ListPipelinesAsync(
            Arg.Is(factoryName),
            Arg.Is(resourceGroup),
            Arg.Is(subscriptionId),
            Arg.Any<string>(),
            Arg.Any<RetryPolicyOptions>())
            .Returns(expectedPipelines);

        var parseResult = _parser.Parse($"list --subscription {subscriptionId} --resource-group {resourceGroup} --factory-name {factoryName}");

        // Act
        var result = await _command.ExecuteAsync(_context, parseResult);

        // Assert
        Assert.Equal(200, result.Status);
        Assert.NotNull(result.Results);
    }

    [Fact]
    public async Task ExecuteAsync_WithEmptyFactory_ReturnsEmptyList()
    {
        // Arrange
        var subscriptionId = "sub123";
        var resourceGroup = "test-rg";
        var factoryName = "empty-factory";
        var expectedPipelines = new List<PipelineModel>();

        _dataFactoryService.ListPipelinesAsync(
            Arg.Is(factoryName),
            Arg.Is(resourceGroup),
            Arg.Is(subscriptionId),
            Arg.Any<string>(),
            Arg.Any<RetryPolicyOptions>())
            .Returns(expectedPipelines);

        var parseResult = _parser.Parse($"list --subscription {subscriptionId} --resource-group {resourceGroup} --factory-name {factoryName}");

        // Act
        var result = await _command.ExecuteAsync(_context, parseResult);

        // Assert
        Assert.Equal(200, result.Status);
        Assert.Null(result.Results);
        Assert.Contains("No pipelines found", result.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ServiceThrowsException_HandlesError()
    {
        // Arrange
        var subscriptionId = "sub123";
        var resourceGroup = "test-rg";
        var factoryName = "test-factory";

        _dataFactoryService.ListPipelinesAsync(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<RetryPolicyOptions>())
            .ThrowsAsync(new InvalidOperationException("Factory not found"));

        var parseResult = _parser.Parse($"list --subscription {subscriptionId} --resource-group {resourceGroup} --factory-name {factoryName}");

        // Act
        var result = await _command.ExecuteAsync(_context, parseResult);

        // Assert
        Assert.Equal(500, result.Status);
        Assert.Contains("Factory not found", result.Message);
    }

    [Fact]
    public async Task ExecuteAsync_MissingRequiredOptions_ReturnsError()
    {
        // Arrange
        var parseResult = _parser.Parse("list --subscription sub123");

        // Act
        var result = await _command.ExecuteAsync(_context, parseResult);

        // Assert
        Assert.Equal(400, result.Status);
        Assert.Contains("required", result.Message, StringComparison.OrdinalIgnoreCase);
    }
}