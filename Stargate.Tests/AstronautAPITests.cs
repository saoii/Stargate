using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Stargate.Tests
{
    public class AstronautAPITests : IClassFixture<WebApplicationFactory<Program>>
    {
        private Fixture _fixture;
        readonly HttpClient _client;

        public AstronautAPITests(WebApplicationFactory<Program> application)
        {
            _fixture = new Fixture();
            _client = application.CreateClient();
        }

        [Fact]
        public async Task GET_retrieves_all()
        {
            var response = await _client.GetAsync("/astronauts");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(1)]
        public async Task GET_valid_astronaut(int id)
        {
            var response = await _client.GetAsync($"/astronaut/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(0)]
        public async Task GET_invalid_astronaut(int id)
        {
            var response = await _client.GetAsync($"/astronaut/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}