using Microsoft.EntityFrameworkCore;
using Stargate.Services.Models;

namespace Stargate.Services.Data;

public class StargateContext : DbContext
{
    public DbSet<Person> People { get; set; }
    public DbSet<Astronaut> Astronauts { get; set; }
    public DbSet<Duty> Duties { get; set; }

    public StargateContext(DbContextOptions<StargateContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StargateContext).Assembly);

        //SeedData(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    //private static void SeedData(ModelBuilder modelBuilder)
    //{
    //    //add seed data
    //    modelBuilder.Entity<Person>()
    //        .HasData(
    //            new Person
    //            {
    //                Id = 1,
    //                LastName = "Picard",
    //                FirstName = "Jean-Luc"
    //            },
    //            new Person
    //            {
    //                Id = 2,
    //                LastName = "Riker",
    //                FirstName = "William"
    //            },
    //            new Person
    //            {
    //                Id = 3,
    //                LastName = "Soong",
    //                FirstName = "Data"
    //            },
    //            new Person
    //            {
    //                Id = 4,
    //                LastName = "La Forge",
    //                FirstName = "Geordi"
    //            },
    //            new Person
    //            {
    //                Id = 5,
    //                LastName = "Crusher",
    //                FirstName = "Beverly"
    //            }
    //        );

    //    modelBuilder.Entity<Astronaut>()
    //        .HasData(
    //            new Astronaut
    //            {
    //                Id= 1,
    //                PersonId = 1,
    //            },
    //            new Astronaut
    //            {
    //                Id= 2,
    //                PersonId = 2,
    //            },
    //            new Astronaut
    //            {
    //                Id= 3,
    //                PersonId = 3,
    //            },
    //            new Astronaut
    //            {
    //                Id= 4,
    //                PersonId = 4,
    //            }
    //        );

    //    modelBuilder.Entity<Duty>()
    //        .HasData(
    //            new Duty
    //            {
    //                AstronautId = 1,
    //                Rank = Rank.Captain,
    //                Title = Title.Commander,
    //                StartDate = DateTime.Parse("01/01/1987"),
    //                EndDate = DateTime.Parse("01/01/2004"),
    //            },
    //            new Duty
    //            {
    //                AstronautId = 1,
    //                Rank = Rank.Captain,
    //                Title = Title.RETIRED,
    //                StartDate = DateTime.Parse("01/01/2004"),
    //            },
    //            new Duty
    //            {
    //                AstronautId = 2,
    //                Rank = Rank.FirstOfficer,
    //                Title = Title.Commander,
    //                StartDate = DateTime.Parse("01/01/1987"),
    //                EndDate = DateTime.Parse("01/01/2004"),
    //            },
    //            new Duty
    //            {
    //                AstronautId = 2,
    //                Rank = Rank.Captain,
    //                Title = Title.Commander,
    //                StartDate = DateTime.Parse("01/01/2004")
    //            },
    //            new Duty
    //            {
    //                AstronautId = 3,
    //                Rank = Rank.SecondLieutenant,
    //                Title = Title.Commander,
    //                StartDate = DateTime.Parse("01/01/1987"),
    //                EndDate  = DateTime.Parse("01/01/1990"),
    //            },
    //            new Duty
    //            {
    //                AstronautId = 3,
    //                Rank = Rank.FirstLieutenant,
    //                Title = Title.Commander,
    //                StartDate = DateTime.Parse("01/01/1990"),
    //            }
    //        );
    //}
}
