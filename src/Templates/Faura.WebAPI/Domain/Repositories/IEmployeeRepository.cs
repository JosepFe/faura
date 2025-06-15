namespace Faura.WebAPI.Domain.Repositories;

using Faura.Infrastructure.UnitOfWork.Repositories;
using Faura.WebAPI.Domain.Entities;

public interface IEmployeeRepository : IEntityRepository<Employee>
{
    Task<Employee?> GetByIdAsync(long id);
}
