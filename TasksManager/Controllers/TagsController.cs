using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TasksManager.DataAccess.Tags.Commands;
using TasksManager.DataAccess.Tags.Queries;
using TasksManager.ViewModels;
using TasksManager.ViewModels.Filters;
using TasksManager.ViewModels.Responses;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ListResponse<TagResponce>))]
        public async Task<IActionResult> GetTagListAsync(TagFilter filter, ListOptions options, [FromServices]ITagListQuery query)
        {
            ListResponse<TagResponce> response = await query.RunAsync(filter, options);
            return Ok(response);
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteTagAsync(int tagId, [FromServices]IDeleteTagCommand command)
        {
            await command.ExecuteAsync(tagId);
            return NoContent();
        }
    }
}
