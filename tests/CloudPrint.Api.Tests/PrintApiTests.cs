using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CloudPrint.Api.Tests;

public sealed class PrintApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    [Fact]
    public async Task Health_returns_200()
        => Assert.Equal(HttpStatusCode.OK, (await _client.GetAsync("/health")).StatusCode);

    [Fact]
    public async Task Create_job_returns_201()
        => Assert.Equal(HttpStatusCode.Created, (await _client.PostAsJsonAsync("/api/print-jobs", ValidRequest())).StatusCode);

    [Fact]
    public async Task Create_job_returns_location_header()
    {
        var response = await _client.PostAsJsonAsync("/api/print-jobs", ValidRequest());
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task Create_job_rejects_missing_document_name()
    {
        var response = await _client.PostAsJsonAsync("/api/print-jobs", new { pageCount = 1, printerName = "HQ LaserJet" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_job_rejects_zero_pages()
    {
        var response = await _client.PostAsJsonAsync("/api/print-jobs", new { documentName = "invalid.pdf", pageCount = 0, printerName = "HQ LaserJet" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task List_jobs_returns_200()
        => Assert.Equal(HttpStatusCode.OK, (await _client.GetAsync("/api/print-jobs")).StatusCode);

    [Fact]
    public async Task Get_job_by_id_returns_created_job()
    {
        var created = await _client.PostAsJsonAsync("/api/print-jobs", ValidRequest());
        var job = await created.Content.ReadFromJsonAsync<JobDto>(JsonOptions);
        var response = await _client.GetAsync($"/api/print-jobs/{job!.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_job_by_unknown_id_returns_404()
        => Assert.Equal(HttpStatusCode.NotFound, (await _client.GetAsync($"/api/print-jobs/{Guid.NewGuid()}")).StatusCode);

    [Fact]
    public async Task Cancel_job_changes_status()
    {
        var created = await _client.PostAsJsonAsync("/api/print-jobs", ValidRequest());
        var job = await created.Content.ReadFromJsonAsync<JobDto>(JsonOptions);
        var response = await _client.PostAsync($"/api/print-jobs/{job!.Id}/cancel", null);
        var cancelled = await response.Content.ReadFromJsonAsync<JobDto>(JsonOptions);
        Assert.Equal("Cancelled", cancelled!.Status);
    }

    [Fact]
    public async Task Cancel_unknown_job_returns_conflict()
        => Assert.Equal(HttpStatusCode.Conflict, (await _client.PostAsync($"/api/print-jobs/{Guid.NewGuid()}/cancel", null)).StatusCode);

    [Fact]
    public async Task List_printers_returns_printers()
    {
        var printers = await _client.GetFromJsonAsync<List<PrinterDto>>("/api/printers", JsonOptions);
        Assert.NotEmpty(printers!);
    }

    private static object ValidRequest() => new { documentName = "api-test.pdf", pageCount = 2, color = true, printerName = "HQ LaserJet" };
    private sealed record JobDto(Guid Id, string Status);
    private sealed record PrinterDto(string Id, string Name);
}
