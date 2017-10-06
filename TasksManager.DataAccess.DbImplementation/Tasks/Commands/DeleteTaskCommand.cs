using System.Threading.Tasks;
using TasksManager.DataAccess.Tasks.Commands;
using TasksManager.Db;
using Task = TasksManager.Entities.Task;

namespace TasksManager.DataAccess.DbImplementation.Tasks.Commands
{
    public class DeleteTaskCommand : IDeleteTaskCommand
    {
        private readonly TasksManagerDbContext _context;

        public DeleteTaskCommand(TasksManagerDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task ExecuteAsync(int taskId)
        {
            Task task = await _context.Tasks.FindAsync(taskId);

            if (task == null) return;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
