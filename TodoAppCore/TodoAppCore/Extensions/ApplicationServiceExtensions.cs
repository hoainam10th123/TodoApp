using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppCore.Data;
using TodoAppCore.Helpers;
using TodoAppCore.Interfaces;
using TodoAppCore.Quartz;
using TodoAppCore.Service;
using TodoAppCore.SignalR;

namespace TodoAppCore.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddDbContext<DataContext>(options =>
            {
                //Install-Package Microsoft.EntityFrameworkCore.SqlServer || options.UseSqlServer
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            // Add Quartz services Install-Package Quartz          
            services.AddScoped<IQuartzHostedService, QuartzHostedService>();
            services.AddScoped<MyJob>();

            services.AddQuartz(//Install-Package Quartz.Extensions.DependencyInjection
                q =>
                {
                    q.SchedulerId = "Scheduler-Core";
                    q.UseMicrosoftDependencyInjectionJobFactory();
                    // these are the defaults
                    q.UseSimpleTypeLoader();
                    q.UseInMemoryStore();
                    q.UseDefaultThreadPool(tp =>
                    {
                        tp.MaxConcurrency = 10;
                    });
                }
            );
            return services;
        }
    }
}
