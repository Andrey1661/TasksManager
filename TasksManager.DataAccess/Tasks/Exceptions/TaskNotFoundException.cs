using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.DataAccess.Tasks.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public int TaskId { get; }

        public TaskNotFoundException(int taskId)
        {
            TaskId = taskId;
        }
    }
}
