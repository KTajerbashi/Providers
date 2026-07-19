using SoapOnWebApi.Soaps.Contracts;
using SoapOnWebApi.Soaps.Models;
using System.ServiceModel;

namespace SoapOnWebApi.Soaps.Services;

// Presentation/Soap/Services/CustomerSoapService.cs
public class CustomerSoapService : ICustomerSoapService
{

    public CustomerSoapService()
    {
    }

    public async Task<CustomerSoapDto> GetCustomerAsync(int customerId)
    {
        CustomerSoapDto result = new()
        {
            Id = 1,
            Email = $"{customerId}@mail.com",
            Name = $"{customerId} : Name"
        };
        if (result is null)
            throw new FaultException($"Customer with id {customerId} not found.");

        return new CustomerSoapDto
        {
            Id = result.Id,
            Name = result.Name,
            Email = result.Email
        };
    }

    public async Task<CustomerSoapDto[]> GetAllCustomersAsync()
    {
        CustomerSoapDto result1 = new()
        {
            Id = 1,
            Email = $"{Guid.NewGuid()}@mail.com",
            Name = $"{Guid.NewGuid()} : Name"
        };
        CustomerSoapDto result2 = new()
        {
            Id = 2,
            Email = $"{Guid.NewGuid()}@mail.com",
            Name = $"{Guid.NewGuid()} : Name"
        };
        List<CustomerSoapDto> res = new();
        res.Add(result1);
        res.Add(result2);
        return res.ToArray();
    }

    public async Task<int> CreateCustomerAsync(CreateCustomerSoapRequest request)
    {
        await Task.CompletedTask;
        return 1;
    }
}
