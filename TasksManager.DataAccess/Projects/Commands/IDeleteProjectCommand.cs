using System.Threading.Tasks;

namespace TasksManager.DataAccess.Projects.Commands
{
    public interface IDeleteProjectCommand
    {
        Task ExecuteAsync(int projectId);
    }
}
