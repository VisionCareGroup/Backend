using Newtonsoft.Json;

namespace VisionCareCore.User.Domain.Model.Aggregates;

public class AuthUser(string email, string passwordHash,string name,string lastname,string registerArea,DateTime registeredAt,string role)

{

    public AuthUser(): this(string.Empty, string.Empty,string.Empty,string.Empty,string.Empty,DateTime.UtcNow,"User"){}
    
    public Guid Id { get; }
    
    public string Email { get; private set; } = email;
    
    [JsonIgnore] public string PasswordHash { get; private set; } = passwordHash;
    public string Name { get; private set; } = name;
    public string LastName { get; private set; } = lastname;
    public string RegisterArea { get; set; } = registerArea; 
    
    public DateTime RegisteredAt { get; set; } = registeredAt;
    public string Role { get; private set; } = role;
    
    public bool IsActive { get; private set; } = false; // Por defecto, la cuenta estará inactiva
    public List<AuthUserRefreshToken> RefreshTokens { get; set; } = new();
    
    
    public AuthUser updateEmail(string email)
    {
        Email = email;
        return this;
    }

    public AuthUser updatePassword(string password)
    {
        PasswordHash = password;
        return this;
    }
    public void SetPassword(string newPassword)
    {
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
    }
  
}