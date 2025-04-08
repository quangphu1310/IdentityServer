using IdentityServer.Models;

namespace IdentityServer.Services.IServices
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
