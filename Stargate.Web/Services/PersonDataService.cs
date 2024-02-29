using Stargate.Services.Models;
using System.Text.Json;

namespace Stargate.Web.Services;

public class PersonDataService(HttpClient httpClient)
{
    private readonly HttpClient? _httpClient = httpClient;
    private Person[]? people;
    public async Task<Person[]> GetAll()
    {
        //otherwise refresh the list locally from the API and set expiration to 1 minute in future
        people = await JsonSerializer.DeserializeAsync<Person[]>
                (await _httpClient.GetStreamAsync($"/people/"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        return people ?? [];
    }
}
