using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TasksManager.Entities
{
    public abstract class DomainObject
    {
        [Key]
        public int Id { get; set; }
    }
}
