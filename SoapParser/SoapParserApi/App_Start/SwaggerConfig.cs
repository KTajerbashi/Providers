using Swashbuckle.Application;
using System.Web;
using System.Web.Http;

[assembly: PreApplicationStartMethod(typeof(SoapParserApi.App_Start.SwaggerConfig), "Register")]

namespace SoapParserApi.App_Start
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "My WebApi");
                    c.PrettyPrint();
                })
                .EnableSwaggerUi();
        }
    }
}