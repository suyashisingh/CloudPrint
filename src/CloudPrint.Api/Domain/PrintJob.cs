namespace CloudPrint.Api.Domain;
public sealed class PrintJob
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string DocumentName { get; init; } = string.Empty;
    public int PageCount { get; init; }
    public bool Color { get; init; }
    public string PrinterName { get; init; } = string.Empty;
    public PrintJobStatus Status { get; set; } = PrintJobStatus.Queued;
    public DateTimeOffset SubmittedAt { get; init; } = DateTimeOffset.UtcNow;
}
