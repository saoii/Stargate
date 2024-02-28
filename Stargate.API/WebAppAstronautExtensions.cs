using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Stargate.Services.Models;
using Stargate.Services.Repos;

namespace Stargate.API;

public static class WebAppAstronautExtensions
{
    public static void MapAstronautEndpoints(this WebApplication app)
    {
        app.MapGet("/astronauts", async (IRepository<Astronaut> repo, ILogger<Astronaut> logger) =>
        {
            var allAstronauts = await repo.AllAsync();

            logger.LogInformation("Listing all astronauts");

            return Results.Ok(allAstronauts);

        }).Produces<Astronaut[]>(StatusCodes.Status200OK).WithOpenApi().WithName("ListAstronaut");

        app.MapGet("/astronaut/{AstronautId:int}", async (int AstronautId, IRepository<Astronaut> repo, ILogger<Astronaut> logger) =>
        {
            var Astronaut = await repo.GetAsync(AstronautId);

            if (Astronaut == null)
            {
                string message = $"Astronaut with ID {AstronautId} not found.";

                logger.LogError(message);

                return Results.Problem(message, statusCode: 404);
            }

            return Results.Ok(Astronaut);
        }).ProducesProblem(404).Produces<Astronaut>(StatusCodes.Status200OK).WithOpenApi().WithName("GetAstronaut");

        app.MapPost("/astronaut", async ([FromBody] Astronaut Astronaut, IRepository<Astronaut> repo, ILogger<Astronaut> logger) =>
        {
            if (!MiniValidator.TryValidate(Astronaut, out var errors))
                return Results.ValidationProblem(errors);

            var newAstronaut = await repo.AddAsync(Astronaut);

            logger.LogInformation("Created new astronaut");

            return Results.Created($"/astronaut/{Astronaut.Id}", newAstronaut);

        }).ProducesValidationProblem().Produces<Astronaut>(StatusCodes.Status201Created).WithOpenApi().WithName("CreateAstronaut");

        app.MapPut("/astronaut", async ([FromBody] Astronaut Astronaut, IRepository<Astronaut> repo, ILogger<Astronaut> logger) =>
        {
            if (!MiniValidator.TryValidate(Astronaut, out var errors))
                return Results.ValidationProblem(errors);

            if (await repo.GetAsync(Astronaut.Id) == null)
            {
                string message = $"Astronaut with ID {Astronaut.Id} not found.";

                logger.LogError(message);

                return Results.Problem(message, statusCode: 404);
            }

            var updatedAstronaut = await repo.UpdateAsync(Astronaut);

            logger.LogInformation($"Updated astronaut {Astronaut.Person.ToString()}");

            return Results.Ok(updatedAstronaut);
        }).ProducesValidationProblem().ProducesProblem(404).Produces<Astronaut>(StatusCodes.Status200OK).WithOpenApi().WithName("UpdateAstronaut");

        app.MapDelete("/astronaut/{AstronautId:int}", async (int AstronautId, IRepository<Astronaut> repo, ILogger<Astronaut> logger) =>
        {
            var Astronaut = await repo.GetAsync(AstronautId);

            if (Astronaut == null)
            {
                string message = $"Astronaut with ID {AstronautId} not found.";

                logger.LogError(message);

                return Results.Problem(message, statusCode: 404);
            }

            await repo.DeleteAsync(Astronaut);

            return Results.Ok();
        }).ProducesProblem(404).Produces(StatusCodes.Status200OK).WithOpenApi().WithName("DeleteAstronaut");
    }
}