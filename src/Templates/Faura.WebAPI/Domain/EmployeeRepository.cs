using Faura.Infrastructure.UnitOfWork.Generated;
using Faura.WebAPI.Domain.Entities;
using YourNamespace.Data;

namespace Faura.WebAPI.Domain;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(EmployeeDbContext dbContext, ILogger<Repository<Employee>> logger, bool enableTracking = false) : base(dbContext, logger, enableTracking)
    {
    }
}
