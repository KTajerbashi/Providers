using UnitOfWork.WebApp.Application.Repositories;
using UnitOfWork.WebApp.Common;
using UnitOfWork.WebApp.Domain;
using UnitOfWork.WebApp.Infrastructure;

namespace UnitOfWork.WebApp.Application.Services;

public class UserRepository : AggregateRepository<UserEntity>, IUserRepository
{
    public UserRepository(DataContext context) : base(context)
    {
    }
}

