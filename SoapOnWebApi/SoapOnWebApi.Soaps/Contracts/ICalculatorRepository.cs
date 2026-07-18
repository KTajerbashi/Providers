namespace SoapOnWebApi.Soaps.Contracts;

public interface ICalculatorRepository
{
    Task<int> Sum(int a, int b);
    Task<int> Sub(int a, int b);
    Task<int> Multiply(int a, int b);
    Task<int> Divided(int a, int b);
}
