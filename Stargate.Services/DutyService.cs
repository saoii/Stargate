using Microsoft.EntityFrameworkCore;
using Stargate.Services.Data;
using Stargate.Services.Helpers;
using Stargate.Services.Models;
using Stargate.Services.Models.Dto;
using Stargate.Services.Repos;

namespace Stargate.Services;
public interface IDutyService
{
    public Task<Duty> GetCurrentAsync(string username);
    public Task<Duty> ChangeTitleAsync(string username, Title newTitle);
    public Task<Duty> SetRetirementAsync(string username, DateTime? retirementDate);
    public Task<Duty> SetPromotionAsync(string username);
    public Task<DutyDto> ReadAsync(int id);
    public Task<List<DutyDto>> ListCurrentAsync();
}

public class DutyService : IDutyService
{
    private readonly StargateContext? _context;
    private readonly IRepository<Duty> _repo;

    public DutyService(StargateContext context, IRepository<Duty> repo)
    {
        _context = context;
        _repo = repo;
    }

    /// <summary>
    /// get a verion of the duty for updating
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<Duty> GetCurrentAsync(string username)
    {
        var result = await _context.Duties
            .Join(_context.Astronauts, d => d.AstronautId, a => a.Id, (d, a) => new { Duty = d, Astronaut = a })
            .Join(_context.People, da => da.Astronaut.PersonId, p => p.Id, (da, p) => new { DutyAstronaut = da, Person = p })
            .Where(x => x.DutyAstronaut.Duty.EndDate == null && (x.Person.UserName == username))
            .Select(x => new { x.DutyAstronaut.Duty, x.DutyAstronaut.Astronaut, x.Person })
            .FirstOrDefaultAsync();

        return result.Duty;
    }

    /// <summary>
    /// get a human readable version of the Duty
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DutyDto> ReadAsync(int id)
    {
        var result = await _context.Duties
       .Join(_context.Astronauts, d => d.AstronautId, a => a.Id, (d, a) => new { Duty = d, Astronaut = a })
       .Join(_context.People, da => da.Astronaut.PersonId, p => p.Id, (da, p) => new { DutyAstronaut = da, Person = p })
       .Where(x => x.DutyAstronaut.Duty.Id == id)
       .Select(x => new { x.DutyAstronaut.Duty, x.DutyAstronaut.Astronaut, x.Person })
       .FirstOrDefaultAsync();

        var duty = result.Duty;
        var person = result.Person;

        var endDate = duty.EndDate ??= DateTime.MaxValue.Date;

        return new DutyDto(duty.Id, person.UserName, TextGenHelper.GetFriendlyText(duty.Rank), TextGenHelper.GetFriendlyText(duty.Title), duty.StartDate.Date, endDate);
    }

    public async Task<Duty> ChangeTitleAsync(string username, Title newTitle)
    {
        Duty newDuty = new();

        DateTime currentDate = DateTime.Now;

        //get the current duty
        var oldDuty = await GetCurrentAsync(username);

        if (oldDuty.Title != Title.RETIRED)
        {
            //Previous Duty End Date is set to the day before the New Astronaut Duty Start Date
            oldDuty.EndDate = currentDate.AddDays(-1);

            if (await _repo.UpdateAsync(oldDuty))
            {
                newDuty = new Duty
                {
                    AstronautId = oldDuty.AstronautId,
                    Rank = oldDuty.Rank, //keep the current rank
                    Title = newTitle,
                    StartDate = currentDate.Date,
                    EndDate = null //A Person's Career End Date is one day before the Retired Duty Start Date.
                };

                newDuty = await _repo.AddAsync(newDuty);
            }
        }
        return newDuty;
    }
    public async Task<Duty> SetPromotionAsync(string username)
    {
        Duty newDuty = new();

        var currentDate = DateTime.Now.Date;

        //get the current duty
        var oldDuty = await GetCurrentAsync(username);

        if (oldDuty.Rank != Rank.Captain)
        {
            //Previous Duty End Date is set to the day before the New Astronaut Duty Start Date
            oldDuty.EndDate = currentDate.AddDays(-1);


            if (await _repo.UpdateAsync(oldDuty))
            {
                newDuty = new Duty
                {
                    AstronautId = oldDuty.AstronautId,
                    Rank = oldDuty.Rank + 1, //promote
                    Title = oldDuty.Title,
                    StartDate = currentDate,
                    EndDate = null //A Person's Current Duty will not have a Duty End Date
                };

                newDuty = await _repo.AddAsync(newDuty);
            }
        }

        return newDuty;
    }
    public async Task<Duty> SetRetirementAsync(string username, DateTime? retirementDate)
    {
        Duty newDuty = new();

        retirementDate ??= DateTime.Now;

        //get the current duty
        var oldDuty = await GetCurrentAsync(username);

        if (oldDuty.Title != Title.RETIRED)
        {
            //Previous Duty End Date is set to the day before the New Astronaut Duty Start Date
            oldDuty.EndDate = retirementDate.Value.AddDays(-1);

            if (await _repo.UpdateAsync(oldDuty))
            {
                newDuty = new Duty
                {
                    AstronautId = oldDuty.AstronautId,
                    Rank = oldDuty.Rank, //keep the current rank
                    Title = Title.RETIRED,
                    StartDate = retirementDate.Value.Date,
                    EndDate = retirementDate.Value.Date.AddDays(-1) //A Person's Career End Date is one day before the Retired Duty Start Date.
                };

                newDuty = await _repo.AddAsync(newDuty);
            }
        }

        return newDuty;
    }

    /// <summary>
    /// get a readable list of the current duties
    /// </summary>
    /// <returns></returns>
    public async Task<List<DutyDto>> ListCurrentAsync()
    {
        List<DutyDto> dutyDtos = new();

        var result = await _context.Duties
         .Join(_context.Astronauts, d => d.AstronautId, a => a.Id, (d, a) => new { Duty = d, Astronaut = a })
         .Join(_context.People, da => da.Astronaut.PersonId, p => p.Id, (da, p) => new { DutyAstronaut = da, Person = p })
         .Where(x => x.DutyAstronaut.Duty.EndDate == null)
         .Select(x => new { x.DutyAstronaut.Duty, x.DutyAstronaut.Astronaut, x.Person })
         .ToListAsync();

        foreach (var r in result)
        {
            var duty = r.Duty;
            var person = r.Person;

            var endDate = duty.EndDate ??= DateTime.MaxValue.Date;

            var dto = new DutyDto(duty.Id, person.UserName, TextGenHelper.GetFriendlyText(duty.Rank), TextGenHelper.GetFriendlyText(duty.Title), duty.StartDate.Date, endDate);


            dutyDtos.Add(dto);
        }

        return dutyDtos;
    }
}
