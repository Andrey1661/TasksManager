using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TasksManager.DataAccess.Projects.Commands;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModels.Requests;
using TasksManager.ViewModels.Responses;

namespace TasksManager.DataAccess.DbImplementation.Projects.Commands
{
    public class CreateProjectCommand : ICreateProjectCommand
    {
        private readonly TasksManagerDbContext _context;
        private readonly IMapper _mapper;

        public CreateProjectCommand(TasksManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectResponse> ExecuteAsync(CreateProjectRequest request)
        {
            var project = _mapper.Map<CreateProjectRequest, Project>(request);

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return _mapper.Map<Project, ProjectResponse>(project);
        }
    }
}
