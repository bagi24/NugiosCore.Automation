namespace Api.NugiosCore.DTOs;

public record LoginResponse(
    int Id,
    string Username,
    string Email,
    string AccessToken,
    string RefreshToken
);