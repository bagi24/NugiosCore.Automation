using Api.NugiosCore.Authentication;
using Api.NugiosCore.Clients;
using Api.NugiosCore.DTOs;
using Xunit;

namespace Api.Tests.NugiosCore;

public class UsersApiTests : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly UsersApiClient _usersApi;
    private readonly AuthService _authService;

    // xUnit-ში [SetUp]-ის მაგივრად გამოიყენება კონსტრუქტორი
    public UsersApiTests()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://dummyjson.com")
        };

        _usersApi = new UsersApiClient(_httpClient);
        _authService = new AuthService(_httpClient);
    }

    // [TearDown]-ის მაგივრად გამოიყენება Dispose მეთოდი
    public void Dispose()
    {
        _httpClient.Dispose();
    }

  [Fact]
    public async Task GetUser_ShouldReturnCorrectUser()
    {
        try
        {
            UserResponse user = await _usersApi.GetUserAsync(1);

            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
            Assert.Equal("wrong_username", user.Username);
            Assert.NotEmpty(user.Email);
        }
        catch (Exception ex)
        {
            // ჩაფეილებისას ლოგების გაგზავნა n8n-ში
            await N8nWebhookReporter.SendFailureAlertAsync(
                nameof(GetUser_ShouldReturnCorrectUser),
                ex.Message,
                ex.StackTrace ?? string.Empty
            );

            // ხელახლა ვისვრით Exception-ს, რომ xUnit-მაც ჩაფეილებულად ჩათვალოს ტესტი
            throw;
        }
    }

    [Fact]
    public async Task Login_ShouldReturnAccessToken()
    {
        var loginRequest = new LoginRequest("emilys", "emilyspass");

        LoginResponse loginResponse = await _authService.LoginAsync(loginRequest);

        Assert.Equal("emilys", loginResponse.Username);
        Assert.NotEmpty(loginResponse.AccessToken);
        Assert.NotEmpty(loginResponse.RefreshToken);
    }
}