using System.Linq;
using AutoMapper;
using loggyAPI.Data.Entities;
using loggyAPI.Dtos;
using loggyAPI.Services;
using loggyAPI.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace loggyAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public ProjectController(IMapper mapper, IProjectService projectService, ILogService logService)
        {
            _mapper = mapper;
            _projectService = projectService;
            _logService = logService;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody]ProjectDto projectDto)
        {
            // map dto to entity
            var project = _mapper.Map<Project>(projectDto);

            try
            {
                // save 
                _projectService.CreateProject(project);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("update")]
        public IActionResult Update([FromBody]ProjectDto projectDto)
        {
            // map dto to entity
            var project = _mapper.Map<Project>(projectDto);

            try
            {
                // save 
                _projectService.UpdateProject(project);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("delete")]
        public IActionResult Delete([FromBody]ProjectDto projectDto)
        {
            // map dto to entity
            var project = _mapper.Map<Project>(projectDto);

            try
            {
                // save 
                _projectService.Delete(project);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetUserProjects([FromBody]UserDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<User>(userDto);

            try
            {
                // save 
                var projects = _projectService.GetUserProjects(user).Select(x => _mapper.Map<ProjectDto>(x));
                return Ok(projects);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUserProject(int id)
        {
            // map dto to entity

            try
            {
                // save 
                var projects = _mapper.Map<ProjectDto>(_projectService.GetProject(id));
                return Ok(projects);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/logs")]
        public IActionResult GetUserProjectEntries(int id)
        {
            // map dto to entity

            try
            {
                // save 
                var projects = _projectService.GetProject(id);
                if(projects == null)
                {
                    return BadRequest(new { message = "Project not found." });
                }
                var result = _logService.GetProjectEntries(projects).Select( x=> new {
                    Description = x.Description,
                    From = x.From ,
                    To = x.To
                });
                return Ok(result);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}