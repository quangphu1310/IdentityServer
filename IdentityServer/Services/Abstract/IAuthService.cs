using IdentityServer.Models;
using IdentityServer.Models.DTOs;

namespace IdentityServer.Services.IServices
{
    public interface IAuthService
    {
        Task<APIResponse> Login(LoginRequest loginRequest);
        Task<APIResponse> RefreshToken(string refreshToken);
        Task<APIResponse> Logout(string refreshToken);
        Task<APIResponse> Register(RegisterRequest request);
    }
}
