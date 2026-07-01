using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace SoapParserWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ISoapParser
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        string ReadSoapStructure(string endpointUrl, string username, string password, bool isSSLIgnore);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        UserModel GetUserInfo(string nationalCode);

    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    [DataContract]
    public class UserModel
    {
        private string username;
        private string password;
        private bool isActive;
        private string mobile;
        private int age;
        private List<Address> addresses;

        [DataMember]
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        [DataMember]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [DataMember]
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        [DataMember]
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        [DataMember]
        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        [DataMember]
        public List<Address> Addresses
        {
            get { return addresses; }
            set { addresses = value; }
        }
    }

    [DataContract]
    public class Address
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Pelak { get; set; }

        [DataMember]
        public string HouseNumber { get; set; }
    }
}
