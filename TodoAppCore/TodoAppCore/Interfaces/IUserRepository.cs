using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppCore.Entities;

namespace TodoAppCore.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
    }
}
