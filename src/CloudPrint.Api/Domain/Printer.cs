namespace CloudPrint.Api.Domain;
public sealed record Printer(string Id, string Name, string Location, bool IsOnline, bool SupportsColor);
