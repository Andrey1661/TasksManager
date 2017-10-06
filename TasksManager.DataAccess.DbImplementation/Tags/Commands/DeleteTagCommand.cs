using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TasksManager.DataAccess.Tags.Commands;
using TasksManager.Db;
using TasksManager.Entities;
using Task = System.Threading.Tasks.Task;

namespace TasksManager.DataAccess.DbImplementation.Tags.Commands
{
    public class DeleteTagCommand : IDeleteTagCommand
    {
        private readonly TasksManagerDbContext _context;

        public DeleteTagCommand(TasksManagerDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(int tagId)
        {
            Tag tag = await _context.Tags.Include(t => t.TaskTags).FirstOrDefaultAsync(t => t.Id == tagId);

            if (tag == null) return;

            //Explicitly remove all taskTags related to current tag
            _context.TaskTag.RemoveRange(tag.TaskTags);
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}
