using CloudPrint.Api.Contracts;
using CloudPrint.Api.Domain;
using CloudPrint.Api.Repositories;
using CloudPrint.Api.Services;

namespace CloudPrint.Api.Tests.Services;

public sealed class PrintJobServiceTests
{
    private static PrintJobService CreateService() => new(new InMemoryPrintJobRepository());
    private static CreatePrintJobRequest ValidRequest(string name = "report.pdf") => new() { DocumentName = name, PageCount = 3, Color = true, PrinterName = "HQ LaserJet" };

    [Fact] public void Create_assigns_an_identifier() => Assert.NotEqual(Guid.Empty, CreateService().Create(ValidRequest()).Id);
    [Fact] public void Create_sets_queued_status() => Assert.Equal(PrintJobStatus.Queued, CreateService().Create(ValidRequest()).Status);
    [Fact] public void Create_preserves_page_count() => Assert.Equal(3, CreateService().Create(ValidRequest()).PageCount);
    [Fact] public void Create_preserves_color_option() => Assert.True(CreateService().Create(ValidRequest()).Color);
    [Fact] public void Create_trims_document_name() => Assert.Equal("report.pdf", CreateService().Create(ValidRequest(" report.pdf ")).DocumentName);
    [Fact] public void Create_trims_printer_name() => Assert.Equal("HQ LaserJet", CreateService().Create(new() { DocumentName = "a.pdf", PageCount = 1, PrinterName = " HQ LaserJet " }).PrinterName);
    [Fact] public void Create_sets_submission_time() => Assert.True(CreateService().Create(ValidRequest()).SubmittedAt <= DateTimeOffset.UtcNow);
    [Fact] public void GetById_returns_created_job() { var service = CreateService(); var job = service.Create(ValidRequest()); Assert.Same(job, service.GetById(job.Id)); }
    [Fact] public void GetById_returns_null_for_unknown_job() => Assert.Null(CreateService().GetById(Guid.NewGuid()));
    [Fact] public void GetAll_is_empty_initially() => Assert.Empty(CreateService().GetAll());
    [Fact] public void GetAll_returns_created_job() { var service = CreateService(); service.Create(ValidRequest()); Assert.Single(service.GetAll()); }
    [Fact] public void GetAll_returns_all_created_jobs() { var service = CreateService(); service.Create(ValidRequest()); service.Create(ValidRequest("second.pdf")); Assert.Equal(2, service.GetAll().Count); }
    [Fact] public void Cancel_marks_queued_job_cancelled() { var service = CreateService(); var job = service.Create(ValidRequest()); Assert.Equal(PrintJobStatus.Cancelled, service.Cancel(job.Id)!.Status); }
    [Fact] public void Cancel_returns_null_for_unknown_job() => Assert.Null(CreateService().Cancel(Guid.NewGuid()));
    [Fact] public void Cancel_returns_null_for_already_cancelled_job() { var service = CreateService(); var job = service.Create(ValidRequest()); service.Cancel(job.Id); Assert.Null(service.Cancel(job.Id)); }
    [Fact] public void Cancel_keeps_job_available() { var service = CreateService(); var job = service.Create(ValidRequest()); service.Cancel(job.Id); Assert.NotNull(service.GetById(job.Id)); }
    [Fact] public void Cancel_updates_persisted_status() { var service = CreateService(); var job = service.Create(ValidRequest()); service.Cancel(job.Id); Assert.Equal(PrintJobStatus.Cancelled, service.GetById(job.Id)!.Status); }
    [Fact] public void Repository_add_returns_job() { var repository = new InMemoryPrintJobRepository(); var job = new PrintJob(); Assert.Same(job, repository.Add(job)); }
    [Fact] public void Repository_returns_null_for_unknown_id() => Assert.Null(new InMemoryPrintJobRepository().GetById(Guid.NewGuid()));
    [Fact] public void Repository_update_replaces_job() { var repository = new InMemoryPrintJobRepository(); var job = repository.Add(new PrintJob()); job.Status = PrintJobStatus.Processing; repository.Update(job); Assert.Equal(PrintJobStatus.Processing, repository.GetById(job.Id)!.Status); }
    [Fact] public void Printer_service_returns_seeded_printers() => Assert.NotEmpty(new PrinterService().GetAvailablePrinters());
    [Fact] public void Printer_service_includes_online_printer() => Assert.Contains(new PrinterService().GetAvailablePrinters(), p => p.IsOnline);
    [Fact] public void Printer_service_includes_color_printer() => Assert.Contains(new PrinterService().GetAvailablePrinters(), p => p.SupportsColor);
    [Fact] public void Printer_ids_are_unique() { var printers = new PrinterService().GetAvailablePrinters(); Assert.Equal(printers.Count, printers.Select(x => x.Id).Distinct().Count()); }
    [Fact] public void Distinct_jobs_have_distinct_ids() { var service = CreateService(); Assert.NotEqual(service.Create(ValidRequest()).Id, service.Create(ValidRequest()).Id); }
    [Fact] public void New_job_defaults_to_queued() => Assert.Equal(PrintJobStatus.Queued, new PrintJob().Status);
    [Fact] public void New_job_has_non_empty_id() => Assert.NotEqual(Guid.Empty, new PrintJob().Id);
}
