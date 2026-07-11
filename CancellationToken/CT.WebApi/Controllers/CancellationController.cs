using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CT.WebApi.Controllers;

public class CancellationController : BaseController
{
    public CancellationController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("WithCancellation")]
    public async Task<IActionResult> WithCancellation(CancellationToken cancellation)
    {
        try
        {
            Console.WriteLine("WithCancellation Start");
            await Task.Delay(3000, cancellation);
            Console.WriteLine("WithCancellation Ended");
            return Ok(new
            {
                cancellation.IsCancellationRequested,
                cancellation.CanBeCanceled,
            });
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("WithCancellation Ex : {0}", ex.Message);
            return NotFound(ex.Message);
        }
    }


    [HttpGet("WithoutCancellation")]
    public async Task<IActionResult> WithoutCancellation()
    {

        Console.WriteLine("WithoutCancellation Start");
        await Task.Delay(5000);
        Console.WriteLine("WithoutCancellation Ended");
        return Ok();
    }

}
