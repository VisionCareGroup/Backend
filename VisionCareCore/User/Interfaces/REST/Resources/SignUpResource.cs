namespace VisionCareCore.User.Interfaces.REST.Resources;

public record SignUpResource(string Email, string Password,string Name,string LastName,string registerArea,DateTime dateCreated,string Role);