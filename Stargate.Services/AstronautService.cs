using Microsoft.EntityFrameworkCore;
using Stargate.Services.Data;
using Stargate.Services.Helpers;
using Stargate.Services.Models.Dto;

namespace Stargate.Services
{
    public interface IAstronautService
    {
        public Task<AstronautDto> ReadAsync(string username);
        public Task<List<AstronautDto>> ListCurrentAsync();
    }

    public class AstronautService : IAstronautService
    {
        private readonly StargateContext? _context;

        public AstronautService(StargateContext context)
        {
            _context = context;
        }

        public async Task<List<AstronautDto>> ListCurrentAsync()
        {
            List<AstronautDto> dtos = new();

            var result = await _context.Duties
           .Join(_context.Astronauts, d => d.AstronautId, a => a.Id, (d, a) => new { Duty = d, Astronaut = a })
           .Join(_context.People, da => da.Astronaut.PersonId, p => p.Id, (da, p) => new { DutyAstronaut = da, Person = p })
           .Where(x => x.DutyAstronaut.Duty.EndDate == null)
           .Select(x => new { x.DutyAstronaut.Duty, x.DutyAstronaut.Astronaut, x.Person }).ToListAsync();

            foreach (var r in result)
            {
                var duty = r.Duty;
                var person = r.Person;
                var astronaut = r.Astronaut;

                var astronautDuties = new List<DutyDto>();

                var endDate = duty.EndDate ??= DateTime.MaxValue.Date;

                astronautDuties.Add(new DutyDto(duty.Id, person.UserName, TextGenHelper.GetFriendlyText(duty.Rank), TextGenHelper.GetFriendlyText(duty.Title), duty.StartDate.Date, endDate));

                var dto = new AstronautDto(astronaut.Id, person.UserName, astronautDuties);

                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<AstronautDto> ReadAsync(string username)
        {
            var result = await _context.Duties
           .Join(_context.Astronauts, d => d.AstronautId, a => a.Id, (d, a) => new { Duty = d, Astronaut = a })
           .Join(_context.People, da => da.Astronaut.PersonId, p => p.Id, (da, p) => new { DutyAstronaut = da, Person = p })
            .Where(x => x.Person.UserName == username)
           .Select(x => new { x.DutyAstronaut.Duty, x.DutyAstronaut.Astronaut, x.Person }).FirstOrDefaultAsync();

            var duty = result.Duty;
            var person = result.Person;
            var astronaut = result.Astronaut;

            var astronautDuties = new List<DutyDto>();

            var endDate = duty.EndDate ??= DateTime.MaxValue.Date;

            astronautDuties.Add(new DutyDto(duty.Id, person.UserName, TextGenHelper.GetFriendlyText(duty.Rank), TextGenHelper.GetFriendlyText(duty.Title), duty.StartDate.Date, endDate));

            return new AstronautDto(astronaut.Id, person.UserName, astronautDuties);
        }
    }
}
