using CloudPrint.Api.Contracts;
using CloudPrint.Api.Domain;
using CloudPrint.Api.Services;
using Microsoft.AspNetCore.Mvc;
namespace CloudPrint.Api.Controllers;
[ApiController, Route("api/print-jobs")]
public sealed class PrintJobsController(IPrintJobService service) : ControllerBase
{
    [HttpPost, ProducesResponseType(typeof(PrintJob), StatusCodes.Status201Created)] public ActionResult<PrintJob> Create(CreatePrintJobRequest request) { var job = service.Create(request); return CreatedAtAction(nameof(GetById), new { id = job.Id }, job); }
    [HttpGet] public ActionResult<IReadOnlyCollection<PrintJob>> GetAll() => Ok(service.GetAll());
    [HttpGet("{id:guid}")] public ActionResult<PrintJob> GetById(Guid id) => service.GetById(id) is { } job ? Ok(job) : NotFound();
    [HttpPost("{id:guid}/cancel")] public ActionResult<PrintJob> Cancel(Guid id) => service.Cancel(id) is { } job ? Ok(job) : Conflict(new { message = "The job does not exist or cannot be cancelled." });
}
