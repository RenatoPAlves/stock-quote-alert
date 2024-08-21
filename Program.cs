using APIServices;
using JSONServices;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Número insuficiente de argumentos.");
            return;
        }

        string symbol = args[0];
        decimal sell = int.Parse(args[1]);
        decimal buy = int.Parse(args[2]);

        var APIHandler = new APIHandler();
        var response = await APIHandler.GetStockQuote(symbol);
        response.EnsureSuccessStatusCode();

        var JsonElement = await JSONConverter.ConvertHttpResponse2JSON(response);
        var StockDataRecord = new StockDataRecord(JsonElement);
        Console.WriteLine(StockDataRecord.ToString());
        var body = await response.Content.ReadAsStringAsync();
        //Console.WriteLine(body);
    }
}
