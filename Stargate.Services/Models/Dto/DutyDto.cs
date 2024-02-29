namespace Stargate.Services.Models.Dto;

public record DutyDto(int Id, string? Name, string? Rank, string? Title, DateTime startDate, DateTime endDate);
public record AstronautDto(int id, string? Name, List<DutyDto> Duties);
