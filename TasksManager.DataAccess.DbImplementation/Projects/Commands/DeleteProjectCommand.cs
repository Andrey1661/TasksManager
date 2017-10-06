using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TasksManager.DataAccess.Projects.Commands;
using TasksManager.DataAccess.Projects.Exceptions;
using TasksManager.Db;
using TasksManager.Entities;
using Task = System.Threading.Tasks.Task;

namespace TasksManager.DataAccess.DbImplementation.Projects.Commands
{
    public class DeleteProjectCommand : IDeleteProjectCommand
    {
        private readonly TasksManagerDbContext _context;

        public DeleteProjectCommand(TasksManagerDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(int projectId)
        {
            Project project = await _context.Projects.Include(t => t.Tasks).FirstOrDefaultAsync(t => t.Id == projectId);

            if (project == null) return;
            if (project.Tasks.Count != 0)
                throw new CannotDeleteProjectWithTasksException();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}
