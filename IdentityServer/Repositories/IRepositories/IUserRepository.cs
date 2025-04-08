using IdentityServer.Models;

namespace IdentityServer.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsername(string username);
        Task<User> GetById(int id);
        Task Add(User user);

    }
}
