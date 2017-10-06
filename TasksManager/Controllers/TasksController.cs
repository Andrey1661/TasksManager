using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TasksManager.DataAccess.Tasks.Commands;
using TasksManager.DataAccess.Tasks.Exceptions;
using TasksManager.DataAccess.Tasks.Queries;
using TasksManager.ViewModels;
using TasksManager.ViewModels.Filters;
using TasksManager.ViewModels.Requests;
using TasksManager.ViewModels.Responses;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ListResponse<TaskResponse>))]
        public async Task<IActionResult> GetTasksListAsync(TaskFilter filter, ListOptions options, [FromServices]ITaskListQuery query)
        {
            ListResponse<TaskResponse> responce = await query.RunAsync(filter, options);
            return Ok(responce);
        }

        [HttpGet("{taskId}", Name = "GetTask")]
        [ProducesResponseType(200, Type = typeof(TaskResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTaskAsync(int taskId, [FromServices]ITaskQuery query)
        {
            TaskResponse responce = await query.RunAsync(taskId);
            return responce == null ? (IActionResult) NotFound() : Ok(responce);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TaskResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostTaskAsync([FromBody] CreateTaskRequest request, [FromServices]ICreateTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TaskResponse responce = await command.ExecuteAsync(request);
            return Created(new Uri(Url.Link("GetTask", new { taskId = responce.Id })), responce);
        }

        [HttpPut("{taskId}")]
        [ProducesResponseType(200, Type = typeof(TaskResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateTaskAsync(int taskId, [FromBody]UpdateTaskRequest request, [FromServices]IUpdateTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TaskResponse responce = await command.ExecuteAsync(taskId, request);
            return Ok(responce);
        }

        [HttpDelete("{taskId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteTaskAsync(int taskId, [FromServices]IDeleteTaskCommand command)
        {
            await command.ExecuteAsync(taskId);
            return NoContent();
        }

        [HttpPost("{taskId}/tags/{tag}")]
        [ProducesResponseType(200, Type = typeof(TaskResponse))]
        [ProducesResponseType(404)]
        public async Task<ActionResult> AddTagToTask(int taskId, string tag, [FromServices]IAddTagToTaskCommand command)
        {
            try
            {
                TaskResponse responce = await command.ExecuteAsync(taskId, tag);
                return Ok(responce);
            }
            catch (TaskNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{taskId}/tags/{tag}")]
        [ProducesResponseType(200, Type = typeof(TaskResponse))]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteTagFromTask(int taskId, string tag, [FromServices]IDeleteTagFromTaskCommand command)
        {
            try
            {
                TaskResponse responce = await command.ExecuteAsync(taskId, tag);
                return Ok(responce);
            }
            catch (TaskNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
