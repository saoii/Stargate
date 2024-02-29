using Stargate.Services.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stargate.Services.Models;

public enum Title
{
    [FriendlyText("Astronaut")]
    Astronaut,
    [FriendlyText("Commander")]
    Commander,
    [FriendlyText("Pilot")]
    Pilot,
    [FriendlyText("Engineer")]
    Engineer,
    [FriendlyText("Specialist")]
    Specialist,
    [FriendlyText("Retired")]
    RETIRED
}
public enum Rank
{
    [FriendlyText("Cadet")]
    Cadet,
    [FriendlyText("1st Lieutenant")]
    FirstLieutenant,
    [FriendlyText("2nd Lieutenant")]
    SecondLieutenant,
    [FriendlyText("Number One")]
    FirstOfficer,
    [FriendlyText("Captain")]
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