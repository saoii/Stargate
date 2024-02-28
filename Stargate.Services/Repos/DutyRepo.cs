using Stargate.Services.Data;
using Stargate.Services.Models;

namespace Stargate.Services.Repos;

public class DutyRepo : StargateRepo<Duty>
{
    private readonly StargateContext _context;
    public DutyRepo(StargateContext context) : base(context)
    {
        _context = context;
    }
}
