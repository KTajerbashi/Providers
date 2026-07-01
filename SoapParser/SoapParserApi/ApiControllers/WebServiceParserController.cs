using SoapParserApi.Models;
using SoapParserApi.ProxyRepository;
using SoapParserApi.ProxyServices;
using System.Web.Http;

namespace SoapParserApi.ApiControllers
{
    public class WebServiceParserController : ApiController
    {
        [HttpGet]
        public IHttpActionResult ImportService()
        {
            ProxyFactory proxyFactory = new ProxyFactory();
            string endpointUrl = "http://localhost:2711/SoapParser.svc?wsdl";
            string username = "";
            string password = "";
            bool isSSLIgnore = true;
            ProxyService result = proxyFactory.ImportService(endpointUrl, username, password, isSSLIgnore);
            var ss = result.ServiceMethods.Values;

            WebServiceModel aggregate = new WebServiceModel();

            aggregate.Name = result.ServiceName;
            foreach (var method in result.ServiceMethods)
            {
                var record = new WebServiceMethodModel();
                record.Name = method.Key;
                foreach (var detail in method.Value.ServiceParameters)
                {
                    record.Parameters.Add(new WebServiceParameterModel()
                    {
                        Name = detail.Name,
                        Type = detail.Type.ToString()
                    });
                }
                foreach (var detail in method.Value.ServiceResults)
                {
                    record.Results.Add(new WebServiceParameterModel()
                    {
                        Name = detail.Name,
                        Type = detail.Type.ToString()
                    });
                }
                aggregate.Methods.Add(record);
            }
            return Ok(aggregate);

        }
    }
}