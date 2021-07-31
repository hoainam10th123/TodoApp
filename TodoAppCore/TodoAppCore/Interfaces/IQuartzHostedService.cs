using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppCore.DTOs;

namespace TodoAppCore.Interfaces
{
    public interface IQuartzHostedService
    {
        Task StartAsync();
        Task StopAsync();
        void SetTodoList(IEnumerable<TodoDto> todos);
    }
}
