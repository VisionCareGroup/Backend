using Newtonsoft.Json;

namespace VisionCareCore.User.Domain.Model.Aggregates;

public enum VisualImpairmentLevel
{
    Leve,
    Moderada,
    Grave,
    Severa
}

public class AuthUser(string email, string passwordHash, string name, string lastname, DateTime registeredAt, VisualImpairmentLevel visualImpairment)
{
    public AuthUser()
        : this(string.Empty, string.Empty, string.Empty, string.Empty, DateTime.UtcNow, VisualImpairmentLevel.Leve) { }

    public Guid Id { get; }

    public string Email { get; private set; } = email;

    [JsonIgnore]
    public string PasswordHash { get; private set; } = passwordHash;

    public string Name { get; private set; } = name;

    public string LastName { get; private set; } = lastname;

    public DateTime RegisteredAt { get; set; } = registeredAt;

    public VisualImpairmentLevel VisualImpairment { get; set; } = visualImpairment;

    public bool IsActive { get; private set; } = false;

    public List<AuthUserRefreshToken> RefreshTokens { get; set; } = new();

    public AuthUser UpdateEmail(string email)
    {
        Email = email;
        return this;
    }

    public AuthUser UpdatePassword(string password)
    {
        PasswordHash = password;
        return this;
    }

    public void SetPassword(string newPassword)
    {
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
    }
}