using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppCore.Data;
using TodoAppCore.DTOs;
using TodoAppCore.Entities;
using TodoAppCore.Interfaces;

namespace TodoAppCore.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public TodoRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TodoDto>> GetCancelTodo(int userid)
        {            
            return await _context.Todos.Where(t => t.Status == Entities.EStatus.Cancel && t.UserId == userid)
                .ProjectTo<TodoDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<TodoDto>> GetDoneTodo(int userid)
        {
            return await _context.Todos.Where(t => t.Status == Entities.EStatus.Done && t.UserId == userid)
                .ProjectTo<TodoDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<TodoDto>> GetDoingTodo(int userid)
        {
            return await _context.Todos.Where(t => t.Status == Entities.EStatus.Doing && t.UserId == userid)
                .ProjectTo<TodoDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task UpdateStatusTodo(int todoId, Entities.EStatus status)
        {
            var todo = await _context.Todos.SingleOrDefaultAsync(x => x.Id == todoId);
            todo.Status = status;
        }

        public async Task<TodoDto> GetTodo(int todoId)
        {
            var todo = await _context.Todos
                .ProjectTo<TodoDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == todoId);
            return todo;
        }

        public void NewTodo(Todo todo)
        {
            _context.Todos.Add(todo);
        }
    }
}
