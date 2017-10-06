using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TasksManager.DataAccess.Projects.Commands;
using TasksManager.DataAccess.Projects.Exceptions;
using TasksManager.DataAccess.Projects.Queries;
using TasksManager.ViewModels;
using TasksManager.ViewModels.Filters;
using TasksManager.ViewModels.Requests;
using TasksManager.ViewModels.Responses;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ListResponse<ProjectResponse>))]
        public async Task<IActionResult> GetProjectsListAsync(ProjectFilter filter, ListOptions options, [FromServices]IProjectListQuery query)
        {
            ListResponse<ProjectResponse> response = await query.RunAsync(filter, options);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProjectResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostTaskAsync([FromBody]CreateProjectRequest request, [FromServices]ICreateProjectCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProjectResponse responce = await command.ExecuteAsync(request);
            return Created(new Uri(Url.Link("GetProject", new {projectId = responce.Id})), responce);
        }

        [HttpGet("{projectId}", Name = "GetProject")]
        [ProducesResponseType(200, Type = typeof(ProjectResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProjectAsync(int projectId, [FromServices]IProjectQuery query)
        {
            ProjectResponse response = await query.RunAsync(projectId);

            return response == null ? (IActionResult) NotFound() : Ok(response);
        }

        [HttpPut("{projectId}")]
        [ProducesResponseType(200, Type = typeof(ProjectResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateProjectAsync(int projectId, [FromBody]UpdateProjectRequest request, [FromServices]IUpdateProjectCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProjectResponse responce = await command.ExecuteAsync(projectId, request);
            return Ok(responce);
        }

        [HttpDelete("{projectId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteProjectAsync(int projectId, [FromServices]IDeleteProjectCommand command)
        {
            try
            {
                await command.ExecuteAsync(projectId);
                return NoContent();
            }
            catch (CannotDeleteProjectWithTasksException)
            {
                return BadRequest("Current project contains tasks and cannot be deleted");
            }
        }
    }
}
