using Microsoft.EntityFrameworkCore;
using Stargate.Services.Data;
using Stargate.Services.Models;

namespace Stargate.Services.Repos;

public class PersonRepo : StargateRepo<Person>
{
    private readonly StargateContext _context;
    public PersonRepo(StargateContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<Person> GetAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.People
            .Where(p => p.UserName == username).FirstOrDefaultAsync();
    }
}
