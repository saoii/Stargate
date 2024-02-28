using System.ComponentModel.DataAnnotations;

namespace Stargate.Services.Models;

public abstract class UserBase
{
    public int Id { get; set; }
    [Required]
    public string UserName { get; set; }
    [MaxLength(50)]
    public string? FirstName { get; set; } = string.Empty;
    [MaxLength(50)]
    public string? LastName { get; set; } = string.Empty;
}
