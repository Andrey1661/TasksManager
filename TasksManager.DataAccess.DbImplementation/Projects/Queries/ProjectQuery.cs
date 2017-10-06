using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.DataAccess.Projects.Queries;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModels.Responses;

namespace TasksManager.DataAccess.DbImplementation.Projects.Queries
{
    public class ProjectQuery : IProjectQuery
    {
        private readonly TasksManagerDbContext _context;
        private readonly IMapper _mapper;

        public ProjectQuery(TasksManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectResponse> RunAsync(int projectId)
        {
            ProjectResponse responce =
                await _context.Projects.Select(t => _mapper.Map<Project, ProjectResponse>(t))
                    .FirstOrDefaultAsync(t => t.Id == projectId);

            return responce;
        }
    }
}
