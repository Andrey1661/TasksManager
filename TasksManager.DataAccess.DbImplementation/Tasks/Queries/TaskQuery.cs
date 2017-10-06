using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.DataAccess.Tasks.Queries;
using TasksManager.Db;
using TasksManager.ViewModels.Responses;
using Task = TasksManager.Entities.Task;

namespace TasksManager.DataAccess.DbImplementation.Tasks.Queries
{
    public class TaskQuery : ITaskQuery
    {
        private readonly TasksManagerDbContext _context;
        private readonly IMapper _mapper;

        public TaskQuery(TasksManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskResponse> RunAsync(int taskId)
        {
            TaskResponse responce =
                await _context.Tasks
                    .Include(task => task.Project)
                    .ThenInclude(proj => proj.Tasks)
                    .Include(task => task.TaskTags)
                    .ThenInclude(taskTag => taskTag.Tag)
                    .Select(t => _mapper.Map<Task, TaskResponse>(t))
                    .FirstOrDefaultAsync(t => t.Id == taskId);

            return responce;
        }
    }
}
