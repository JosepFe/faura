using Faura.Infrastructure.UnitOfWork.UnitOfWork;

namespace Faura.WebAPI.Infrastructure.Persistence;

/// <summary>
/// Unit of Work implementation for Sample context.
/// Manages transactions and coordinates work across repositories.
/// </summary>
public class SampleUoW : UnitOfWork<SampleDbContext>, ISampleUoW
{
    public SampleUoW(SampleDbContext context)
        : base(context) { }
}
