using System.Text.Json;

class Subscribers
{
    public string Email { get; private set; }
    public string Name { get; private set; }

    public Subscribers(JsonElement jsonElement)
    {
        Email = jsonElement.GetProperty("email").GetString() ?? "";
        Name = jsonElement.GetProperty("name").GetString() ?? "";
    }
}
