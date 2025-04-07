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

        public void SaveRefreshToken(RefreshToken token)
        {
            //var tokenCreate = new RefreshToken
            //{
            //    ExpiryDate = token.ExpiryDate,
            //    UserId = token.UserId,
            //    Token = token.Token
            //};
            _context.RefreshTokens.Add(token);
            _context.SaveChanges();
        }

        public RefreshToken GetByToken(string token) =>
            _context.RefreshTokens.Include(r => r.User).FirstOrDefault(r => r.Token == token);

        public void RevokeToken(string token)
        {
            var rt = GetByToken(token);
            if (rt != null)
            {
                rt.IsRevoked = true;
                _context.SaveChanges();
            }
        }
    }
}
