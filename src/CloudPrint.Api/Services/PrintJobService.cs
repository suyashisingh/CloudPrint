using CloudPrint.Api.Contracts;
using CloudPrint.Api.Domain;
using CloudPrint.Api.Repositories;
namespace CloudPrint.Api.Services;
public sealed class PrintJobService(IPrintJobRepository repository) : IPrintJobService
{
    public PrintJob Create(CreatePrintJobRequest request) => repository.Add(new PrintJob { DocumentName = request.DocumentName.Trim(), PageCount = request.PageCount, Color = request.Color, PrinterName = request.PrinterName.Trim() });
    public IReadOnlyCollection<PrintJob> GetAll() => repository.GetAll();
    public PrintJob? GetById(Guid id) => repository.GetById(id);
    public PrintJob? Cancel(Guid id) { var job = repository.GetById(id); if (job is null || job.Status is PrintJobStatus.Completed or PrintJobStatus.Cancelled) return null; job.Status = PrintJobStatus.Cancelled; repository.Update(job); return job; }
}
