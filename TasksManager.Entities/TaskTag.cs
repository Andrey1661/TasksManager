using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TasksManager.Entities
{
    public class TaskTag : DomainObject
    {
        [ForeignKey("Task")]
        public int TaskId { get; set; }

        [ForeignKey("Tag")]
        public int TagId { get; set; }

        public Task Task { get; set; }
        public Tag Tag { get; set; }
    }
}
