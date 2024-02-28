using System.ComponentModel.DataAnnotations.Schema;

namespace Stargate.Services.Models;


[Table("Astronaut")]
public class Astronaut
{
    public int Id { get; set; }
    public int PersonId { get; set; } = 0;
    public virtual Person? Person { get; set; } = null; // Required reference navigation to principal
    public virtual ICollection<Duty> Duties { get; set; } = new HashSet<Duty>(); //one to many
}
