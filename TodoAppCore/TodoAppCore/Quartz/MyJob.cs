using Microsoft.AspNetCore.SignalR;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppCore.Interfaces;
using TodoAppCore.SignalR;

namespace TodoAppCore.Quartz
{
    public class MyJob : IJob
    {
        private IHubContext<PresenceHub> _presenceHub;
        //private readonly PresenceTracker _presenceTracker;
        //private readonly IUnitOfWork _unitOfWork;

        public MyJob(IHubContext<PresenceHub> presenceHub)
        {
            _presenceHub = presenceHub;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //var todo = _unitOfWork.TodoRepository.GetTodo(1);
            await _presenceHub.Clients.All.SendAsync("NewTaskReceived", context.JobDetail.Key.Name);
            await Console.Out.WriteLineAsync(context.JobDetail.Key.Name);
        }
    }
}
