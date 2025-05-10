namespace VisionCareCore.User.Domain.Model.Commands;

public record SignUpCommand(string Email, string Password, string Name,string LastName, string RegisterArea, DateTime DateCreatedAt, string Role = "User");
