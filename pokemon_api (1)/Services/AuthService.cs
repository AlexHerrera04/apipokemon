using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace PokemonAPI.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        // Validates an API key passed in the Authorization header
        public bool ValidateApiKey(HttpRequest request)
        {
            if (!request.Headers.TryGetValue("Authorization", out var extractedKey))
                return false;

            var apiKey = _config["ApiKey"];
            return !string.IsNullOrEmpty(apiKey) && apiKey == extractedKey.ToString();
        }
    }
}
