using SoapParserApi.ProxyRepository;
using System;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace SoapParserApi.Models
{
    public sealed class WebServiceModel
    {
        private WebServiceModel()
        {
        }

        public string Name { get; private set; }

        public IReadOnlyList<WebServiceMethodModel> Methods => _methods;
        private readonly List<WebServiceMethodModel> _methods = new List<WebServiceMethodModel>();

        public static WebServiceModel SetService(ProxyService service)
        {
            var aggregate = new WebServiceModel
            {
                Name = service.ServiceName
            };

            foreach (var method in service.ServiceMethods)
            {
                var methodModel = WebServiceMethodModel.Create(
                    method.Key,
                    method.Value);

                aggregate._methods.Add(methodModel);
            }

            return aggregate;
        }
    }

    public sealed class WebServiceMethodModel
    {
        private WebServiceMethodModel()
        {
        }

        public string Name { get; private set; }

        public IReadOnlyList<WebServiceParameterModel> Parameters => _parameters;
        private readonly List<WebServiceParameterModel> _parameters = new List<WebServiceParameterModel>();

        public IReadOnlyList<WebServiceParameterModel> Results => _results;
        private readonly List<WebServiceParameterModel> _results = new List<WebServiceParameterModel>();

        internal static WebServiceMethodModel Create(
            string methodName,
            ProxyMethod method)
        {
            var model = new WebServiceMethodModel
            {
                Name = methodName
            };

            foreach (var detail in method.ServiceParameters)
            {
                model._parameters.Add(
                    WebServiceParameterModel.Create(
                        detail.Name,
                        detail.Type));
            }

            foreach (var detail in method.ServiceResults)
            {
                model._results.Add(
                    WebServiceParameterModel.Create(
                        detail.Name,
                        detail.Type));
            }

            return model;
        }
    }

    public sealed class WebServiceParameterModel
    {
        private WebServiceParameterModel()
        {
        }

        public string Name { get; private set; }

        public string Type { get; private set; }

        public bool IsSystemProperty =>
            Name.EndsWith("Specified", StringComparison.OrdinalIgnoreCase);

        internal static WebServiceParameterModel Create(
            string name,
            Type type)
        {
            return new WebServiceParameterModel
            {
                Name = name,
                Type = type.ToString()
            };
        }
    }


}