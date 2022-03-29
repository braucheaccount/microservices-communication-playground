using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.cs.Models;

namespace UserService.cs.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> ListUsersAsync();

        Task<User> FindUserByIdAsync(Guid id);

        Task<User> CreateUserAsync(User newUser);

        Task DeleteUserAsync(Guid id);
    }
}
