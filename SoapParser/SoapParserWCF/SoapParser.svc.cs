using Newtonsoft.Json;
using SoapParserWCF.ProxyFactory;
using SoapParserWCF.ProxyRepository;
using System;

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

        public string ReadSoapStructure(string endpointUrl, string username, string password, bool isSSLIgnore)
        {
            ProxyService result = new ProxyFactory.ProxyFactory().ImportService(endpointUrl, username, password, isSSLIgnore);
            var ss = result.ServiceMethods.Values;

            return JsonConvert.SerializeObject(result);
        }
    }
}
