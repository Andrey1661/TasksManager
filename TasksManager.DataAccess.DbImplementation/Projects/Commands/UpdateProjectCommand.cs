using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.DataAccess.Projects.Commands;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModels.Requests;
using TasksManager.ViewModels.Responses;

namespace TasksManager.DataAccess.DbImplementation.Projects.Commands
{
    public class UpdateProjectCommand : IUpdateProjectCommand
    {
        private readonly TasksManagerDbContext _context;
        private readonly IMapper _mapper;

        public UpdateProjectCommand(TasksManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectResponse> ExecuteAsync(int projectId, UpdateProjectRequest request)
        {
            Project project = await _context.Projects.FindAsync(projectId);

            if (project == null) return null;

            project.Name = request.Name;
            project.Description = request.Description;

            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<Project, ProjectResponse>(project);
        }
    }
}
