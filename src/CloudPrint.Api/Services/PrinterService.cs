using CloudPrint.Api.Domain;
namespace CloudPrint.Api.Services;
public sealed class PrinterService : IPrinterService
{
    private static readonly Printer[] Printers = [new("printer-hq-01", "HQ LaserJet", "Head Office / Floor 2", true, true), new("printer-ops-01", "Operations Mono", "Operations / Floor 1", true, false), new("printer-remote-01", "Remote Color", "Remote Office", false, true)];
    public IReadOnlyCollection<Printer> GetAvailablePrinters() => Printers;
}
