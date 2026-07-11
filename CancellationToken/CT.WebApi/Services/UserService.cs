using CT.WebApi.Handlers.CreateUser;
using CT.WebApi.Repositories;

namespace CT.WebApi.Services;

public class UserService : IUserRepository
{
    public async Task<Guid> AddRoleAsync(UserCreateCommand command, CancellationToken cancellation)
    {
        await Task.Delay(3000, cancellation);
        return Guid.NewGuid();
    }

    public async Task<Guid> CreateAsync(UserCreateCommand command, CancellationToken cancellation)
    {
        await Task.Delay(3000, cancellation);
        return Guid.NewGuid();
    }
}
