using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TasksManager.ViewModels;
using TasksManager.ViewModels.Filters;
using TasksManager.ViewModels.Responses;

namespace TasksManager.DataAccess.Tasks.Queries
{
    public interface ITaskListQuery
    {
        Task<ListResponse<TaskResponse>> RunAsync(TaskFilter filter, ListOptions options);
    }
}
