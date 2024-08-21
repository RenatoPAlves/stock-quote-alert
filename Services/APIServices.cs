using System.Net.Http.Headers;

namespace APIServices
{
    
class APIHandler
{
    private HttpClient httpClient;
    private HttpRequestMessage httpRequestMessage;
    const string baseURL = "https://real-time-finance-data.p.rapidapi.com";

    public APIHandler()
    {
        this.httpClient = new HttpClient();
        this.httpRequestMessage = new HttpRequestMessage();
    }

    public Task<HttpResponseMessage> GetStockQuote(string symbol, string market = "BVMF")
    {
        httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{baseURL}/stock-quote?symbol={symbol}%3A{market}&language=en"),
            Headers =
            {
                { "x-rapidapi-key", "b8c33ec124msh52c407e6a80545fp17d985jsnd4663d14422d" },
                { "x-rapidapi-host", "real-time-finance-data.p.rapidapi.com" },
            },
        };

        return httpClient.SendAsync(httpRequestMessage);
    }
}
}
