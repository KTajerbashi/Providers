using System.Net.Http.Headers;

namespace HttpRequestHandler.Desktop
{
    public class AppointmentLicenseService
    {
        public async Task MembersManager()
        {
            // The API URL
            var url = "https://desktop.mashhad.ir/api/AppointmentLicense/MembersManager";

            // Your Bearer token
            var token = "eyJhbGciOiJBMjU2S1ciLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwidHlwIjoiSldUIiwiY3R5IjoiSldUIn0.yINHulAEx3b3LwvNc_1IMzdQezDZqnABNxtpwcsrVew8ULKG1Rkw-OLRMVBn2ayfJfpm1-t7r-LFhSFJxOu3nwqCi9DYUNJd.LnrekeghWhGJ-WOYdPjEEg.Y24w9d3ONw34EqtlhCflfHinJpL-Lhp8j_aRM4KXtB8Hefhxgb-4nvCoZ3cAsq5yGJtrwgkzsBZph1-_e11YyhTYlhpCuqk98OkcP145hzGqNmG5vZ12u7iRP5Ia2fD4tVBRt8GRR2iXmO-v15CAmK9QK6dLvWJabs49CkGc7AcH6JeClacAnb0EtYV7-gvpCL5ozlGes64wWT0yLbJ9-YioI-R85pAjcu9ZRc0Q_arc0VqAwJsRHh8hD2i9r0TpLPKgAkyw5eUyFm3EP3IKoo2JElUz8bXKE9LbcP8BPY_xcNXws_7SJ7j0oluDueQx.0y7MrjlZ7Fc3P_hw1kbj-lgqOtdiNM3HmviKP4xrkqE";

            using (var client = new HttpClient())
            {
                // Add the Bearer token to the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                try
                {
                    // Send GET request
                    var response = await client.GetAsync(url);

                    // Ensure the request was successful
                    response.EnsureSuccessStatusCode();

                    // Read the response content
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Response:");
                    Console.WriteLine(content);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                }
            }
        }

    }
}
