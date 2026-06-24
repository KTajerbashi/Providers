using UnitOfWork.WebApp.Application.Repositories;
using UnitOfWork.WebApp.Common;
using UnitOfWork.WebApp.Domain;
using UnitOfWork.WebApp.Infrastructure;

namespace UnitOfWork.WebApp.Application.Services;

public class GroupRepository : AggregateRepository<GroupEntity>, IGroupRepository
{
    public GroupRepository(DataContext context) : base(context)
    {
    }
}

