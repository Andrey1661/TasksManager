using System.Threading.Tasks;
using TasksManager.ViewModels.Responses;

namespace TasksManager.DataAccess.Tasks.Commands
{
    public interface IAddTagToTaskCommand
    {
        Task<TaskResponse> ExecuteAsync(int taskId, string tag);
    }
}
