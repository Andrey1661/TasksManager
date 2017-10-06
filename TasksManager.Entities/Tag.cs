using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TasksManager.Entities
{
    public class Tag : DomainObject
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        public ICollection<TaskTag> TaskTags { get; set; }
    }
}
