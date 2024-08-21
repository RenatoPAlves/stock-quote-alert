using System.Net.Http.Json;
using APIServices;
using JSONServices;
using static StockDataRecord;

var APIHandler = new APIHandler();

// using (var response = await client.SendAsync(request))
// {
// }

using (var response = await APIHandler.GetStockQuote("PETR4"))
{
    response.EnsureSuccessStatusCode();

    var JsonElement = await JSONConverter.ConvertHttpResponse2JSON(response);
    var StockDataRecord = new StockDataRecord(JsonElement);
    Console.WriteLine(StockDataRecord.ToString());
    var body = await response.Content.ReadAsStringAsync();
    //Console.WriteLine(body);
}
