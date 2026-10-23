using CloudPrint.Api.Repositories;
using CloudPrint.Api.Services;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSingleton<IPrintJobRepository, InMemoryPrintJobRepository>();
builder.Services.AddSingleton<IPrintJobService, PrintJobService>();
builder.Services.AddSingleton<IPrinterService, PrinterService>();
var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.Run();
public partial class Program { }
