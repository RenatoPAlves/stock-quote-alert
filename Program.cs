using System.Net.Http.Headers;
var client = new HttpClient();
var request = new HttpRequestMessage
{
	Method = HttpMethod.Get,
	RequestUri = new Uri("https://real-time-finance-data.p.rapidapi.com/stock-quote?symbol=PETR4%3ABVMF&language=en"),
	Headers =
	{
		{ "x-rapidapi-key", "b8c33ec124msh52c407e6a80545fp17d985jsnd4663d14422d" },
		{ "x-rapidapi-host", "real-time-finance-data.p.rapidapi.com" },
	},
};
using (var response = await client.SendAsync(request))
{
	response.EnsureSuccessStatusCode();
	var body = await response.Content.ReadAsStringAsync();
	Console.WriteLine(body);
}