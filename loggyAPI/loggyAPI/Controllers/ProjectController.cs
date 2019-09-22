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
        private readonly IMapper _mapper;

        public ProjectController(IMapper mapper, IProjectService projectService)
        {
            _mapper = mapper;
            _projectService = projectService;
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
    }
}