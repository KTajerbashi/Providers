using CT.WebApi.Repositories;
using MediatR;

namespace CT.WebApi.Handlers.CreateUser;

public record UserCreateCommand(string Username, string Password) : IRequest<bool>;
public class UserCreateHandler : IRequestHandler<UserCreateCommand, bool>
{
    private readonly IUserRepository _userRepository;
    public UserCreateHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<bool> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine("CreateUser Start");
            await _userRepository.CreateAsync(request, cancellationToken);
            Console.WriteLine("CreateUser Ended");

            await Task.Delay(2000);

            Console.WriteLine("AddRoleToUser Start");
            await _userRepository.AddRoleAsync(request, cancellationToken);
            Console.WriteLine("AddRoleToUser Ended");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("IsCancellationRequested : {0}", cancellationToken.IsCancellationRequested);
            Console.WriteLine("CanBeCanceled : {0}",cancellationToken.CanBeCanceled);
            Console.WriteLine("Ex Message : {0}", ex.Message);
            //throw ex;
            return false;
        }
    }
}