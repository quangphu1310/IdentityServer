using IdentityServer.Models;

namespace IdentityServer.Repositories.IRepositories
{
    public interface ITokenRepository
    {
        Task SaveRefreshToken(RefreshToken refreshToken); 
        Task<RefreshToken> GetByToken(string token);
        Task RevokeToken(string token);
    }
}
