using System.Threading.Tasks;
using TasksManager.ViewModels.Responses;

namespace TasksManager.DataAccess.Projects.Queries
{
    public interface IProjectQuery
    {
        Task<ProjectResponse> RunAsync(int projectId);
    }
}
