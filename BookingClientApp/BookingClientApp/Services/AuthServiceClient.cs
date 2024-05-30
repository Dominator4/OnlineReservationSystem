using BookingClientApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BookingClientApp.Services
{
    /// <summary>
    /// Handles communication with the AuthenticationService for login and registration operations.
    /// </summary>
    public class AuthServiceClient
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the AuthServiceClient class.
        /// </summary>
        /// <param name="client">The HTTP client used for making requests.</param>
        public AuthServiceClient(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="model">The registration details including email, password, and confirmation password.</param>
        /// <returns>Returns a success message if the registration is successful; otherwise, returns null.</returns>
        public async Task<string> RegisterAsync(RegisterViewModel model)
        {
            var response = await _client.PostAsync("auth/register", new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                // You can return a token or other data from the response
                return "Registration successful.";
            }
            else
            {
                // Handle errors
                return null;
            }
        }

        /// <summary>
        /// Authenticates a user with the provided login details.
        /// </summary>
        /// <param name="model">The login details including email and password.</param>
        /// <returns>Returns a JWT token if the login is successful; otherwise, returns null.</returns>
        public async Task<string> LoginAsync(LoginViewModel model)
        {
            var response = await _client.PostAsync("auth/login", new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response to get the token
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
            else
            {
                // Handle errors
                return null;
            }
        }
    }
}
