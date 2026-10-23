using System.ComponentModel.DataAnnotations;
namespace CloudPrint.Api.Contracts;
public sealed class CreatePrintJobRequest
{
    [Required, StringLength(120, MinimumLength = 1)] public string DocumentName { get; init; } = string.Empty;
    [Range(1, 500)] public int PageCount { get; init; }
    public bool Color { get; init; }
    [Required, StringLength(80, MinimumLength = 1)] public string PrinterName { get; init; } = string.Empty;
}
