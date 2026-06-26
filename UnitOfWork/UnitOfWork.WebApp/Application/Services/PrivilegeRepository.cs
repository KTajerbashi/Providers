using UnitOfWork.WebApp.Application.Repositories;
using UnitOfWork.WebApp.Common;
using UnitOfWork.WebApp.Domain;
using UnitOfWork.WebApp.Infrastructure;

namespace UnitOfWork.WebApp.Application.Services;

public class PrivilegeRepository : AggregateRepository<PrivilegeEntity>, IPrivilegeRepository
{
    public PrivilegeRepository(DataContext context) : base(context)
    {
        Console.WriteLine($"PrivilegeRepository : {context.ContextId.InstanceId.ToString()}");
    }
}

