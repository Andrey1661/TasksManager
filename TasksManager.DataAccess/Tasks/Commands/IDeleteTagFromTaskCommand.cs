using System.Threading.Tasks;
using TasksManager.ViewModels.Responses;

namespace TasksManager.DataAccess.Tasks.Commands
{
    public interface IDeleteTagFromTaskCommand
    {
        Task<TaskResponse> ExecuteAsync(int taskId, string tag);
    }
}
