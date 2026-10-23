using CloudPrint.Api.Contracts;
using CloudPrint.Api.Domain;
namespace CloudPrint.Api.Services;
public interface IPrintJobService { PrintJob Create(CreatePrintJobRequest request); IReadOnlyCollection<PrintJob> GetAll(); PrintJob? GetById(Guid id); PrintJob? Cancel(Guid id); }
