using System;

namespace TasksManager.DataAccess.Projects.Exceptions
{
    public class CannotDeleteProjectWithTasksException : Exception
    {
        public CannotDeleteProjectWithTasksException()
        {
        }

        public CannotDeleteProjectWithTasksException(string message) : base(message)
        {
        }

        public CannotDeleteProjectWithTasksException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
