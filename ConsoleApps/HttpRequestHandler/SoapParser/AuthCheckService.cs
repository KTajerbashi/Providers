using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace HttpRequestHandler.SoapParser
{
    public class AuthCheckService
    {
        private const string SecretKey = "b7Dk29XmPqX2vBn4BtYw6Uz1HsEfGcQ2";
        //public static string GenerateToken()
        //{
        //    var key = Encoding.UTF8.GetBytes(SecretKey);

        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    var descriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //            {
        //                new Claim("app", "SoapParser"),
        //                new Claim("user", "tajer")
        //            }),

        //        Expires = DateTime.Now.AddMinutes(30),

        //        SigningCredentials =
        //                new SigningCredentials(
        //                    new SymmetricSecurityKey(key),
        //                    SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var token = tokenHandler.CreateToken(descriptor);

        //    return tokenHandler.WriteToken(token);
        //}

        private string GenerateToken()
        {
            return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjbGllbnROYW1lIjoiUHJvdmlkZXIiLCJuYmYiOjE3ODMxNTc3NDgsImV4cCI6MTgxNDI2MTc0OCwiaWF0IjoxNzgzMTU3NzQ4fQ.H02qAEPlELrFC_SAoO_LGhnr__JwURaSf67HmLtldfY";
        }

        public async Task CallSecureApi()
        {
            using HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GenerateToken());

            client.DefaultRequestHeaders.Add("ClientName", "Provider");

            var request = new
            {
                EndpointUrl = "http://localhost:2711/SoapParser.svc?wsdl",
                Username = "",
                Password = "",
                IsSSLIgnore = true
            };

            // Serialize object to JSON
            var json = JsonSerializer.Serialize(request);

            // Create request content
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send POST request
            var response = await client.PostAsync("http://localhost:8080/api/WebServiceParser/ParseService", content);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }
    }
}
