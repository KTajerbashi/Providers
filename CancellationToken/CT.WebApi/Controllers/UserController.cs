using CT.WebApi.Handlers.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CT.WebApi.Controllers;

public class UserController : BaseController
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser(UserCreateCommand command, CancellationToken cancellation)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        var result = await Mediator.Send(command);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Task Completed : {0}", result);
        return Ok(new
        {
            result,
            cancellation.IsCancellationRequested,
            cancellation.CanBeCanceled,
        });
    }
}
