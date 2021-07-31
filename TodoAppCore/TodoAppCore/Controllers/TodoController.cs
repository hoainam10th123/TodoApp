using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppCore.DTOs;
using TodoAppCore.Entities;
using TodoAppCore.Extensions;
using TodoAppCore.Interfaces;

namespace TodoAppCore.Controllers
{
    [Authorize]
    public class TodoController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        IQuartzHostedService _quartz;

        public TodoController(IUnitOfWork unitOfWork, IMapper mapper, IQuartzHostedService quartz)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _quartz = quartz;
        }

        [HttpGet("quartz")]
        public async Task<ActionResult> GetShedule()
        {
            var list = await _unitOfWork.TodoRepository.GetDoingTodo(User.GetUserId());
            if(list.Count() == 0)
            {
                return NoContent();
            }
            else
            {
                _quartz.SetTodoList(list);

                await _quartz.StartAsync();

                return Ok();
            }            
        }

        [HttpGet("get-cancel-todos")]
        public async Task<ActionResult> GetCancelTodos()
        {
            return Ok(await _unitOfWork.TodoRepository.GetCancelTodo(User.GetUserId()));
        }

        [HttpGet("get-done-todos")]
        public async Task<ActionResult> GetDoneTodos()
        {
            return Ok(await _unitOfWork.TodoRepository.GetDoneTodo(User.GetUserId()));
        }

        [HttpGet("get-doing-todos")]
        public async Task<ActionResult> GetDoingTodos()
        {
            return Ok(await _unitOfWork.TodoRepository.GetDoingTodo(User.GetUserId()));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateStatusTodo(int todoId, Entities.EStatus status)
        {
            await _unitOfWork.TodoRepository.UpdateStatusTodo(todoId, status);
            if(await _unitOfWork.Complete())
                return Ok();

            return BadRequest("Error update todo");
        }

        [HttpPost]
        public async Task<ActionResult> NewTodo(TodoDto todo)
        {
            if (ModelState.IsValid)
            {
                todo.UserId = User.GetUserId();
                todo.StartDate = DateTime.Now.AddMinutes(0.5);
                var todoDb = _mapper.Map<Todo>(todo);
                _unitOfWork.TodoRepository.NewTodo(todoDb);

                if (await _unitOfWork.Complete())
                    return Ok(await _unitOfWork.TodoRepository.GetTodo(todoDb.Id));

                return BadRequest("Error add new todo");
            }
            else
            {
                return BadRequest("ModelState.IsValid=" + ModelState.IsValid);
            }            
        }
    }
}
