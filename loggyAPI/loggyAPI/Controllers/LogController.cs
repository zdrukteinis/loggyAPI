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
    public class LogController : Controller
    {

        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public LogController(IMapper mapper, ILogService logService)
        {
            _mapper = mapper;
            _logService = logService;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody]LogEntryDto projectDto)
        {
            // map dto to entity
            var project = _mapper.Map<LogEntry>(projectDto);

            try
            {
                // save 
                _logService.CreateLogEntry(project);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("update")]
        public IActionResult Update([FromBody]LogEntryDto logEntryDto)
        {
            // map dto to entity
            var logEntry = _mapper.Map<LogEntry>(logEntryDto);

            try
            {
                // save 
                var updateLogEntry = _logService.UpdateLogEntry(logEntry);
                return Ok(_mapper.Map<LogEntryDto>(logEntryDto));
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("delete")]
        public IActionResult Delete([FromBody]LogEntryDto projectDto)
        {
            // map dto to entity
            var logEntry = _mapper.Map<LogEntry>(projectDto);

            try
            {
                // save
                _logService.DeleteLogEntry(logEntry);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("project-entries")]
        public IActionResult GetProjectLogEntries([FromBody]ProjectDto projectDto)
        {
            // map dto to entity
            var project = _mapper.Map<Project>(projectDto);

            try
            {
                // save
                _logService.GetProjectEntries(project);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}