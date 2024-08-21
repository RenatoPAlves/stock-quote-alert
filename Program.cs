using APIServices;

var APIHandler = new APIServices.APIHandler();

// using (var response = await client.SendAsync(request))
// {
// }

using (var response = await APIHandler.GetStockQuote("PETR4"))
{
    response.EnsureSuccessStatusCode();
    var body = await response.Content.ReadAsStringAsync();
    Console.WriteLine(body);
}
