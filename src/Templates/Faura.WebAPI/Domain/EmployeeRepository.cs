namespace Faura.WebAPI.Domain;

using Faura.Infrastructure.UnitOfWork.Repositories;
using Faura.WebAPI.Domain.Entities;
using Faura.WebAPI.Infrastructure.Persistence;

public class EmployeeRepository : EntityRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(
        EmployeeDbContext dbContext,
        ILogger<EmployeeRepository> logger,
        bool enableTracking = false)
        : base(dbContext, logger, enableTracking)
    {
    }
}
