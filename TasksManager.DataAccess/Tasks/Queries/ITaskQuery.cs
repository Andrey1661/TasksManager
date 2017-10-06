using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TasksManager.ViewModels.Responses;

namespace TasksManager.DataAccess.Tasks.Queries
{
    public interface ITaskQuery
    {
        Task<TaskResponse> RunAsync(int taskId);
    }
}
