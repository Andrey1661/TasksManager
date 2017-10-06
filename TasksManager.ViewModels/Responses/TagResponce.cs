using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.ViewModels.Responses
{
    public class TagResponce
    {
        public string Name { get; set; }

        public int TaskCount { get; set; }

        public int OpenTaskCount { get; set; }
    }
}
