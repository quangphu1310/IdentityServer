using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer.Repositories.IRepositories;

namespace IdentityServer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public User GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(x=>x.Username == username);
        }
        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

    }
}
