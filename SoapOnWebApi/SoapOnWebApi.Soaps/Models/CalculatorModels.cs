using System.Runtime.Serialization;

namespace SoapOnWebApi.Soaps.Models;

[DataContract]
public class CustomerSoapDto
{
    [DataMember] public int Id { get; set; }
    [DataMember] public string Name { get; set; } = string.Empty;
    [DataMember] public string Email { get; set; } = string.Empty;
}

[DataContract]
public class CreateCustomerSoapRequest
{
    [DataMember] public string Name { get; set; } = string.Empty;
    [DataMember] public string Email { get; set; } = string.Empty;
}