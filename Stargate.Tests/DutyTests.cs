using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Stargate.Services.Models;
using System.Net;

namespace Stargate.Tests
{
    public class DutyTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private Fixture _fixture;
        readonly HttpClient _client;

        public DutyTests(WebApplicationFactory<Program> application)
        {
            _fixture = new Fixture();
            _client = application.CreateClient();
        }

        [Fact]
        public async Task GET_all()
        {
            var response = await _client.GetAsync("/duties");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(1)]
        public async Task GET_valid(int id)
        {
            var response = await _client.GetAsync($"/duty/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(0)]
        public async Task GET_invalid(int id)
        {
            var response = await _client.GetAsync($"/duty/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Retrieve Astronaut Duty by name.
        /// </summary>
        /// <param name="name"></param>
        [Theory]
        [InlineData("jpicard")]
        public async Task GET_byname(string name)
        {
            var response = await _client.GetAsync($"/duty/{name}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// Add an Astronaut Duty, set retirement.
        /// </summary>
        /// <param name="name"></param>
        [Theory]
        [InlineData("wriker")]
        public async Task POST_set_promotion(string username)
        {
            var response = await _client.PostAsync($"/duty/promote/{username}", null);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        /// <summary>
        /// Add an Astronaut Duty, set retirement.
        /// </summary>
        /// <param name="name"></param>
        [Theory]
        [InlineData("wriker")]
        public async Task POST_set_retirement(string username)
        {
            var response = await _client.PostAsync($"/duty/retire/{username}", null);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }


        /// <summary>
        /// Add an Astronaut Duty, change title.
        /// </summary>
        /// <param name="name"></param>
        [Theory]
        [InlineData("glaforge", Title.Commander)]
        public async Task POST_set_title(string username, Title newTitle)
        {
            var response = await _client.PostAsync($"/duty/changetitle/{username}?newTitle={newTitle}", null);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}