using Faura.Infrastructure.UnitOfWork.UnitOfWork;

namespace Faura.WebAPI.Infrastructure.Persistence;

public class EmployeeUoW : UnitOfWork<EmployeeDbContext>, IEmployeeUoW
{
    public EmployeeUoW(EmployeeDbContext context)
        : base(context) { }
}
