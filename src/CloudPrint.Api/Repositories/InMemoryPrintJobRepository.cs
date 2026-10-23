using System.Collections.Concurrent;
using CloudPrint.Api.Domain;
namespace CloudPrint.Api.Repositories;
public sealed class InMemoryPrintJobRepository : IPrintJobRepository
{
    private readonly ConcurrentDictionary<Guid, PrintJob> _jobs = new();
    public PrintJob Add(PrintJob job) { _jobs.TryAdd(job.Id, job); return job; }
    public IReadOnlyCollection<PrintJob> GetAll() => _jobs.Values.OrderByDescending(x => x.SubmittedAt).ToArray();
    public PrintJob? GetById(Guid id) => _jobs.GetValueOrDefault(id);
    public void Update(PrintJob job) => _jobs[job.Id] = job;
}
