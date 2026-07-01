using System.Collections.Generic;

namespace SoapParserApi.Models
{
    public class WebServiceModel
    {
        public WebServiceModel()
        {
            Methods = new List<WebServiceMethodModel>();
        }
        public string Name { get; set; }
        public List<WebServiceMethodModel> Methods { get; set; }
    }
    public class WebServiceMethodModel
    {
        public WebServiceMethodModel()
        {
            Parameters = new List<WebServiceParameterModel>();
            Results = new List<WebServiceParameterModel>();
        }
        public string Name { get; set; }
        public List<WebServiceParameterModel> Parameters { get; set; }
        public List<WebServiceParameterModel> Results { get; set; }
    }
    public class WebServiceParameterModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}