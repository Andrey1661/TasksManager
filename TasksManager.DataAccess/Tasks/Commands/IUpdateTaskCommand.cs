using System.Threading.Tasks;
using TasksManager.ViewModels.Requests;
using TasksManager.ViewModels.Responses;

namespace TasksManager.DataAccess.Tasks.Commands
{
    public interface IUpdateTaskCommand
    {
        Task<TaskResponse> ExecuteAsync(int taskId, UpdateTaskRequest request);
    }
}
