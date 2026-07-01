using Newtonsoft.Json;
using SoapParserWCF.Models;
using SoapParserWCF.ProxyFactory;
using SoapParserWCF.ProxyRepository;
using System;
using System.Collections.Generic;

namespace SoapParserWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class SoapParser : ISoapParser
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public UserModel GetUserInfo(string nationalCode)
        {
            return new UserModel()
            {
                Username = $"{nationalCode}@mail.com",
                Age = 20,
                IsActive = true,
                Mobile = "09011001000",
                Password = "@User123",
                Addresses = new List<Address>()
                {
                    new Address(){ HouseNumber = "House 1",Name = "School - 1",Pelak="P-12-1"},
                    new Address(){ HouseNumber = "House 2",Name = "School - 2",Pelak="P-12-2"},
                    new Address(){ HouseNumber = "House 3",Name = "School - 3",Pelak="P-12-3"},
                    new Address(){ HouseNumber = "House 4",Name = "School - 4",Pelak="P-12-4"},
                    new Address(){ HouseNumber = "House 5",Name = "School - 5",Pelak="P-12-5"},
                }
            };
        }

        public string ReadSoapStructure(string endpointUrl, string username, string password, bool isSSLIgnore)
        {
            ProxyService result = new ProxyFactory.ProxyFactory().ImportService(endpointUrl, username, password, isSSLIgnore);
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


            return JsonConvert.SerializeObject(aggregate);
        }
    }
}
