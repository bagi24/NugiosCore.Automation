using System.Net.Http.Json;
using Api.NugiosCore.DTOs;

namespace Api.NugiosCore.Clients;

public class UsersApiClient
{
    private readonly HttpClient _httpClient;

    public UsersApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserResponse> GetUserAsync(
        int userId)
    {
        var response =
            await _httpClient.GetAsync(
                $"/users/{userId}"
            );

        response.EnsureSuccessStatusCode();

        var user =
            await response.Content
                .ReadFromJsonAsync<UserResponse>();

        return user
            ?? throw new InvalidOperationException(
                "User response was empty."
            );
    }
}