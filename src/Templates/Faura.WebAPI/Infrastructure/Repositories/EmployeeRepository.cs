namespace Faura.WebAPI.Infrastructure.Repositories;

using System.Threading.Tasks;
using Faura.Infrastructure.UnitOfWork.Repositories;
using Faura.WebAPI.Domain.Entities;
using Faura.WebAPI.Domain.Repositories;
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

    public Task<Employee?> GetByIdAsync(long id)
        => GetFirstOrDefaultAsync(e => e.Id == id);
}
