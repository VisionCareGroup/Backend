using VisionCareCore.User.Domain.Model.Commands;
using VisionCareCore.User.Interfaces.REST.Resources;

namespace VisionCareCore.User.Interfaces.REST.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        string role = string.IsNullOrEmpty(resource.Role) ? "User" : resource.Role; // Asignar rol por defecto

        return new SignUpCommand(resource.Email, resource.Password,resource.Name,resource.LastName,resource.registerArea,DateTime.UtcNow,role);
    }
}