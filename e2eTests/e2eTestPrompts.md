# Azure MCP End-to-End Test Prompts

This file contains prompts used for end-to-end testing to ensure each tool is invoked properly by MCP clients. The tables are organized by Azure MCP Server areas in alphabetical order.

## Azure AI Foundry

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-foundry-models-list | List all AI Foundry models |
| azmcp-foundry-models-list | Show me the available AI Foundry models |
| azmcp-foundry-models-deploy | Deploy a GPT4o instance on my resource \<resource-name> |
| azmcp-foundry-models-deployments-list | List all AI Foundry model deployments |
| azmcp-foundry-models-deployments-list | Show me all AI Foundry model deployments |

## Azure AI Search

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-search-index-describe | Show me the details of the index \<index-name> in Cognitive Search service \<service-name> |
| azmcp-search-index-list | List all indexes in the Cognitive Search service \<service-name> |
| azmcp-search-index-list | Show me the indexes in the Cognitive Search service \<service-name> |
| azmcp-search-index-query | Search for instances of \<search_term> in the index \<index-name> in Cognitive Search service \<service-name> |
| azmcp-search-list | List all Cognitive Search services in my subscription |
| azmcp-search-list | Show me the Cognitive Search services in my subscription |
| azmcp-search-list | Show me my Cognitive Search services |

## Azure App Configuration

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-appconfig-account-list | List all App Configuration stores in my subscription |
| azmcp-appconfig-account-list | Show me the App Configuration stores in my subscription |
| azmcp-appconfig-account-list | Show me my App Configuration stores |
| azmcp-appconfig-kv-delete | Delete the key <key_name> in App Configuration store <app_config_store_name> |
| azmcp-appconfig-kv-list | List all key-value settings in App Configuration store <app_config_store_name> |
| azmcp-appconfig-kv-list | Show me the key-value settings in App Configuration store <app_config_store_name> |
| azmcp-appconfig-kv-lock | Lock the key <key_name> in App Configuration store <app_config_store_name> |
| azmcp-appconfig-kv-set | Set the key <key_name> in App Configuration store <app_config_store_name> to \<value> |
| azmcp-appconfig-kv-show | Show the content for the key <key_name> in App Configuration store <app_config_store_name> |
| azmcp-appconfig-kv-unlock | Unlock the key <key_name> in App Configuration store <app_config_store_name> |

## Azure CLI

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-extension-az | Create a Storage account with name <storage_account_name> |
| azmcp-extension-az | Show me the details of the storage account <account_name> |
| azmcp-extension-az | List all virtual machines in my subscription |

## Azure Cosmos DB

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-cosmos-account-list | List all cosmosdb accounts in my subscription |
| azmcp-cosmos-account-list | Show me the cosmosdb accounts in my subscription |
| azmcp-cosmos-account-list | Show me my cosmosdb accounts |
| azmcp-cosmos-database-container-item-query | Show me the items that contain the word <search_term> in the container <container_name> in the database <database_name> for the cosmosdb account <account_name> |
| azmcp-cosmos-database-container-list | List all the containers in the database <database_name> for the cosmosdb account <account_name> |
| azmcp-cosmos-database-container-list | Show me the containers in the database <database_name> for the cosmosdb account <account_name> |
| azmcp-cosmos-database-list | List all the databases in the cosmosdb account <account_name> |
| azmcp-cosmos-database-list | Show me the databases in the cosmosdb account <account_name> |

## Azure Data Factory

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-datafactory-pipeline-list | List all pipelines in the Data Factory <factory_name> in resource group <resource_group> |
| azmcp-datafactory-pipeline-list | Show me the pipelines in my Data Factory <factory_name> |
| azmcp-datafactory-pipeline-run | Run the pipeline <pipeline_name> in Data Factory <factory_name> |
| azmcp-datafactory-pipeline-run | Execute pipeline <pipeline_name> in Data Factory <factory_name> with parameters {"param1": "value1"} |
| azmcp-datafactory-pipelinerun-get | Get the status of pipeline run <run_id> in Data Factory <factory_name> |
| azmcp-datafactory-pipelinerun-get | Show me the details of pipeline run <run_id> in Data Factory <factory_name> |
| azmcp-datafactory-dataset-list | List all datasets in the Data Factory <factory_name> |
| azmcp-datafactory-dataset-list | Show me the datasets in my Data Factory <factory_name> |
| azmcp-datafactory-linkedservice-list | List all linked services in the Data Factory <factory_name> |
| azmcp-datafactory-linkedservice-list | Show me the linked services in my Data Factory <factory_name> |

## Azure Data Explorer

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-kusto-cluster-get | Show me the details of the Data Explorer cluster <cluster_name> |
| azmcp-kusto-cluster-list | List all Data Explorer clusters in my subscription |
| azmcp-kusto-cluster-list | Show me the Data Explorer clusters in my subscription |
| azmcp-kusto-cluster-list | Show me my Data Explorer clusters |
| azmcp-kusto-database-list | List all databases in the Data Explorer cluster <cluster_name> |
| azmcp-kusto-database-list | Show me the databases in the Data Explorer cluster <cluster_name> |
| azmcp-kusto-query | Show me all items that contain the word <search_term> in the Data Explorer table <table_name> in cluster <cluster_name> |
| azmcp-kusto-sample | Show me a data sample from the Data Explorer table <table_name> in cluster <cluster_name> |
| azmcp-kusto-table-list | List all tables in the Data Explorer database <database_name> in cluster <cluster_name> |
| azmcp-kusto-table-list | Show me the tables in the Data Explorer database <database_name> in cluster <cluster_name> |
| azmcp-kusto-table-schema | Show me the schema for table <table_name> in the Data Explorer database <database_name> in cluster <cluster_name> |

## Azure Database for PostgreSQL

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-postgres-database-list | List all PostgreSQL databases in server \<server> |
| azmcp-postgres-database-list | Show me the PostgreSQL databases in server \<server> |
| azmcp-postgres-database-query | Show me all items that contain the word \<search_term> in the PostgreSQL database \<database> in server \<server> |
| azmcp-postgres-server-config | Show me the configuration of PostgreSQL server \<server> |
| azmcp-postgres-server-list | List all PostgreSQL servers in my subscription |
| azmcp-postgres-server-list | Show me the PostgreSQL servers in my subscription |
| azmcp-postgres-server-list | Show me my PostgreSQL servers |
| azmcp-postgres-server-param | Show me if the parameter my PostgreSQL server \<server> has replication enabled |
| azmcp-postgres-server-setparam | Enable replication for my PostgreSQL server \<server> |
| azmcp-postgres-table-list | List all tables in the PostgreSQL database \<database> in server \<server> |
| azmcp-postgres-table-list | Show me the tables in the PostgreSQL database \<database> in server \<server> |
| azmcp-postgres-table-schema | Show me the schema of table \<table> in the PostgreSQL database \<database> in server \<server> |

## Azure Developer CLI

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-extension-azd | Create a To-Do list web application that uses NodeJS and MongoDB |
| azmcp-extension-azd | Deploy my web application to Azure App Service |

## Azure Key Vault

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-keyvault-key-create | Create a new key called <key_name> with the RSA type in the key vault <key_vault_account_name> |
| azmcp-keyvault-key-get | Show me the details of key <key_name> in the key vault <key_vault_account_name> |
| azmcp-keyvault-key-list | List all keys in the key vault <key_vault_account_name> |
| azmcp-keyvault-key-list | Show me the keys in the key vault <key_vault_account_name> |
| azmcp-keyvault-secret-get | Show me the details about the secret <secret_name> in the key vault <key_vault_account_name> |

## Azure Kubernetes Service (AKS)

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-aks-cluster-list | List all AKS clusters in my subscription |
| azmcp-aks-cluster-list | Show me my Azure Kubernetes Service clusters |
| azmcp-aks-cluster-list | What AKS clusters do I have? |

## Azure Managed Grafana

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-grafana-list | List all Azure Managed Grafana in one subscription |

## Azure MCP Best Practices

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-bestpractices-get | Fetch the latest Azure best practices |
| azmcp-bestpractices-get | Fetch the latest Azure best practices and generate code sample to get a secret from Azure Key Vault |

## Azure MCP Tools

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-tool-list | List all available tools in the Azure MCP server |
| azmcp-tool-list | Show me the available tools in the Azure MCP server |

## Azure Load Testing
| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-loadtesting-testresource-list | List all load testing resources in the resource group <resource-group> in my subscription |
| azmcp-loadtesting-testresource-create | Create a load test resource <load-test-resource-name> in the resource group <resource-group> in my subscription |
| azmcp-loadtesting-test-get | Get the load test with id <test-id> in the load test resource <test-resource> in resource group <resource-group> |
| azmcp-loadtesting-test-create | Create a basic URL test using the following endpoint URL <test-url> that runs for 30 minutes with 45 virtual users. The test name is <sample-name> with the test id <test-id> and the load testing resource is <load-test-resource> in the resource group <resource-group> in my subscription |
| azmcp-loadtesting-testrun-get | Get the load test run with id <testrun-id> in the load test resource <test-resource> in resource group <resource-group> | 
| azmcp-loadtesting-testrun-list |  Get all the load test runs for the test with id <test-id> in the load test resource <test-resource> in resource group <resource-group> |
| azmcp-loadtesting-testrun-create | Create a test run using the id <testrun-id> for test <test-id> in the load testing resource <load-testing-resource> in resource group <resource-group>. Use the name of test run <display-name> and description as <description> |
| azmcp-loadtesting-testrun-update | Update a test run display name as <display-name> for the id <testrun-id> for test <test-id> in the load testing resource <load-testing-resource> in resource group <resource-group>.|

## Azure Monitor

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-monitor-healthmodels-entity-gethealth | Show me the health status of entity <entity_id> in the Log Analytics workspace <workspace_name> |
| azmcp-monitor-metrics-definitions | What metric definitions are available for the Application Insights resource <resource_name> |
| azmcp-monitor-metrics-definitions | Show me all available metrics and their definitions for storage account <account_name> |
| azmcp-monitor-metrics-definitions | Get metric definitions for <resource_type> <resource_name> from the namespace |
| azmcp-monitor-metrics-query | Query the <metric_name> metric for <resource_type> <resource_name> for the last <time_period> |
| azmcp-monitor-metrics-query | What's the request per second rate for my Application Insights resource <resource_name> over the last <time_period> |
| azmcp-monitor-metrics-query | Investigate error rates and failed requests for Application Insights resource <resource_name> for the last <time_period> |
| azmcp-monitor-metrics-query | Analyze the performance trends and response times for Application Insights resource <resource_name> over the last <time_period> |
| azmcp-monitor-metrics-query | Check the availability metrics for my Application Insights resource <resource_name> for the last <time_period> |
| azmcp-monitor-metrics-query | Get the <aggregation_type> <metric_name> metric for <resource_type> <resource_name> over the last <time_period> with intervals |
| azmcp-monitor-resource-log-query | Show me the logs for the past hour for the resource <resource_name> in the Log Analytics workspace <workspace_name> |
| azmcp-monitor-table-list | List all tables in the Log Analytics workspace <workspace_name> |
| azmcp-monitor-table-list | Show me the tables in the Log Analytics workspace <workspace_name> |
| azmcp-monitor-table-type-list | List all available table types in the Log Analytics workspace <workspace_name> |
| azmcp-monitor-table-type-list | Show me the available table types in the Log Analytics workspace <workspace_name> |
| azmcp-monitor-workspace-list | List all Log Analytics workspaces in my subscription |
| azmcp-monitor-workspace-list | Show me the Log Analytics workspaces in my subscription |
| azmcp-monitor-workspace-list | Show me my Log Analytics workspaces |
| azmcp-monitor-workspace-log-query | Show me the logs for the past hour in the Log Analytics workspace <workspace_name> |

## Azure Native ISV

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-datadog-monitoredresources-list | List all monitored resources in the Datadog resource <resource_name> |
| azmcp-datadog-monitoredresources-list | Show me the monitored resources in the Datadog resource <resource_name> |

## Azure RBAC

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-role-assignment-list | List all available role assignments in my subscription |
| azmcp-role-assignment-list | Show me the available role assignments in my subscription |

## Azure Redis

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-redis-cache-list | List all Redis Caches in my subscription |
| azmcp-redis-cache-list | Show me the Redis Caches in my subscription |
| azmcp-redis-cache-list | Show me my Redis Caches |
| azmcp-redis-cache-list-accesspolicy | List all access policies in the Redis Cache <cache_name> |
| azmcp-redis-cache-list-accesspolicy | Show me the access policies in the Redis Cache <cache_name> |
| azmcp-redis-cluster-database-list | List all databases in the Redis Cluster <cluster_name> |
| azmcp-redis-cluster-database-list | Show me the databases in the Redis Cluster <cluster_name> |
| azmcp-redis-cluster-list | List all Redis Clusters in my subscription |
| azmcp-redis-cluster-list | Show me the Redis Clusters in my subscription |
| azmcp-redis-cluster-list | Show me my Redis Clusters |

## Azure Resource Group

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-group-list | List all resource groups in my subscription |
| azmcp-group-list | Show me the resource groups in my subscription |
| azmcp-group-list | Show me my resource groups |

## Azure Service Bus

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-servicebus-queue-details | Show me the details of service bus <service_bus_name> queue <queue_name> |
| azmcp-servicebus-queue-peek | Show me the latest message in service bus <service_bus_name> queue <queue_name> |
| azmcp-servicebus-topic-details | Show me the details of service bus <service_bus_name> topic <topic_name> |
| azmcp-servicebus-topic-subscription-details | Show me the details of service bus <service_bus_name> subscription <subscription_name> |
| azmcp-servicebus-topic-subscription-peek | Show me the latest message in service bus <service_bus_name> subscription <subscription_name> for the topic <topic_name> |

## Azure SQL Database

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-sql-db-show | Show me the details of SQL database <database_name> in server <server_name> |
| azmcp-sql-db-show | Get the configuration details for the SQL database <database_name> on server <server_name> |

## Azure SQL Server Operations

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-sql-server-entra-admin-list | List Microsoft Entra ID administrators for SQL server <server_name> |
| azmcp-sql-server-entra-admin-list | Show me the Entra ID administrators configured for SQL server <server_name> |
| azmcp-sql-server-entra-admin-list | What Microsoft Entra ID administrators are set up for my SQL server <server_name>? |

## Azure Storage

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-storage-account-list | List all storage accounts in my subscription |
| azmcp-storage-account-list | Show me the storage accounts in my subscription |
| azmcp-storage-account-list | Show me my storage accounts |
| azmcp-storage-blob-container-details | Show me the properties of the storage container files in the storage account <account_name> |
| azmcp-storage-blob-container-list | List all blob containers in the storage account <account_name> |
| azmcp-storage-blob-container-list | Show me the blob containers in the storage account <account_name> |
| azmcp-storage-blob-list | List all blobs in the blob container <container_name> in the storage account <account_name> |
| azmcp-storage-blob-list | Show me the blobs in the blob container <container_name> in the storage account <account_name> |
| azmcp-storage-table-list | List all tables in the storage account <account_name> |
| azmcp-storage-table-list | Show me the tables in the storage account <account_name> |

## Azure Subscription Management

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-subscription-list | List all subscriptions for my account |
| azmcp-subscription-list | What subscriptions do I have? |
| azmcp-subscription-list | Show me my subscriptions |
| azmcp-subscription-list | What is my current subscription? |

## Azure Terraform Best Practices

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-azureterraformbestpractices-get | Fetch the Azure Terraform best practices |
| azmcp-azureterraformbestpractices-get | Show me the Azure Terraform best practices and generate code sample to get a secret from Azure Key Vault |

## Bicep

| Tool Name | Test Prompt |
|:----------|:----------|
| azmcp-bicepschema-get | How can I use Bicep to create an Azure OpenAI service? |
