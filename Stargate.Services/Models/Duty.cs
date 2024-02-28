using System.ComponentModel.DataAnnotations.Schema;

namespace Stargate.Services.Models;

public enum Title
{
    Astronaut,
    Commander,
    Pilot,
    Engineer,
    Specialist,
    RETIRED
}
public enum Rank
{
    Cadet,
    FirstLieutenant,
    SecondLieutenant,
    FirstOfficer,
    Captain
}

[Table("Duty")]
public class Duty
{
    public int Id { get; set; }

    public int AstronautId { get; set; }

    public Rank Rank { get; set; } = 0;

    public Title Title { get; set; } = 0;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}