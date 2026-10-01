namespace Api.NugiosCore.DTOs;

public record LoginRequest(
    string Username,
    string Password
);