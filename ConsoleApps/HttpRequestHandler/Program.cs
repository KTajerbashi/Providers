
using HttpRequestHandler.SoapParser;
using System.Net.Http.Headers;

class Program
{
    static async Task Main()
    {
        AuthCheckService service = new AuthCheckService();
        await service.CallSecureApi();
    }
}