// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json;
using AzureMcp.Tests.Client;
using AzureMcp.Tests.Client.Helpers;
using Xunit;

namespace AzureMcp.Tests.Areas.DataFactory.LiveTests;

[Trait("Area", "DataFactory")]
public class DataFactoryCommandTests(LiveTestFixture liveTestFixture, ITestOutputHelper output)
    : CommandTestsBase(liveTestFixture, output),
    IClassFixture<LiveTestFixture>
{
    [Fact]
    [Trait("Category", "Live")]
    public async Task Should_list_pipelines()
    {
        var result = await CallToolAsync(
            "azmcp_datafactory_pipeline_list",
            new()
            {
                { "subscription", Settings.SubscriptionId },
                { "resource-group", Settings.ResourceGroupName },
                { "factory", Settings.ResourceBaseName }
            });

        var pipelines = result.AssertProperty("pipelines");
        Assert.Equal(JsonValueKind.Array, pipelines.ValueKind);
    }

    [Fact]
    [Trait("Category", "Live")]
    public async Task Should_list_datasets()
    {
        var result = await CallToolAsync(
            "azmcp_datafactory_dataset_list",
            new()
            {
                { "subscription", Settings.SubscriptionId },
                { "resource-group", Settings.ResourceGroupName },
                { "factory", Settings.ResourceBaseName }
            });

        var datasets = result.AssertProperty("datasets");
        Assert.Equal(JsonValueKind.Array, datasets.ValueKind);
    }

    [Fact]
    [Trait("Category", "Live")]
    public async Task Should_list_linked_services()
    {
        var result = await CallToolAsync(
            "azmcp_datafactory_linkedservice_list",
            new()
            {
                { "subscription", Settings.SubscriptionId },
                { "resource-group", Settings.ResourceGroupName },
                { "factory", Settings.ResourceBaseName }
            });

        var linkedServices = result.AssertProperty("linkedServices");
        Assert.Equal(JsonValueKind.Array, linkedServices.ValueKind);
    }

    [Fact]
    [Trait("Category", "Live")]
    public async Task Should_run_pipeline_with_parameters()
    {
        // First, create a simple pipeline to test with
        var pipelines = await CallToolAsync(
            "azmcp_datafactory_pipeline_list",
            new()
            {
                { "subscription", Settings.SubscriptionId },
                { "resource-group", Settings.ResourceGroupName },
                { "factory", Settings.ResourceBaseName }
            });

        var pipelineArray = pipelines.AssertProperty("pipelines").EnumerateArray();
        if (!pipelineArray.Any())
        {
            // Skip test if no pipelines exist
            Output.WriteLine("No pipelines found in Data Factory, skipping run test");
            return;
        }

        var firstPipeline = pipelineArray.First();
        var pipelineName = firstPipeline.GetProperty("name").GetString();

        var result = await CallToolAsync(
            "azmcp_datafactory_pipeline_run",
            new()
            {
                { "subscription", Settings.SubscriptionId },
                { "resource-group", Settings.ResourceGroupName },
                { "factory", Settings.ResourceBaseName },
                { "pipeline", pipelineName },
                { "parameters", "{\"testParam\": \"testValue\"}" }
            });

        var runId = result.AssertProperty("runId");
        Assert.Equal(JsonValueKind.String, runId.ValueKind);
        Assert.NotEmpty(runId.GetString()!);
    }

    [Fact]
    [Trait("Category", "Live")]
    public async Task Should_get_pipeline_run_status()
    {
        // First, run a pipeline to get a run ID
        var pipelines = await CallToolAsync(
            "azmcp_datafactory_pipeline_list",
            new()
            {
                { "subscription", Settings.SubscriptionId },
                { "resource-group", Settings.ResourceGroupName },
                { "factory", Settings.ResourceBaseName }
            });

        var pipelineArray = pipelines.AssertProperty("pipelines").EnumerateArray();
        if (!pipelineArray.Any())
        {
            // Skip test if no pipelines exist
            Output.WriteLine("No pipelines found in Data Factory, skipping run status test");
            return;
        }

        var firstPipeline = pipelineArray.First();
        var pipelineName = firstPipeline.GetProperty("name").GetString();

        var runResult = await CallToolAsync(
            "azmcp_datafactory_pipeline_run",
            new()
            {
                { "subscription", Settings.SubscriptionId },
                { "resource-group", Settings.ResourceGroupName },
                { "factory", Settings.ResourceBaseName },
                { "pipeline", pipelineName }
            });

        var runId = runResult.AssertProperty("runId").GetString();

        // Now get the run status
        var statusResult = await CallToolAsync(
            "azmcp_datafactory_pipelinerun_get",
            new()
            {
                { "subscription", Settings.SubscriptionId },
                { "resource-group", Settings.ResourceGroupName },
                { "factory", Settings.ResourceBaseName },
                { "run-id", runId }
            });

        var status = statusResult.AssertProperty("status");
        Assert.Equal(JsonValueKind.String, status.ValueKind);
        Assert.NotEmpty(status.GetString()!);

        var runIdFromStatus = statusResult.AssertProperty("runId");
        Assert.Equal(runId, runIdFromStatus.GetString());
    }
}