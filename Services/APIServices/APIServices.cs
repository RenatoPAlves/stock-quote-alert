using System.Net.Http.Headers;
using dotenv.net;
using stock_quote_alert.Services.EmailServices;
using stock_quote_alert.Services.JSONServices;

namespace stock_quote_alert.Services.APIServices
{
    class APIHandler
    {
        private readonly HttpClient httpClient;
        private HttpRequestMessage httpRequestMessage;
        const string baseURL = "https://brapi.dev/api";
        private readonly EmailHandler EmailHandler;

        public APIHandler()
        {
            httpClient = new HttpClient();
            httpRequestMessage = new HttpRequestMessage();
            EmailHandler = new EmailHandler();
        }

        /// <summary>
        /// Returns Task to get HttpReponseMessage of Stock
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="market"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> GetStockQuote(string symbol, string market = "BVMF")
        {
            DotEnv.Load();
            var BRAPI_Key = Environment.GetEnvironmentVariable("BRAPI_Key");
            httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{baseURL}/quote/{symbol}?token={BRAPI_Key}"),
            };

            return httpClient.SendAsync(httpRequestMessage);
        }

        /// <summary>
        /// Subscribe to get update of given Stock
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="sell"></param>
        /// <param name="buy"></param>
        /// <returns></returns>
        public async Task Subscribe2Stock(string symbol, decimal sell, decimal buy, int timeout = 30)
        {
            var StockDataRecord = new StockDataRecord();
            for (; ; )
            {
                var response = await GetStockQuote(symbol);
                response.EnsureSuccessStatusCode();
                var JsonElement = await JSONConverter.ConvertHttpResponse2JSON(response);
                StockDataRecord.UpdateStockDataRecord(JsonElement);
                Console.WriteLine(StockDataRecord.ToString());

                if (StockDataRecord.Price > sell)
                    EmailHandler.NotifySubscribers(StockDataRecord, sell, Events.TAKE_PROFIT);
                if (StockDataRecord.Price < buy)
                    EmailHandler.NotifySubscribers(StockDataRecord, buy, Events.BUY_THE_DIP);

                await Task.Delay(timeout * 30 * 1000);
            }
        }
    }
}
