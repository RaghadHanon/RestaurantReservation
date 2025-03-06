
namespace AuthenticationService;
public interface IJwtTokenGenerator
{
    string? GenerateToken(ApplicationUser user, IList<string> userRoles);
}
