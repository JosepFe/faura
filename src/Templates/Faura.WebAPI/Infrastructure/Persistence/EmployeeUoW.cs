using Faura.Infrastructure.UnitOfWork.UnitOfWork;
using YourNamespace.Data;

namespace Faura.WebAPI.Infrastructure.Persistence;

public class EmployeeUoW : UnitOfWork<EmployeeDbContext>, IEmployeeUoW
{
    public EmployeeUoW(EmployeeDbContext context) : base(context)
    {
    }
}
