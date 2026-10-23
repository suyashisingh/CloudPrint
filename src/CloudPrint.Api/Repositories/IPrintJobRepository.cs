using CloudPrint.Api.Domain;
namespace CloudPrint.Api.Repositories;
public interface IPrintJobRepository { PrintJob Add(PrintJob job); IReadOnlyCollection<PrintJob> GetAll(); PrintJob? GetById(Guid id); void Update(PrintJob job); }
