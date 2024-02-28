using Microsoft.EntityFrameworkCore;
using Stargate.Services.Data;
using Stargate.Services.Models;

namespace Stargate.Services
{
    public interface IAstronautService
    {
        public Task<Astronaut> ReadAsync(string nam3);
    }

    public class AstronautService : IAstronautService
    {
        private readonly StargateContext? _context;

        public AstronautService(StargateContext context)
        {
            _context = context;
        }
        public async Task<Astronaut> ReadAsync(string name)
        {
            return await _context.Astronauts
            .Include(p => p.Person)
            .Include(d => d.Duties)
            //.Where(p => p.Person.FirstName.Contains(name) || p.Person.LastName.Contains(name)).FirstOrDefaultAsync();
            .Where(p => p.Person.LastName == name).FirstOrDefaultAsync();
        }
    }
}
