using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Stargate.Services.Models;
using Stargate.Services.Repos;

namespace Stargate.API;

public static class WebAppPersonExtensions
{
    public static void MapPersonEndpoints(this WebApplication app)
    {
        app.MapGet("/people", (IRepository<Person> repo) => repo.AllAsync())
            .Produces<Person[]>(StatusCodes.Status200OK).WithOpenApi().WithName("ListPeople");

        app.MapGet("/person/{personId:int}", async (int personId, IRepository<Person> repo) =>
        {
            var person = await repo.GetAsync(personId);

            if (person == null)
                return Results.Problem($"Person with ID {personId} not found.", statusCode: 404);

            return Results.Ok(person);
        }).ProducesProblem(404).Produces<Person>(StatusCodes.Status200OK).WithOpenApi().WithName("GetPerson");

        app.MapGet("/person/{username}", async (string username, IRepository<Person> repo) =>
        {
            var person = await repo.GetAsync(username);

            if (person == null)
                return Results.Problem($"Person with name {username} not found.", statusCode: 404);

            return Results.Ok(person);
        }).ProducesProblem(404).Produces<Person>(StatusCodes.Status200OK).WithOpenApi().WithName("GetPersonByName");

        app.MapPost("/person", async ([FromBody] Person person, IRepository<Person> repo) =>
        {
            if (!MiniValidator.TryValidate(person, out var errors))
                return Results.ValidationProblem(errors);

            var result = await repo.GetAsync(person.UserName);

            if (result != null)
                return Results.Problem($"Person with username {person.UserName} already exists.", statusCode: 403);

            var newPerson = await repo.AddAsync(person);

            return Results.Created($"/person/{person.Id}", newPerson);

        }).ProducesValidationProblem()
        .Produces<Person>(StatusCodes.Status201Created)
        .Produces<Person>(StatusCodes.Status403Forbidden)
        .WithOpenApi()
        .WithName("CreatePerson");

        app.MapPut("/person", async ([FromBody] Person person, IRepository<Person> repo) =>
        {
            if (!MiniValidator.TryValidate(person, out var errors))
                return Results.ValidationProblem(errors);

            if (await repo.GetAsync(person.Id) == null)
                return Results.Problem($"Person with Id {person.Id} not found", statusCode: 404);

            var updatedperson = await repo.UpdateAsync(person);

            return Results.Ok(updatedperson);
        }).ProducesValidationProblem().ProducesProblem(404).Produces<Person>(StatusCodes.Status200OK).WithOpenApi().WithName("UpdatePerson");

        app.MapDelete("/person/{personId:int}", async (int personId, IRepository<Person> repo) =>
        {
            var person = await repo.GetAsync(personId);

            if (person == null)
                return Results.Problem($"person with Id {personId} not found", statusCode: 404);

            await repo.DeleteAsync(person);

            return Results.Ok();
        }).ProducesProblem(404).Produces(StatusCodes.Status200OK).WithOpenApi().WithName("DeletePerson");
    }
}