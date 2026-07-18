using SoapOnWebApi.Soaps.Contracts;

namespace SoapOnWebApi.Soaps.Services;

public class CalculatorRepository : ICalculatorRepository
{
    public async Task<int> Divided(int a, int b)
    {

        return await Task.Run(() =>
        {
            return a / b;
        });
    }

    public async Task<int> Multiply(int a, int b)
    {
        return await Task.Run(() =>
        {
            return a * b;
        });
    }

    public async Task<int> Sub(int a, int b)
    {
        return await Task.Run(() =>
        {
            return a - b;
        });
    }

    public async Task<int> Sum(int a, int b)
    {
        return await Task.Run(() =>
        {
            return a + b;
        });
    }
}
