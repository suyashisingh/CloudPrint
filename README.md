# CloudPrint API

CloudPrint is a C# / ASP.NET Core 8 cloud print-service backend. It uses a layered, object-oriented design: controllers handle HTTP, services own business rules, and a repository stores jobs. Interfaces and constructor injection make every layer independently testable.

## REST API

| # | Method | Route | Purpose |
|---|---|---|---|
| 1 | POST | `/api/print-jobs` | Submit a print job (201 Created) |
| 2 | GET | `/api/print-jobs` | List submitted jobs |
| 3 | GET | `/api/print-jobs/{id}` | Get a job by ID |
| 4 | POST | `/api/print-jobs/{id}/cancel` | Cancel a queued/processing job |
| 5 | GET | `/api/printers` | List available cloud printers |

`GET /health` provides a deployment health probe.

### Submit a job

```json
POST /api/print-jobs
{
  "documentName": "quarterly-report.pdf",
  "pageCount": 12,
  "color": true,
  "printerName": "HQ LaserJet"
}
```

The API validates required fields, document/printer name lengths, and page count (1–500). Jobs are stored in-memory for this sample; replace `InMemoryPrintJobRepository` with an Azure-backed implementation (for example Azure Cosmos DB or Table Storage) without changing controllers or services.

## Run locally

```powershell
dotnet restore CloudPrint.sln
dotnet run --project src/CloudPrint.Api
dotnet test CloudPrint.sln
```

## Azure deployment

The included [GitHub Actions workflow](.github/workflows/azure-webapp.yml) builds, runs the xUnit test suite, publishes, and deploys to Azure App Service on pushes to `main`. Create an Azure Web App, set `AZURE_WEBAPP_NAME` in the workflow, and add its downloaded publish profile as the `AZURE_WEBAPP_PUBLISH_PROFILE` GitHub secret. A production-ready Dockerfile is also included for Azure Container Apps or App Service containers.

## Testing

The xUnit suite has 38 tests: focused unit tests for service/repository rules plus integration tests that exercise every REST endpoint and validation errors.
