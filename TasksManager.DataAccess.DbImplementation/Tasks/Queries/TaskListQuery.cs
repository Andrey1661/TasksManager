using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.DataAccess.DbImplementation.Utilities;
using TasksManager.DataAccess.Tasks.Queries;
using TasksManager.Db;
using TasksManager.ViewModels;
using TasksManager.ViewModels.Filters;
using TasksManager.ViewModels.Responses;
using Task = TasksManager.Entities.Task;

namespace TasksManager.DataAccess.DbImplementation.Tasks.Queries
{
    public class TaskListQuery : ITaskListQuery
    {
        private readonly TasksManagerDbContext _context;
        private readonly IMapper _mapper;

        public TaskListQuery(TasksManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListResponse<TaskResponse>> RunAsync(TaskFilter filter, ListOptions listOptions)
        {
            IQueryable<Task> query =
                _context.Tasks.Include(t => t.Project).Include(t => t.TaskTags).ThenInclude(t => t.Tag).AsQueryable();

            int totalCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(filter.Name)) query = query.Where(t => t.Name.StartsWith(filter.Name));
            if (!string.IsNullOrWhiteSpace(filter.Tag)) query = query.Where(t => t.TaskTags.Select(p => p.Tag.Name).Contains(filter.Tag));
            if (filter.ProjectId.HasValue) query = query.Where(t => t.ProjectId == filter.ProjectId.Value);
            if (filter.Status.HasValue) query = query.Where(t => t.Status.Equals(filter.Status.Value));
            if (filter.HasDueDate.HasValue) query = query.Where(t => t.DueDate.HasValue == filter.HasDueDate.Value);

            if (filter.CompletedDateFrom.HasValue)
                query = query.Where(
                    t => t.CompleteDate.HasValue && t.CompleteDate.Value.CompareTo(filter.CompletedDateFrom.Value) >= 0);
            if (filter.CompletedDateTo.HasValue)
                query = query.Where(
                    t => t.CompleteDate.HasValue && t.CompleteDate.Value.CompareTo(filter.CompletedDateTo.Value) <= 0);

            if (filter.DueDateFrom.HasValue)
                query = query.Where(
                    t => t.DueDate.HasValue && t.DueDate.Value.CompareTo(filter.DueDateFrom.Value) >= 0);
            if (filter.DueDateFrom.HasValue)
                query = query.Where(
                    t => t.DueDate.HasValue && t.DueDate.Value.CompareTo(filter.DueDateTo.Value) <= 0);

            if (filter.CreatedDateFrom.HasValue)
                query = query.Where(
                    t => t.CreateDate.CompareTo(filter.CreatedDateFrom.Value) >= 0);
            if (filter.CreatedDateFrom.HasValue)
                query = query.Where(
                    t => t.CreateDate.CompareTo(filter.CreatedDateTo.Value) <= 0);

            IQueryable<TaskResponse> responceQuery = query.Select(t => _mapper.Map<Task, TaskResponse>(t));

            if (!string.IsNullOrWhiteSpace(listOptions.Sort))
            {
                string[] sortList = listOptions.Sort.Replace(" ", "").Split(',');
                responceQuery = responceQuery.OrderBy(sortList);
            }

            responceQuery = responceQuery.Skip(listOptions.GetOffset());

            int? toTake = listOptions.Count.ToInt32();
            if (toTake.HasValue) responceQuery = responceQuery.Take(toTake.Value);

            List<TaskResponse> tasks = await responceQuery.ToListAsync();

            return new ListResponse<TaskResponse>
            {
                PageNumber = listOptions.Page ?? 0,
                PageSize = toTake ?? 0,
                Sorting = listOptions.Sort,
                TotalItemsCount = totalCount,
                Items = tasks
            };
        }
    }
}
