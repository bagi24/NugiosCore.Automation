using System.Net.Http.Json;
using Api.NugiosCore.DTOs;

namespace Api.NugiosCore.Authentication;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResponse> LoginAsync(
        LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/auth/login",
            request
        );

        response.EnsureSuccessStatusCode();

        var loginResponse =
            await response.Content.ReadFromJsonAsync<LoginResponse>();

        return loginResponse
            ?? throw new InvalidOperationException(
                "Login response was empty."
            );
    }
}