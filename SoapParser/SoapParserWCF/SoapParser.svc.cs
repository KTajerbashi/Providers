using Newtonsoft.Json;
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
            };
        }

    }
}
