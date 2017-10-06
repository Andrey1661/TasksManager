using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.DataAccess.Tags.Queries;
using TasksManager.DataAccess.DbImplementation.Utilities;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModels;
using TasksManager.ViewModels.Filters;
using TasksManager.ViewModels.Responses;

namespace TasksManager.DataAccess.DbImplementation.Tags.Queries
{
    public class TagListQuery : ITagListQuery
    {
        private readonly TasksManagerDbContext _context;
        private readonly IMapper _mapper;

        public TagListQuery(TasksManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListResponse<TagResponce>> RunAsync(TagFilter filter, ListOptions listOptions)
        {
            IQueryable<Tag> query = _context.Tags.Include(t => t.TaskTags).ThenInclude(t => t.Task).AsQueryable();

            int totalCount = await _context.Tags.CountAsync();

            if (!string.IsNullOrWhiteSpace(filter.Name)) query = query.Where(t => t.Name.StartsWith(filter.Name));

            IQueryable<TagResponce> responceQuery = query.Select(t => _mapper.Map<Tag, TagResponce>(t));

            if (!string.IsNullOrWhiteSpace(listOptions.Sort))
            {
                string[] sortList = listOptions.Sort.Replace(" ", "").Split(',');
                responceQuery = responceQuery.OrderBy(sortList);
            }

            responceQuery = responceQuery.Skip(listOptions.GetOffset());

            int? toTake = listOptions.Count.ToInt32();
            if (toTake.HasValue) responceQuery = responceQuery.Take(toTake.Value);

            List<TagResponce> tasks = await responceQuery.ToListAsync();

            return new ListResponse<TagResponce>
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
