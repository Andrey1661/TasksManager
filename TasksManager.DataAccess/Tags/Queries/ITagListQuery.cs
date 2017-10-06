using System.Threading.Tasks;
using TasksManager.ViewModels;
using TasksManager.ViewModels.Filters;
using TasksManager.ViewModels.Responses;

namespace TasksManager.DataAccess.Tags.Queries
{
    public interface ITagListQuery
    {
        Task<ListResponse<TagResponce>> RunAsync(TagFilter filter, ListOptions listOptions);
    }
}
