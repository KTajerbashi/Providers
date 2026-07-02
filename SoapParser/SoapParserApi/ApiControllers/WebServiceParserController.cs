using SoapParserApi.Models;
using SoapParserApi.ProxyRepository;
using SoapParserApi.ProxyServices;
using System.Web.Http;

namespace SoapParserApi.ApiControllers
{
    public class WebServiceParserController : ApiController
    {
        [HttpGet]
        [Route("ParseService")]
        public IHttpActionResult ImportService()
        {
            ProxyFactory proxyFactory = new ProxyFactory();
            string endpointUrl = "http://localhost:2711/SoapParser.svc?wsdl";
            string username = "";
            string password = "";
            bool isSSLIgnore = true;
            ProxyService result = proxyFactory.ImportService(endpointUrl, username, password, isSSLIgnore);


            return Ok(WebServiceModel.SetService(result));

        }
    }
}