@minLength(6)
@maxLength(20)
@description('The base resource name.')
param baseName string = resourceGroup().name

@description('The location of the resource. By default, this is the same as the resource group.')
param location string = resourceGroup().location

@description('The tenant ID to which the application and resources belong.')
param tenantId string = tenant().tenantId

@description('The client OID to grant access to test resources.')
param testApplicationOid string

var factoryName = baseName

resource factory 'Microsoft.DataFactory/factories@2018-06-01' = {
  name: factoryName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {}
}

// Create a sample pipeline for testing
resource pipeline 'Microsoft.DataFactory/factories/pipelines@2018-06-01' = {
  parent: factory
  name: 'test-pipeline'
  properties: {
    description: 'A test pipeline for MCP testing'
    parameters: {
      testParam: {
        type: 'String'
        defaultValue: 'defaultValue'
      }
    }
    activities: [
      {
        name: 'Wait1'
        type: 'Wait'
        typeProperties: {
          waitTimeInSeconds: 1
        }
      }
    ]
  }
}

// Create a sample linked service
resource linkedService 'Microsoft.DataFactory/factories/linkedservices@2018-06-01' = {
  parent: factory
  name: 'test-storage'
  properties: {
    type: 'AzureBlobStorage'
    typeProperties: {
      connectionString: 'DefaultEndpointsProtocol=https;AccountName=dummystorage;EndpointSuffix=core.windows.net'
    }
  }
}

// Create a sample dataset
resource dataset 'Microsoft.DataFactory/factories/datasets@2018-06-01' = {
  parent: factory
  name: 'test-dataset'
  properties: {
    linkedServiceName: {
      referenceName: linkedService.name
      type: 'LinkedServiceReference'
    }
    type: 'Binary'
    typeProperties: {
      location: {
        type: 'AzureBlobStorageLocation'
        container: 'test-container'
      }
    }
  }
}

// Grant the test application access to the factory
resource roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(factory.id, testApplicationOid, 'datafactory-contributor')
  scope: factory
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '673868aa-7521-48a0-acc6-0f60742d39f5') // Data Factory Contributor
    principalId: testApplicationOid
    principalType: 'ServicePrincipal'
  }
}

output factoryName string = factory.name