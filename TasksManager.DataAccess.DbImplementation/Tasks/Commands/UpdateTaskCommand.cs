using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.DataAccess.Tasks.Commands;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModels.Requests;
using TasksManager.ViewModels.Responses;
using Task = TasksManager.Entities.Task;

namespace TasksManager.DataAccess.DbImplementation.Tasks.Commands
{
    public class UpdateTaskCommand : IUpdateTaskCommand
    {
        private readonly TasksManagerDbContext _context;
        private readonly IMapper _mapper;

        public UpdateTaskCommand(TasksManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskResponse> ExecuteAsync(int taskId, UpdateTaskRequest request)
        {
            Task task =
                await _context.Tasks.Include(t => t.TaskTags)
                    .ThenInclude(t => t.Tag)
                    .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null) return null;

            //Check which of passed tags are contained in db
            List<Tag> existingTags = await _context.Tags.Where(t => request.Tags.Contains(t.Name)).ToListAsync();

            //Separate new tags
            List<Tag> newTags =
                request.Tags.Except(existingTags.Select(t => t.Name)).Select(t => new Tag {Name = t}).ToList();

            //Collection of TaskTag that should be removed from db
            List<TaskTag> taskTagsToRemove = task.TaskTags.Where(t => !request.Tags.Contains(t.Tag.Name)).ToList();

            //Concat new and existing tags in one collection and create list of TaskTag (excluding tags that are already related to current task)
            List<TaskTag> newTaskTags =
                existingTags.Except(task.TaskTags.Select(t => t.Tag))
                    .Concat(newTags)
                    .Select(t => new TaskTag {TaskId = task.Id, Tag = t})
                    .ToList();

            //Reset task's collection of TaskTags (exclude odd items and add new ones)
            task.TaskTags = task.TaskTags.Except(taskTagsToRemove).Concat(newTaskTags).ToList();

            task = _mapper.Map(request, task);

            _context.TaskTag.RemoveRange(taskTagsToRemove);
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<Task, TaskResponse>(task);
        }
    }
}
