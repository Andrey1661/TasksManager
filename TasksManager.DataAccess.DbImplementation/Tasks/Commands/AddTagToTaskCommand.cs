using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.DataAccess.Tasks.Commands;
using TasksManager.DataAccess.Tasks.Exceptions;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModels.Responses;
using Task = TasksManager.Entities.Task;

namespace TasksManager.DataAccess.DbImplementation.Tasks.Commands
{
    public class AddTagToTaskCommand : IAddTagToTaskCommand
    {
        private readonly TasksManagerDbContext _context;
        private readonly IMapper _mapper;

        public AddTagToTaskCommand(TasksManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskResponse> ExecuteAsync(int taskId, string tag)
        {
            Task task =
                await _context.Tasks.Include(t => t.TaskTags)
                    .ThenInclude(t => t.Tag)
                    .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null) throw new TaskNotFoundException(taskId);

            if (!task.TaskTags.Select(t => t.Tag.Name).Contains(tag))
            {
                Tag tagToAdd = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tag) ?? new Tag { Name = tag };

                await _context.TaskTag.AddAsync(new TaskTag { TaskId = taskId, Tag = tagToAdd });
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<Task, TaskResponse>(task);
        }
    }
}
