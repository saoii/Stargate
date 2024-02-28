using Microsoft.EntityFrameworkCore;
using Stargate.Services.Data;
using Stargate.Services.Models;

namespace Stargate.Services.Repos;

public class AstronautRepo : StargateRepo<Astronaut>
{
    private readonly StargateContext _context;

    public AstronautRepo(StargateContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// List the entire related record
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task<Astronaut> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Astronauts
            .Include(p => p.Person)
            .Include(d => d.Duties)
            .Where(a => a.Id == id).FirstOrDefaultAsync();
    }
}
