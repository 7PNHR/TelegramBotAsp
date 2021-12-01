using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TelegramBotAsp.Commands;
using TelegramBotAsp.Services;

namespace TelegramBotAsp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<DataContext>(opt =>
                opt.UseSqlServer(_configuration.GetConnectionString("Db")), ServiceLifetime.Singleton);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "TelegramBot-Onboarding", Version = "v1"});
            });
            services.AddSingleton<TelegramBot>();
            services.AddSingleton<IMessageHandler, MessageHandler>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<BaseCommand, StartCommand>();
            services.AddSingleton<BaseCommand, TextCommand>();
            services.AddSingleton<BaseCommand, HelpCommand>();
            services.AddSingleton<BaseCommand, AllTempsCommand>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json","TelegramBot-Onboarding"));
            serviceProvider.GetRequiredService<TelegramBot>().GetBot().Wait();
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}