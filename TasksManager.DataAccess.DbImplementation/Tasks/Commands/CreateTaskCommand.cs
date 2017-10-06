using System;
using System.Collections;
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
    public class CreateTaskCommand : ICreateTaskCommand
    {
        private readonly TasksManagerDbContext _context;
        private readonly IMapper _mapper;

        public CreateTaskCommand(TasksManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskResponse> ExecuteAsync(CreateTaskRequest request)
        {
            //Check which of passed tags are contained in db
            List<Tag> existingTags = await _context.Tags.Where(t => request.Tags.Contains(t.Name)).ToListAsync();

            //Separate new tags
            List<Tag> newTags = request.Tags.Except(existingTags.Select(t => t.Name)).Select(t => new Tag {Name = t}).ToList();

            //Concat new and existing tags in one collection
            List<Tag> allTags = existingTags.Concat(newTags).ToList();

            Task task = _mapper.Map<CreateTaskRequest, Task>(request);

            task.TaskTags = allTags.Select(t => new TaskTag {Tag = t, Task = task}).ToList();

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return _mapper.Map<Task, TaskResponse>(task);
        }
    }
}
