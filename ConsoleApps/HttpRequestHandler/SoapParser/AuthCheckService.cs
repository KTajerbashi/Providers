using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace HttpRequestHandler.SoapParser
{
    public class AuthCheckService
    {
        private const string SecretKey = "b7Dk29XmPqX2vBn4BtYw6Uz1HsEfGcQ2";
        public static string GenerateToken()
        {
            var key = Encoding.UTF8.GetBytes(SecretKey);

            var tokenHandler = new JwtSecurityTokenHandler();

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("app", "SoapParser"),
                        new Claim("user", "tajer")
                    }),

                Expires = DateTime.Now.AddSeconds(30),

                SigningCredentials =
                        new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task CallSecureApi()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GenerateToken());

            client.DefaultRequestHeaders.Add("ClientName", "Provider");
            var response = await client.GetAsync("https://localhost:44304/api/test/secure");
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }
    }
}
