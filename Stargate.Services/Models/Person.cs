using System.ComponentModel.DataAnnotations.Schema;

namespace Stargate.Services.Models;

[Table("Person")]
public class Person : UserBase
{
    //uniquekey
    //public override string ToString() => $@"{FirstName}{LastName}".Trim();
}
