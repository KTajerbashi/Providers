using System;
using System.Collections.Generic;
using System.Text;

namespace SoapOnWebApi.Soaps.Interfaces;
// Application/Interfaces/ISoapUserValidator.cs
public interface ISoapUserValidator
{
    Task<bool> ValidateAsync(string username, string password);
}