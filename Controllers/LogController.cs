using AutoMapper;
using Demo2CoreAPICrud.Dto;
using Demo2CoreAPICrud.Interface;
using Demo2CoreAPICrud.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo2CoreAPICrud.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        private readonly ILogsRepo _logsRepo;
        private readonly IMapper _mapper;

        public LogController(ILogsRepo logsRepo, IMapper mapper)
        {
            _logsRepo = logsRepo;
            _mapper = mapper;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<object>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllLogs()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var logs = await _logsRepo.GetAll();

            if (logs == null)
                return NotFound();

            return Ok(logs);
        }

        [HttpGet("{logId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByLogId([FromRoute] int logId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var logMapped = await _logsRepo.GetById(logId);

            if (logMapped == null)
                return NotFound();

            return Ok(logMapped);
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<object>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId([FromRoute] string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var logs = await _logsRepo.GetByUserId(userId);

            if (logs == null)
                return NotFound();

            return Ok(logs);
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<object>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Search([FromQuery] LogsSearchDto logsSearchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var search = await _logsRepo.Search(logsSearchDto);

            if (search == null || !search.Any())
                return NotFound();

            return Ok(search);
        }

        [HttpGet("search/user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<object>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchUsers([FromQuery] UserSearchDto userSearchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var search = await _logsRepo.SearchByUser(userSearchDto);

            if (search == null || !search.Any())
                return NotFound();

            return Ok(search);
        }

        [HttpGet("search/location")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<object>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchLocations([FromQuery] LocationSearchDto locationSearchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var search = await _logsRepo.SearchByLocation(locationSearchDto);

            if (search == null || !search.Any())
                return NotFound();

            return Ok(search);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> NewLogEntry([FromBody] LogsDto logsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (logsDto == null)
                return BadRequest(ModelState);
            
            if (!_logsRepo.LocationUserExistById(logsDto.LocationNumber, logsDto.UserId).Result)
            {
                ModelState.AddModelError("", "Location id or user id is invalid.");
                return StatusCode(422, ModelState);
            }
            
            var logMap = _mapper.Map<Log>(logsDto);

            if (!await _logsRepo.CreateLog(logMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving log.");
                return StatusCode(500, ModelState);
            }
            return Ok("Log successfully created.");
        }
    }
}
