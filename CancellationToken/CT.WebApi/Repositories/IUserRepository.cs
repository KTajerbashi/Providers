using CT.WebApi.Handlers.CreateUser;

namespace CT.WebApi.Repositories;

public interface IUserRepository
{
    Task<Guid> CreateAsync(UserCreateCommand command, CancellationToken cancellation);
    Task<Guid> AddRoleAsync(UserCreateCommand command, CancellationToken cancellation);
}
