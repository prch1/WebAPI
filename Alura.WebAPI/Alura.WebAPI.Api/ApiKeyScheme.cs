using Microsoft.OpenApi.Models;

namespace Alura.WebAPI.Api
{
    internal class ApiKeyScheme : OpenApiSecurityScheme
    {
        public string Name { get; set; }
        public string In { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}