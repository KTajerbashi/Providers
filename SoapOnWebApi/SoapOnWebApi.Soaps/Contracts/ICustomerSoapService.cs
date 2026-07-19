using SoapOnWebApi.Soaps.Models;
using System.ServiceModel;

namespace SoapOnWebApi.Soaps.Contracts;

[ServiceContract(Namespace = "http://yourcompany.com/customers")]
public interface ICustomerSoapService
{
    [OperationContract]
    Task<CustomerSoapDto> GetCustomerAsync(int customerId);

    [OperationContract]
    Task<CustomerSoapDto[]> GetAllCustomersAsync();

    [OperationContract]
    Task<int> CreateCustomerAsync(CreateCustomerSoapRequest request);
}
