using Microsoft.AspNetCore.Mvc;

namespace SoapOnWebApi.WebApi.Controllers;

public class CalculatorController : BaseController
{
    [HttpGet("Call")]
    public async Task<IActionResult> Call()
    {
        var response = await Task.Run(() =>
        {
            return new
            {
                Message = "Hello from CalculatorController.Call()"
            };
        });
        return Ok(response);
    }

}