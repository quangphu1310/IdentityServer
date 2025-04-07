using IdentityServer.Models;

namespace IdentityServer.Repositories.IRepositories
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        User GetById(int id);
        void Add(User user);

    }
}
