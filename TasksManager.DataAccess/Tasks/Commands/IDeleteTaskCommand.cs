using System.Threading.Tasks;

namespace TasksManager.DataAccess.Tasks.Commands
{
    public interface IDeleteTaskCommand
    {
        Task ExecuteAsync(int taskId);
    }
}
