# Azure Service Fabric - Currency Manager
## Overview
Azure Service Fabric is a distributed systems platform used to build hyper-scalable, reliable and easily managed applications for the cloud.

Nowadays it powers many Azure Cloud and Microsoft services.

![image](https://user-images.githubusercontent.com/97020391/147913169-7a4e4af3-ac80-41b2-a000-66d669b7566b.png)

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

# Azure Service Fabric Version
https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started
![image](https://user-images.githubusercontent.com/97020391/147914723-1b876cb0-69b6-43e8-8e64-5fa1bd7af3ea.png)
![image](https://user-images.githubusercontent.com/97020391/147914744-e0bc9a27-fc21-42ad-85fa-dca359a0afc4.png)
![image](https://user-images.githubusercontent.com/97020391/147914751-bbf35faa-3563-4ee2-8b3b-e8a298d063eb.png)

# Azure Service Fabric - programming model
Azure Service Fabric cluster is on .NET Framework 4.8. 
It can run the folowing services/processes:
- Reliable service (stateful and stateless) with .NET 6.0
- ASP.NET Core services (stateful and stateless) with  ASP.NET Core 5/.NET 5 (Web API, MVC, with Angular/React etc.)
- Reliable Actors (reliable stateful services) with .NET Framework 4.8
- Guest executable - self-contained applications (e.g. NodeJS or Java)
- Containers (with Docker image)

with Visual Studio 2022:

![image](https://user-images.githubusercontent.com/97020391/147924407-06b5a7e8-4f3d-496d-b8e9-517c8a3c9d4a.png)

![image](https://user-images.githubusercontent.com/97020391/147924413-d379d964-cc11-409f-92f6-915ebd3e913d.png)

![image](https://user-images.githubusercontent.com/97020391/147924424-1deed3bd-8c90-4152-89eb-a9d6600dd1f1.png)


## Currency Manager project
The current project consists of:

- One Azure Service Fabric application - Currency Manager
- 
It has the following elements:

![image](https://user-images.githubusercontent.com/97020391/147924520-5f7c8eff-4792-4948-af1a-25310d23812a.png)

- Stateful ASP.NET Core 5 Web API service - **CountriesService**
- Stateful ASP.NET Core 5 Web API service  - **CurrencyManagerService**
- Stateless ASP.NET Core 5 MVC service - **CurrencyManagerWeb**

![image](https://user-images.githubusercontent.com/97020391/147924538-35e49441-3ea0-499e-9d48-304949caf0ac.png)


![image](https://user-images.githubusercontent.com/97020391/147924511-d1b70def-0043-42f6-8c44-99410460cece.png)


