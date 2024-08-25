using stock_quote_alert.Services.APIServices;
using stock_quote_alert.Services.EmailServices;

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
        await APIHandler.Subscribe2Stock(symbol, sell, buy);
    }
}
