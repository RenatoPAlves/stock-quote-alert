using System.Text.Json;

namespace JSONServices
{
    class JSONConverter
    {
        public static async Task<JsonElement> ConvertHttpResponse2JSON(HttpResponseMessage response)
        {
            var JsonString = await response.Content.ReadAsStringAsync();
            var JsonElement = JsonSerializer.Deserialize<JsonElement>(JsonString);
            return JsonElement;
        }
    }
}
