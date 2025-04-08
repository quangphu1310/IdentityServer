using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace IdentityServer.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _context;
        public TokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveRefreshToken(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            _context.SaveChanges();
        }

        public async Task<RefreshToken> GetByToken(string token) =>
            await _context.RefreshTokens.Include(r => r.User).FirstOrDefaultAsync(r => r.Token == token);

        public async Task RevokeToken(string token)
        {
            var rt = await GetByToken(token);
            if (rt != null)
            {
                rt.IsRevoked = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
