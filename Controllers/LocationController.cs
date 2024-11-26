using AutoMapper;
using Demo2CoreAPICrud.Dto;
using Demo2CoreAPICrud.Helpers;
using Demo2CoreAPICrud.Interface;
using Demo2CoreAPICrud.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo2CoreAPICrud.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        private readonly ILocationRepo _locationRepo;
        private readonly IMapper _mapper;

        public LocationController(ILocationRepo locationRepo, IMapper mapper)
        {
            _locationRepo = locationRepo;
            _mapper = mapper;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LocationDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var locations = _mapper.Map<IEnumerable<LocationDto>>(await _locationRepo.GetAllLocations());

            if (locations == null)
                return NotFound();

            return Ok(locations);
        }

        [HttpGet("{locationId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocationDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int locationId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var locationMap = _mapper.Map<LocationDto>(await _locationRepo.GetLocationById(locationId));

            if (locationMap == null)
                return NotFound();

            return Ok(locationMap);
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LocationDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBySearch([FromQuery] LocationSearchDto locationSearchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var search = await _locationRepo.Search(locationSearchDto);

            if (search == null || !search.Any())
                return NotFound();

            var searchResult = _mapper.Map<IEnumerable<LocationDto>>(search);

            return Ok(searchResult);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult NewLocation([FromBody] LocationDto locationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (locationDto == null)
                return BadRequest(ModelState);

            if (_locationRepo.LocationExistsByName(locationDto))
            {
                ModelState.AddModelError("", "Location already exists.");
                return StatusCode(422, ModelState);
            }

            if (CustomiseString.HasEmptyString(locationDto) == true)
            {
                ModelState.AddModelError("", "Blank spaces are not valid input.");
                return StatusCode(422, ModelState);
            }
            var locationMap = _mapper.Map<Location>(locationDto);

            if (!_locationRepo.CreateLocation(locationMap).Result)
            {
                ModelState.AddModelError("", "Something went wrong while saving location.");
                return StatusCode(500, ModelState);
            }
            return Ok("Location successfully created.");
        }

        [HttpPut("edit/{locationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EditLocation([FromBody] LocationDto locationDto, [FromRoute] int locationId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (locationDto == null)
                return BadRequest(ModelState);

            if (locationId != locationDto.AreaId)
            {
                ModelState.AddModelError("", "Incorrect id input.");
                return StatusCode(422, ModelState);
            }

            if (!_locationRepo.LocationExistsById(locationId))
                return NotFound();

            if (CustomiseString.HasEmptyString(locationDto) == true)
            {
                ModelState.AddModelError("", "Blank spaces are not valid input.");
                return StatusCode(422, ModelState);
            }

            var locationMap = _mapper.Map<Location>(locationDto);

            if (!_locationRepo.UpdateLocation(locationMap).Result)
            {
                ModelState.AddModelError("", "Something went wrong while updating location.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("delete/{locationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveLocation([FromRoute] int locationId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_locationRepo.LocationExistsById(locationId))
                return NotFound();

            var location = await _locationRepo.GetLocationById(locationId);

            if (!_locationRepo.DeleteLocation(location!).Result)
            {
                ModelState.AddModelError("", "Something went wrong deleting location.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
