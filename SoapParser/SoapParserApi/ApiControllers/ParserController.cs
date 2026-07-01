using System.Web.Http;

namespace SoapParserApi.ApiControllers
{
    public class ParserController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(new[]
            {
                "Ali",
                "Reza"
            });
        }
    }
}