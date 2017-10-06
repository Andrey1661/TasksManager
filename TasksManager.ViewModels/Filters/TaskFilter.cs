using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.ViewModels.Filters
{
    public class TaskFilter
    {
        public string Name { get; set; }

        public TaskStatus? Status { get; set; }

        public DateTime? CreatedDateFrom { get; set; }

        public DateTime? CreatedDateTo { get; set; }

        public DateTime? DueDateFrom { get; set; }

        public DateTime? DueDateTo { get; set; }

        public DateTime? CompletedDateFrom { get; set; }

        public DateTime? CompletedDateTo { get; set; }

        public bool? HasDueDate { get; set; }

        public string Tag { get; set; }

        public int? ProjectId { get; set; }
    }
}
