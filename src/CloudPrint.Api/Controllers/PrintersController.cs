using CloudPrint.Api.Domain;
using CloudPrint.Api.Services;
using Microsoft.AspNetCore.Mvc;
namespace CloudPrint.Api.Controllers;
[ApiController, Route("api/printers")]
public sealed class PrintersController(IPrinterService service) : ControllerBase
{ [HttpGet] public ActionResult<IReadOnlyCollection<Printer>> GetAll() => Ok(service.GetAvailablePrinters()); }
