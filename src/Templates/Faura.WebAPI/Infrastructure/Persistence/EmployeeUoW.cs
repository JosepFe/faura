namespace Faura.WebAPI.Infrastructure.Persistence;

using Faura.Infrastructure.UnitOfWork.UnitOfWork;

public class EmployeeUoW : UnitOfWork<EmployeeDbContext>, IEmployeeUoW
{
    public EmployeeUoW(EmployeeDbContext context)
        : base(context)
    {
    }
}
