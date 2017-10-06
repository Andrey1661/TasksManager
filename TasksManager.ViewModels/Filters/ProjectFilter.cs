using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.ViewModels.Filters
{
    public class ProjectFilter
    {
        public string Name { get; set; }

        public int? OpenTasksCountFrom { get; set; }

        public int? OpenTasksCountTo { get; set; }
    }
}
