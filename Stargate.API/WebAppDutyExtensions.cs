using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Stargate.Services;
using Stargate.Services.Models;
using Stargate.Services.Repos;

namespace Stargate.API;

public static class WebAppDutyExtensions
{
    public static void MapDutyEndpoints(this WebApplication app)
    {
        app.MapGet("/duties", async (IDutyService service, ILogger<Duty> logger) =>
        {
            var allDutys = await service.ListCurrentAsync();

            logger.LogInformation("Listing all duties");

            return Results.Ok(allDutys);

        }).Produces<Duty[]>(StatusCodes.Status200OK).WithOpenApi().WithName("ListDuty");

        app.MapGet("/duties/{astronautId:int}", async (int astronautId, IDutyService service, ILogger<Duty> logger) =>
        {
            var allDutys = await service.ListHistoryAsync(astronautId);

            logger.LogInformation("Listing all duties");

            return Results.Ok(allDutys);

        }).Produces<Duty[]>(StatusCodes.Status200OK).WithOpenApi().WithName("ListDutyHistory");


        app.MapGet("/duty/{DutyId:int}", async (int DutyId, IDutyService service, ILogger<Duty> logger) =>
        {
            var Duty = await service.ReadAsync(DutyId);

            if (Duty == null)
            {
                string message = $"Duty with ID {DutyId} not found.";

                logger.LogError(message);

                return Results.Problem(message, statusCode: 404);
            }

            logger.LogInformation($"Get current duty for Astronaut: {Duty.Name}");

            return Results.Ok(Duty);
        }).ProducesProblem(404).Produces<Duty>(StatusCodes.Status200OK).WithOpenApi().WithName("GetDuty");

        app.MapGet("/duty/{name}", async (string name, IDutyService service, ILogger<Duty> logger) =>
        {
            var duty = await service.GetCurrentAsync(name);

            if (duty == null)
                return Results.Problem($"Duty with name {name} not found.", statusCode: 404);

            logger.LogInformation($"Got current duty for Astronaut: {duty.AstronautId}");

            return Results.Ok(duty);
        }).ProducesProblem(404).Produces<Person>(StatusCodes.Status200OK).WithOpenApi().WithName("GetDutyByName");

        app.MapPost("/duty", async ([FromBody] Duty Duty, IRepository<Duty> repo, ILogger<Duty> logger) =>
        {
            if (!MiniValidator.TryValidate(Duty, out var errors))
                return Results.ValidationProblem(errors);

            var newDuty = await repo.AddAsync(Duty);

            logger.LogInformation($"New duty created: {newDuty.Id}");

            return Results.Created($"/duty/{Duty.Id}", newDuty);

        }).ProducesValidationProblem().Produces<Duty>(StatusCodes.Status201Created).WithOpenApi().WithName("CreateDuty");

        app.MapPost("/duty/retire/{username}", async (string username, [FromQuery] DateTime? retirementDate, IDutyService service, ILogger<Duty> logger) =>
        {
            var newDuty = await service.SetRetirementAsync(username, retirementDate);

            if (newDuty == null)
            {
                var message = $"Failed to retire Astronaut: {newDuty.AstronautId}";

                logger.LogError(message);

                return Results.Problem(message, statusCode: 404);
            }

            logger.LogInformation($"Retirement set for Astronaut: {newDuty.AstronautId}");

            return Results.Created($"/duty/{newDuty.Id}", newDuty);

        }).ProducesValidationProblem().Produces<Duty>(StatusCodes.Status201Created).WithOpenApi().WithName("SetRetirement");

        app.MapPost("/duty/promote/{username}", async (string username, IDutyService service, ILogger<Duty> logger) =>
        {
            var newDuty = await service.SetPromotionAsync(username);

            if (newDuty == null)
            {
                var message = $"Failed to promote Astronaut: {newDuty.AstronautId}";

                logger.LogError(message);

                return Results.Problem(message, statusCode: 404);
            }

            logger.LogInformation($"Promotion set for Astronaut: {newDuty.AstronautId}");

            return Results.Created($"/duty/{newDuty.Id}", newDuty);

        }).ProducesValidationProblem().Produces<Duty>(StatusCodes.Status201Created).WithOpenApi().WithName("SetPromotion").WithGroupName("Duty");

        app.MapPost("/duty/changetitle/{username}", async (string username, [FromQuery] Title newTitle, IDutyService service, ILogger<Duty> logger) =>
        {
            var newDuty = await service.ChangeTitleAsync(username, newTitle);

            if (newDuty == null)
            {
                var message = $"Failed to change title for Astronaut: {newDuty.AstronautId}";

                logger.LogError(message);

                return Results.Problem(message, statusCode: 404);
            }

            logger.LogInformation($"Title change for Astronaut: {newDuty.AstronautId}");

            return Results.Created($"/duty/{newDuty.Id}", newDuty);

        }).ProducesValidationProblem().Produces<Duty>(StatusCodes.Status201Created).WithOpenApi().WithName("ChangeTitle");

        app.MapPut("/duty", async ([FromBody] Duty Duty, IRepository<Duty> repo, ILogger<Duty> logger) =>
        {
            if (!MiniValidator.TryValidate(Duty, out var errors))
                return Results.ValidationProblem(errors);

            if (await repo.GetAsync(Duty.Id) == null)
            {
                string message = $"Duty with ID {Duty.Id} not found.";

                logger.LogError(message);

                return Results.Problem(message, statusCode: 404);
            }

            var updatedDuty = await repo.UpdateAsync(Duty);

            logger.LogInformation($"New duty updated: {Duty.Id}");

            return Results.Ok(updatedDuty);
        }).ProducesValidationProblem().ProducesProblem(404).Produces<Duty>(StatusCodes.Status200OK).WithOpenApi().WithName("UpdateDuty");

        app.MapDelete("/duty/{DutyId:int}", async (int DutyId, IRepository<Duty> repo, ILogger<Duty> logger) =>
        {
            var Duty = await repo.GetAsync(DutyId);

            if (Duty == null)
            {
                string message = $"Duty with ID {DutyId} not found.";

                logger.LogError(message);

                return Results.Problem(message, statusCode: 404);
            }

            await repo.DeleteAsync(Duty);

            logger.LogInformation($"Duty deleted: {Duty.Id}");

            return Results.Ok();
        }).ProducesProblem(404).Produces(StatusCodes.Status200OK).WithOpenApi().WithName("DeleteDuty");
    }
}