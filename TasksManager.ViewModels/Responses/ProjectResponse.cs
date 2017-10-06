using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.ViewModels.Responses
{
    public class ProjectResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int OpenTasks { get; set; }
    }
}
