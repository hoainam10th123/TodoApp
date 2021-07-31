using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAppCore.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ITodoRepository TodoRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
