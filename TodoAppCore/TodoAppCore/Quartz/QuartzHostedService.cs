using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppCore.DTOs;
using TodoAppCore.Interfaces;

namespace TodoAppCore.Quartz
{
    public class QuartzHostedService : IQuartzHostedService
    {
        private IScheduler scheduler;

        private IEnumerable<TodoDto> JobsSchedule { get ; set; }
        private readonly IJobFactory _jobFactory;
        public QuartzHostedService(IJobFactory jobFactory)
        {
            _jobFactory = jobFactory;
        }

        public async Task StartAsync()
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            scheduler = await factory.GetScheduler();
            scheduler.JobFactory = _jobFactory;

            foreach (var todo in JobsSchedule)
            {
                var job = CreateJob(todo);
                var trigger = CreateTrigger(todo);
                await scheduler.ScheduleJob(job, trigger);
            }
            await scheduler.Start();
        }

        private static IJobDetail CreateJob(TodoDto todo)
        {
            return JobBuilder
                .Create<MyJob>()
                .WithIdentity(todo.Name)
                .WithDescription(todo.Description)
                .Build();
        }
        private static ITrigger CreateTrigger(TodoDto todo)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity(todo.Name)
                .StartAt(todo.StartDate.ToUniversalTime())
                .WithDescription(todo.Description)
                .Build();
        }

        public async Task StopAsync()
        {
            await scheduler.Shutdown();
        }

        public void SetTodoList(IEnumerable<TodoDto> todos)
        {
            JobsSchedule = todos;
        }
    }
}
