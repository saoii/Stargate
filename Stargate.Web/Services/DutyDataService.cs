using Stargate.Services.Models.Dto;
using System.Text.Json;

namespace Stargate.Web.Services;

public class DutyDataService(HttpClient httpClient)
{
    private readonly HttpClient? _httpClient = httpClient;
    private DutyDto[]? duties;
    public async Task<DutyDto[]> GetHistory(int astronautId)
    {
        //otherwise refresh the list locally from the API and set expiration to 1 minute in future
        duties = await JsonSerializer.DeserializeAsync<DutyDto[]>
                (await _httpClient.GetStreamAsync($"/duties/{astronautId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        return duties ?? [];
    }

    public async Task<DutyDto> GetCurrent(int astronautId)
    {
        //otherwise refresh the list locally from the API and set expiration to 1 minute in future
        var duty = await JsonSerializer.DeserializeAsync<DutyDto>
                (await _httpClient.GetStreamAsync($"/duties/{astronautId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        return duty;
    }
}
