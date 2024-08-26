using System;
using System.Text.Json;
using stock_quote_alert.Services.JSONServices;

class StockDataRecord
{
    public DateTime? MarketTime { set; get; }
    public string? Symbol { set; get; }
    public string? Name { set; get; }
    public decimal? Price { set; get; }
    public decimal? Open { set; get; }
    public decimal? High { set; get; }
    public decimal? Low { set; get; }
    public decimal? Change { set; get; }

    public StockDataRecord(JsonElement jsonElement)
    {
        var data = jsonElement.GetProperty("data");
        MarketTime = ConvertUtcToBrasilia(data.GetProperty("last_update_utc").GetString() ?? "");
        Symbol = data.GetProperty("symbol").GetString() ?? "";
        Name = data.GetProperty("name").GetString() ?? "";
        Price = data.GetProperty("price").GetDecimal();
        Open = data.GetProperty("open").GetDecimal();
        High = data.GetProperty("high").GetDecimal();
        Low = data.GetProperty("low").GetDecimal();
        Change = data.GetProperty("change").GetDecimal();
    }

    public StockDataRecord() { }

    /// <summary>
    /// Update StockDataRecord instead of creating new object
    /// To improve performance and avoid memory expenses 
    /// </summary>
    /// <param name="jsonElement"></param>
    public void UpdateStockDataRecord(JsonElement jsonElement)
    {
        var result = jsonElement.GetProperty("results")[0];
        MarketTime = ConvertUtcToBrasilia(result.GetProperty("regularMarketTime").GetString() ?? "");
        Symbol = result.GetProperty("symbol").GetString() ?? "";
        Name = result.GetProperty("longName").GetString() ?? "";
        Price = result.GetProperty("regularMarketPrice").GetDecimal();
        Open = result.GetProperty("regularMarketOpen").GetDecimal();
        High = result.GetProperty("regularMarketDayHigh").GetDecimal();
        Low = result.GetProperty("regularMarketDayLow").GetDecimal();
        Change = result.GetProperty("regularMarketChange").GetDecimal();
    }

    public override string ToString()
    {
        return $"UTC : {MarketTime}; Symbol: {Symbol}; Name: {Name}; Price: {Price}; Open: {Open}; High: {High}; Low: {Low}; Change: {Change}";
    }

    public static DateTime ConvertUtcToBrasilia(string utcDateString)
    {
        DateTime utcDateTime = DateTime.Parse(
            utcDateString,
            null,
            System.Globalization.DateTimeStyles.RoundtripKind
        );
        TimeZoneInfo brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
            "E. South America Standard Time"
        );
        DateTime brasiliaDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, brasiliaTimeZone);

        return brasiliaDateTime;
    }
}
