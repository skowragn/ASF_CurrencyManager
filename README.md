# Azure Service Fabric - Currency Manager

## 1. Azure Service Fabric Overview
Azure Service Fabric is a distributed systems platform used to build hyper-scalable, reliable and easily managed applications for the cloud.

Nowadays it powers many Azure Cloud and Microsoft services.

![image](https://user-images.githubusercontent.com/97020391/151328873-d70de507-f41d-4090-88a9-70e877ef8cee.png)


- Azure SQL Database
- Azure Cosmos DB
- Azure IoT Hub
- Azure Event Hub
- Skype
- Cortana
- Intune
- Dynamics
- Power BI
- Azure Database for MySQL
- Azure Database for PostgreSQL
- Azure Container Registry
- Azure Event Grid
- Azure Stream Analytics
- Azure DevOps (formerly Visual Studio Team Services)
- Azure Monitor
- Core Azure Services
- Azure Archive Storage


![image](https://user-images.githubusercontent.com/97020391/151329150-a071d8d0-a540-43b9-b01d-250f5526c4d9.png)



## 2. Azure Service Fabric Version
https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started
![image](https://user-images.githubusercontent.com/97020391/147914744-e0bc9a27-fc21-42ad-85fa-dca359a0afc4.png)
![image](https://user-images.githubusercontent.com/97020391/147914751-bbf35faa-3563-4ee2-8b3b-e8a298d063eb.png)

## 3. Azure Service Fabric - programming model
Azure Service Fabric cluster is on .NET Framework 4.8. 
It can run the folowing services/processes:
- Reliable service (stateful and stateless) with .NET 6.0
- ASP.NET Core services (stateful and stateless) with  ASP.NET Core 5/.NET 5 (Web API, MVC, with Angular/React etc.)
- Reliable Actors (reliable stateful services) with .NET Framework 4.8
- Guest executable - self-contained applications (e.g. NodeJS or Java)
- Containers (with Docker image)

![image](https://user-images.githubusercontent.com/97020391/151333252-13f99c2b-bff9-49ce-8591-20245af6d92f.png)



### 3.1 Azure Service Fabric - Application Model
![image](https://user-images.githubusercontent.com/97020391/151333428-f3d0842b-2bee-407d-b85f-013e3c3723d0.png)

![image](https://user-images.githubusercontent.com/97020391/151333530-9475cd89-2e5d-4969-8cc1-0e49a5982d9b.png)


with Visual Studio 2022:

![image](https://user-images.githubusercontent.com/97020391/147924407-06b5a7e8-4f3d-496d-b8e9-517c8a3c9d4a.png)

![image](https://user-images.githubusercontent.com/97020391/147924413-d379d964-cc11-409f-92f6-915ebd3e913d.png)

![image](https://user-images.githubusercontent.com/97020391/147924424-1deed3bd-8c90-4152-89eb-a9d6600dd1f1.png)


### 3.2 Azure Service Fabric - several application version on one cluster
![image](https://user-images.githubusercontent.com/97020391/148247465-d8521023-6e74-447e-8469-74aa90923d2a.png)

### 3.3 Azure Service Fabric - monitoring and diagnostic
- Application Monitoring
- Cluster Monitoring
- Infrustructure Monitoring

## 4. Currency Manager project
### 4.1 Prerequisites
- Visual Studio 2022 Pro: https://visualstudio.microsoft.com/pl/vs/
- Azure Service Fabric SDK and Tools: https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started

See: https://github.com/skowragn/ASF_CurrencyBooker/blob/main/README.md#2-azure-service-fabric-version

### 4.2 Project Overview
The current project consists of:

- One Azure Service Fabric application - Currency Manager

It has the following elements:

- Stateful ASP.NET Core 5 Web API service - **CountriesService**
- Stateful ASP.NET Core 5 Web API service  - **CurrencyManagerService**
- Stateless ASP.NET Core 5 MVC service - **CurrencyManagerWeb**


![image](https://user-images.githubusercontent.com/97020391/151197774-310d2a52-02e9-42c2-be6e-154c2851ea2d.png)


![image](https://user-images.githubusercontent.com/97020391/147924520-5f7c8eff-4792-4948-af1a-25310d23812a.png)

![image](https://user-images.githubusercontent.com/97020391/147941627-68bece4c-9695-49ea-909f-e380b2303700.png)

![image](https://user-images.githubusercontent.com/97020391/147940439-d6c93259-6599-4464-b0bf-e0c990e89175.png)

![image](https://user-images.githubusercontent.com/97020391/147924511-d1b70def-0043-42f6-8c44-99410460cece.png)

### 4.1. Currency Manager – ApplicationManifest.xml
![image](https://user-images.githubusercontent.com/97020391/147942280-2e4607b2-bb2b-47f4-b04e-9662e430344b.png)

### 4.2. Currency Manager project - StartupServices.xml
![image](https://user-images.githubusercontent.com/97020391/147943399-f501e09e-4b61-4f5b-bb43-f82234626f99.png)

### 4.3. Currency Manager – LocalNode5.xml
![image](https://user-images.githubusercontent.com/97020391/147942487-c441b900-05e2-4148-a71f-98ada239c2d5.png)

![image](https://user-images.githubusercontent.com/97020391/151334871-931686ee-cb52-4e41-87d7-cf4976daae6b.png)

### 4.4. CurrencyManagerWeb - ASP.NET Core Stateless service
![image](https://user-images.githubusercontent.com/97020391/147942555-2f84c4ff-0a5f-4467-bcc2-a9855241d8c3.png)
![image](https://user-images.githubusercontent.com/97020391/147942570-a2ba5d61-cdf6-4c0c-bbd9-c87d42a8261e.png)
![image](https://user-images.githubusercontent.com/97020391/147942183-60d4820b-6bc2-4e59-91a7-cd6d6c5955d7.png)

### 4.5. CurrencyManagerService - ASP.NET Core Stateful service
![image](https://user-images.githubusercontent.com/97020391/147942628-0891782c-364d-4923-8db4-b4b49f3dcae7.png)
![image](https://user-images.githubusercontent.com/97020391/147942648-cd31596c-8738-4bbb-89ed-f9a300f44717.png)


### 4.6. Currency Manager - Sum up

#### 4.6.1. CountriesService - Stateful Service

|     Parameter        | Number|
| -------------------- | ----- |
| PartitionCount       |     1 |
| MinReplicaSetSize    |     1 |
| TargetReplicaSetSize |     1 |


#### 4.6.2. CurrencyManagerService - Stateful Service

|     Parameter        | Number|
| -------------------- | ----- |
| PartitionCount       |     3 |
| MinReplicaSetSize    |     1 |
| TargetReplicaSetSize |     3 |


#### 4.6.3. CurrencyManagerWeb - Stateless Service

|     Parameter        | Number|
| -------------------- | ----- |
| InstanceCount        |     1 |


#### 4.6.4. Azure Service Fabric Cluster

![image](https://user-images.githubusercontent.com/97020391/147940564-44f77200-a1fe-40ef-b441-a567f38ed6b6.png)

|     Parameter        | Number|
| -------------------- | ----- |
| Nodes                |     5 |
| Application          |     1 |
| Services             |     3 |
| Partitions           |     5 |
| Replicas             |    11 |


![image](https://user-images.githubusercontent.com/97020391/151201515-d169974e-6e88-4f8c-9352-942366e5e17b.png)



#### 4.6.5. CurrencyManager - current solution

![image](https://user-images.githubusercontent.com/97020391/148247267-b1ccea4a-40fc-485d-b51b-15fda68977d2.png)

![image](https://user-images.githubusercontent.com/97020391/148247310-3ad3ee20-3f2b-43cc-b390-3929e842f6b0.png)

![image](https://user-images.githubusercontent.com/97020391/148247324-d446b479-72b9-4dc3-a58c-6906f17fb559.png)


