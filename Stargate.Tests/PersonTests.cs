using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Stargate.Services.Extensions;
using Stargate.Services.Models;
using System.Net;

namespace Stargate.Tests
{
    public class PersonTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private Fixture _fixture;
        readonly HttpClient _client;

        public PersonTests(WebApplicationFactory<Program> application)
        {
            _fixture = new Fixture();
            _client = application.CreateClient();
        }

        /// <summary>
        /// Retrieve all people.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GET_all()
        {
            var response = await _client.GetAsync("/people");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(1)]
        public async Task GET_valid(int id)
        {
            var response = await _client.GetAsync($"/person/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(0)]
        public async Task GET_invalid(int id)
        {
            var response = await _client.GetAsync($"/person/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Retrieve a person by name.
        /// </summary>
        /// <param name="username"></param>
        [Theory]
        [InlineData("wriker")]
        public async Task GET_byname(string username)
        {
            var response = await _client.GetAsync($"/person/{username}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// try to sign up twice
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("wriker")]
        public async Task POST_duplicate_byname(string username)
        {
            var person = new Person
            {
                UserName = username
            };

            var response = await _client.PostAsync("/person", person.GetJsonContent());

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }


        [Theory]
        [InlineData("redshirt")]
        public async Task POST_byname(string username)
        {
            var person = new Person
            {
                UserName = username
            };

            var response = await _client.PostAsync("/person", person.GetJsonContent());

            //clean up
            var responseContent = await response.Content.ReadAsStringAsync();

            person = responseContent.FromJson<Person>();

            var deleteResponse = await _client.DeleteAsync($"/person/{person.Id}");

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Theory]
        [InlineData("yellowhirt")]
        public async Task PUT_byname(string username)
        {
            //arrage
            var person = new Person
            {
                UserName = username
            };

            //act

            //create test record
            var createResponse = await _client.PostAsync("/person", person.GetJsonContent());

            var responseContent = await createResponse.Content.ReadAsStringAsync();
            person = responseContent.FromJson<Person>();

            //update the record
            person.FirstName = "Yellow";
            person.LastName = "Shirt";

            var updateResponse = await _client.PutAsync("/person", person.GetJsonContent());

            //delete the record
            var deleteResponse = await _client.DeleteAsync($"/person/{person.Id}");

            //assert
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}