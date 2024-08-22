using System.Text.Json;

namespace stock_quote_alert.Services.JSONServices
{
    class JSONConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<JsonElement> ConvertHttpResponse2JSON(HttpResponseMessage response)
        {
            var JsonString = await response.Content.ReadAsStringAsync();
            var JsonElement = JsonSerializer.Deserialize<JsonElement>(JsonString);
            return JsonElement;
        }

        public static List<Subscribers> ParseSubscribers2Json(string jsonString)
        {
            List<Subscribers> subscribersList = new();
            var JsonElement = JsonSerializer.Deserialize<JsonElement>(jsonString);
            foreach (var element in JsonElement.EnumerateArray())
            {
                Subscribers subscriber = new(element);
                subscribersList.Add(subscriber);
            }

            return subscribersList;
        }
    }
}
