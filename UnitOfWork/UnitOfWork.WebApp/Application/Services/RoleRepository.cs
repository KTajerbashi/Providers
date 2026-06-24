using UnitOfWork.WebApp.Application.Repositories;
using UnitOfWork.WebApp.Common;
using UnitOfWork.WebApp.Domain;
using UnitOfWork.WebApp.Infrastructure;

namespace UnitOfWork.WebApp.Application.Services;

public class RoleRepository : AggregateRepository<RoleEntity>, IRoleRepository
{
    public RoleRepository(DataContext context) : base(context)
    {
    }
}

