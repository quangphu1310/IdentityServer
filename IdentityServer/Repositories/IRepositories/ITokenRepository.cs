using IdentityServer.Models;

namespace IdentityServer.Repositories.IRepositories
{
    public interface ITokenRepository
    {
        void SaveRefreshToken(RefreshToken refreshToken); 
        RefreshToken GetByToken(string token);
        void RevokeToken(string token);
    }
}
