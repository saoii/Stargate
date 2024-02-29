
using Stargate.Services.Models.Dto;
using System.Text.Json;

namespace Stargate.Web.Services;
public class AstronautDataService(HttpClient httpClient)
{
    private readonly HttpClient? _httpClient = httpClient;
    private AstronautDto[]? astronauts;
    public async Task<AstronautDto[]> GetAll()
    {
        //otherwise refresh the list locally from the API and set expiration to 1 minute in future
        astronauts = await JsonSerializer.DeserializeAsync<AstronautDto[]>
                (await _httpClient.GetStreamAsync($"/astronauts/"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        return astronauts ?? [];
    }
}