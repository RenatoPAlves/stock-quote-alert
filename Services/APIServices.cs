using System.Net.Http.Headers;
using dotenv.net;

namespace APIServices
{
    
class APIHandler
{
    private HttpClient httpClient;
    private HttpRequestMessage httpRequestMessage;
    private string API_Key;
    const string baseURL = "https://real-time-finance-data.p.rapidapi.com";

    public APIHandler()
    {
        this.httpClient = new HttpClient();
        this.httpRequestMessage = new HttpRequestMessage();
    }

    public Task<HttpResponseMessage> GetStockQuote(string symbol, string market = "BVMF")
    {
        DotEnv.Load();
        var API_Key = Environment.GetEnvironmentVariable("RapidAPI_Key");
        Console.WriteLine(API_Key);
        httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{baseURL}/stock-quote?symbol={symbol}%3A{market}&language=en"),
            Headers =
            {
                { "x-rapidapi-key", API_Key },
                { "x-rapidapi-host", "real-time-finance-data.p.rapidapi.com" },
            },
        };

        return httpClient.SendAsync(httpRequestMessage);
    }
}
}
