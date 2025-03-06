
namespace AuthenticationService;
public interface IUserValidation
{
    Task<(ApplicationUser, IList<string>)?> Validate(string username, string password);
}