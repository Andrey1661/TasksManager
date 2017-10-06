using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using TasksManager.DataAccess.DbImplementation.Projects.Commands;
using TasksManager.DataAccess.DbImplementation.Projects.Queries;
using TasksManager.DataAccess.DbImplementation.Tags.Commands;
using TasksManager.DataAccess.DbImplementation.Tags.Queries;
using TasksManager.DataAccess.DbImplementation.Tasks.Commands;
using TasksManager.DataAccess.DbImplementation.Tasks.Queries;
using TasksManager.DataAccess.Projects.Commands;
using TasksManager.DataAccess.Projects.Queries;
using TasksManager.DataAccess.Tags.Commands;
using TasksManager.DataAccess.Tags.Queries;
using TasksManager.DataAccess.Tasks.Commands;
using TasksManager.DataAccess.Tasks.Queries;
using TasksManager.Db;

namespace TasksManager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<TasksManagerDbContext>(
                opt => opt.UseSqlServer(Configuration.GetConnectionString("TasksContext")));

            services.AddMvc();
            services.AddAutoMapper();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles(new[]
                {
                    "TasksManager",
                    "TasksManager.DataAccess.DbImplementation"
                });
            });

            RegisterQueriesAndCommands(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TaskManager", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TasksManagerDbContext context)
        {
            context.Database.Migrate();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvcWithDefaultRoute();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager v1");
                c.RoutePrefix = "api-docs";
            });
        }

        private void RegisterQueriesAndCommands(IServiceCollection services)
        {
            services.AddScoped<ICreateProjectCommand, CreateProjectCommand>();
            services.AddScoped<IUpdateProjectCommand, UpdateProjectCommand>();
            services.AddScoped<IDeleteProjectCommand, DeleteProjectCommand>();
            services.AddScoped<IProjectQuery, ProjectQuery>();
            services.AddScoped<IProjectListQuery, ProjectListQuery>();

            services.AddScoped<ITaskQuery, TaskQuery>();
            services.AddScoped<ITaskListQuery, TaskListQuery>();
            services.AddScoped<ICreateTaskCommand, CreateTaskCommand>();
            services.AddScoped<IUpdateTaskCommand, UpdateTaskCommand>();
            services.AddScoped<IDeleteTaskCommand, DeleteTaskCommand>();
            services.AddScoped<IAddTagToTaskCommand, AddTagToTaskCommand>();
            services.AddScoped<IDeleteTagFromTaskCommand, DeleteTagFromTaskCommand>();

            services.AddScoped<IDeleteTagCommand, DeleteTagCommand>();
            services.AddScoped<ITagListQuery, TagListQuery>();
        }
    }
}
