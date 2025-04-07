using IdentityServer.Models;
using IdentityServer.Models.DTOs;

namespace IdentityServer.Services.IServices
{
    public interface IAuthService
    {
        APIResponse Login(LoginRequest loginRequest);
        APIResponse RefreshToken(string refreshToken);
        APIResponse Logout(string refreshToken);
        APIResponse Register(RegisterRequest request);
    }
}
