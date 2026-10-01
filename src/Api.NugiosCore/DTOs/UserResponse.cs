namespace Api.NugiosCore.DTOs;

public record UserResponse(
    int Id,
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Gender,
    string Image
);