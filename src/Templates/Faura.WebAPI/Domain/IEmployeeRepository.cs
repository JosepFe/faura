using Faura.Infrastructure.UnitOfWork.Repositories;
using Faura.WebAPI.Domain.Entities;
namespace Faura.WebAPI.Domain;

public interface IEmployeeRepository : IEntityRepository<Employee>
{
}
