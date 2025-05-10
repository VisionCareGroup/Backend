namespace VisionCareCore.User.Interfaces.ACL;

public interface IIamContextFacade
{
    Task<Guid> CreateAuthUser(string email, string password,string name,string lastname,string registerArea,DateTime datecreatedat,string role);
    Task<Guid> FetchAuthUserIdByEmail(string email);
    Task<string> FetchAuthUsernameByUserId(Guid userId);
}