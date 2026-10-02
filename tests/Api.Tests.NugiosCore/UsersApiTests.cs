using Xunit;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Api.Tests.NugiosCore.Client;
using Api.Tests.NugiosCore.Client.Models;

namespace Api.Tests.NugiosCore
{
    public class UsersApiTests : IClassFixture<TestServerFixture>
    {
        private readonly UsersApiClient _usersApiClient;
        private readonly TestServerFixture _fixture;

        public UsersApiTests(TestServerFixture fixture)
        {
            _fixture = fixture;
            _usersApiClient = new UsersApiClient(_fixture.Client);
        }

        [Fact]
        public async Task CreateUser_ValidRequest_ReturnsCreatedUserAnd201()
        {
            // Arrange
            // Generate unique user data to prevent conflicts with previous test runs
            var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
            var newUser = new UserDto
            {
                UserName = $"testuser_{uniqueId}",
                Email = $"testuser_{uniqueId}@example.com", // FIX: Added Email as it's typically a required field
                Password = "Password123!"
            };

            // Act
            var response = await _usersApiClient.CreateUserAsync(newUser);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created); // Assert that the status code is 201 Created

            var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();
            createdUser.Should().NotBeNull("The created user object should not be null");
            createdUser!.UserName.Should().Be(newUser.UserName, "The UserName of the created user should match the request");
            createdUser.Email.Should().Be(newUser.Email, "The Email of the created user should match the request");
            createdUser.Id.Should().NotBeEmpty("The created user should have a non-empty ID");
            // Optionally, verify other properties or that sensitive data like password is not returned
        }

        // Additional tests would go here, e.g.:
        // [Fact]
        // public async Task GetUserById_ExistingUser_ReturnsUserAnd200()
        // {
        //     // ... test implementation ...
        // }

        // [Fact]
        // public async Task CreateUser_InvalidData_ReturnsBadRequest()
        // {
        //     // Arrange
        //     var invalidUser = new UserDto { UserName = "", Email = "invalid", Password = "short" };
        //
        //     // Act
        //     var response = await _usersApiClient.CreateUserAsync(invalidUser);
        //
        //     // Assert
        //     response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        // }
    }
}
