using BookingClientApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

//Odpowiada za komunikację z `AuthenticationService` w celu logowania i rejestracji.
namespace BookingClientApp.Services
{
    public class AuthServiceClient
    {
        private readonly HttpClient _client;

        public AuthServiceClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> RegisterAsync(RegisterViewModel model)
        {
            var response = await _client.PostAsync("auth/register", new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                // Możesz zwrócić token lub inne dane z odpowiedzi
                return "Rejestracja pomyślna.";
            }
            else
            {
                // Obsługa błędów
                return null;
            }
        }

        public async Task<string> LoginAsync(LoginViewModel model)
        {
            var response = await _client.PostAsync("auth/login", new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {

                // Deserializuj odpowiedź, aby uzyskać token
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
            else
            {
                // Obsługa błędów
                return null;
            }
        }
    }
}
