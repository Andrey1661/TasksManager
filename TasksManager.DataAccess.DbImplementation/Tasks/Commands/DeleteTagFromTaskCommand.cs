using System.Linq;
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
    public class DeleteTagFromTaskCommand : IDeleteTagFromTaskCommand
    {
        private readonly TasksManagerDbContext _context;
        private readonly IMapper _mapper;

        public DeleteTagFromTaskCommand(TasksManagerDbContext context, IMapper mapper)
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

            if (task.TaskTags.Select(t => t.Tag.Name).Contains(tag))
            {
                TaskTag taskTagToDelete = task.TaskTags.FirstOrDefault(t => t.Tag.Name == tag);

                _context.TaskTag.Remove(taskTagToDelete);
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<Task, TaskResponse>(task);
        }
    }
}
