using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppCore.DTOs;
using TodoAppCore.Entities;

namespace TodoAppCore.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoDto>> GetCancelTodo(int userid);
        Task<IEnumerable<TodoDto>> GetDoneTodo(int userid);
        Task<IEnumerable<TodoDto>> GetDoingTodo(int userid);
        Task UpdateStatusTodo(int todoId, Entities.EStatus status);
        Task<TodoDto> GetTodo(int todoId);
        void NewTodo(Todo todo);
    }
}
