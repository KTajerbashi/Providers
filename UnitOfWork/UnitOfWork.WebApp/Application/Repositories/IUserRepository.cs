using UnitOfWork.WebApp.Common;
using UnitOfWork.WebApp.Domain;

namespace UnitOfWork.WebApp.Application.Repositories;

public interface IUserRepository : IAggregateRepository<UserEntity>
{
}
