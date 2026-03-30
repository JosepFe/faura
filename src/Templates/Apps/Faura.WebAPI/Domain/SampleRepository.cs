using Faura.Infrastructure.UnitOfWork.Repositories;
using Faura.WebAPI.Domain.Entities;
using Faura.WebAPI.Infrastructure.Persistence;

namespace Faura.WebAPI.Domain;

/// <summary>
/// Repository implementation for Sample entity.
/// Uses EntityRepository base class from Faura.Infrastructure.UnitOfWork.
/// </summary>
public class SampleRepository : EntityRepository<Sample>, ISampleRepository
{
    public SampleRepository(
        SampleDbContext dbContext,
        ILogger<EntityRepository<Sample>> logger,
        bool enableTracking = false
    )
        : base(dbContext, logger, enableTracking) { }
}
