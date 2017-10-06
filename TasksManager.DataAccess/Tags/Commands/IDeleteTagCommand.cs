using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.DataAccess.Tags.Commands
{
    public interface IDeleteTagCommand
    {
        Task ExecuteAsync(int tagId);
    }
}
