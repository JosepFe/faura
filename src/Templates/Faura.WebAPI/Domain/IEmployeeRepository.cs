namespace Faura.WebAPI.Domain;

using Faura.Infrastructure.UnitOfWork.Repositories;
using Faura.WebAPI.Domain.Entities;

public interface IEmployeeRepository : IEntityRepository<Employee>
{
}
