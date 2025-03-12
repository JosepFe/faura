using Faura.Infrastructure.UnitOfWork.Repositories;
using Faura.WebAPI.Domain.Entities;
using YourNamespace.Data;

namespace Faura.WebAPI.Domain;

public class EmployeeRepository : EntityRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(EmployeeDbContext dbContext, ILogger<EntityRepository<Employee>> logger, bool enableTracking = false) : base(dbContext, logger, enableTracking)
    {
    }
}
