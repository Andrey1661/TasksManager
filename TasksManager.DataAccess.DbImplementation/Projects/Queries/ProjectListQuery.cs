using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.DataAccess.DbImplementation.Utilities;
using TasksManager.DataAccess.Projects.Queries;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModels;
using TasksManager.ViewModels.Filters;
using TasksManager.ViewModels.Responses;

namespace TasksManager.DataAccess.DbImplementation.Projects.Queries
{
    public class ProjectListQuery : IProjectListQuery
    {
        private readonly TasksManagerDbContext _context;
        private readonly IMapper _mapper;

        public ProjectListQuery(TasksManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListResponse<ProjectResponse>> RunAsync(ProjectFilter filter, ListOptions listOptions)
        {
            IQueryable<Project> query = _context.Projects.Include(t => t.Tasks).AsQueryable();

            int totalCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(filter.Name)) query = query.Where(t => t.Name.StartsWith(filter.Name));
            if (filter.OpenTasksCountFrom.HasValue) query = query.Where(t => t.Tasks.Count >= filter.OpenTasksCountFrom.Value);
            if (filter.OpenTasksCountTo.HasValue) query = query.Where(t => t.Tasks.Count <= filter.OpenTasksCountTo.Value);

            IQueryable<ProjectResponse> responceQuery = query.Select(t => _mapper.Map<Project, ProjectResponse>(t));

            if (!string.IsNullOrWhiteSpace(listOptions.Sort))
            {
                string[] sortList = listOptions.Sort.Replace(" ", "").Split(',');
                responceQuery = responceQuery.OrderBy(sortList);
            }

            responceQuery = responceQuery.Skip(listOptions.GetOffset());

            int? toTake = listOptions.Count.ToInt32();
            if (toTake.HasValue) responceQuery = responceQuery.Take(toTake.Value);

            List<ProjectResponse> projects = await responceQuery.ToListAsync();

            return new ListResponse<ProjectResponse>
            {
                PageNumber = listOptions.Page ?? 0,
                PageSize = toTake ?? projects.Count,
                Sorting = listOptions.Sort,
                TotalItemsCount = totalCount,
                Items = projects
            };
        }
    }
}
