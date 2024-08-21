using System.Text.Json;
using JSONServices;

class StockDataRecord
{
    private string symbol;
    private string name;
    private string type;
    private decimal price;
    private decimal open;
    private decimal high;
    private decimal low;
    private decimal change;

    public StockDataRecord(JsonElement jsonElement)
    {
        var data = jsonElement.GetProperty("data");
        symbol = data.GetProperty("symbol").GetString() ?? "";
        name = data.GetProperty("name").GetString() ?? "";
        type = data.GetProperty("type").GetString() ?? "";
        price = data.GetProperty("price").GetDecimal();
        open = data.GetProperty("open").GetDecimal();
        high = data.GetProperty("high").GetDecimal();
        low = data.GetProperty("low").GetDecimal();
        change = data.GetProperty("change").GetDecimal();
    }

    public void UpdateStockDataRecord(JsonElement jsonElement)
    {
        var data = jsonElement.GetProperty("data");
        symbol = data.GetProperty("symbol").GetString() ?? "";
        name = data.GetProperty("name").GetString() ?? "";
        type = data.GetProperty("type").GetString() ?? "";
        price = data.GetProperty("price").GetDecimal();
        open = data.GetProperty("open").GetDecimal();
        high = data.GetProperty("high").GetDecimal();
        low = data.GetProperty("low").GetDecimal();
        change = data.GetProperty("change").GetDecimal();
    }

    public decimal GetPrice()
    {
        return price;
    }

    public override string ToString()
    {
        return $"Symbol: {symbol}; Name: {name}; Type: {type}; Price: {price}; Open: {open}; High: {high}; Low: {low}; Change: {change}";
    }
}
