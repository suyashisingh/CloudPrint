using CloudPrint.Api.Domain;
namespace CloudPrint.Api.Services;
public interface IPrinterService { IReadOnlyCollection<Printer> GetAvailablePrinters(); }
