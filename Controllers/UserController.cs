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
    public class UserController : Controller
    {
        private readonly IUsersRepo _usersRepo;
        private readonly IMapper _mapper;

        public UserController(IUsersRepo usersRepo, IMapper mapper)
        {
            _usersRepo = usersRepo;
            _mapper = mapper;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {   
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var users = _mapper.Map<IEnumerable<UserDto>>(await _usersRepo.GetAllUsers());

            if (users == null) 
                return NotFound();
            
            return Ok(users);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById([FromRoute] string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var userMapped = _mapper.Map<UserDto>(_usersRepo.GetUserById(userId));
            
            if (userMapped == null) 
                return NotFound();

            return Ok(userMapped);
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBySearch([FromQuery] UserSearchDto userSearchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var search = await _usersRepo.Search(userSearchDto);
            
            if (search == null || !search.Any())
                return NotFound();            

            var searchResult = _mapper.Map<IEnumerable<UserDto>>(search);

            return Ok(searchResult);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (userDto == null) 
                return BadRequest(ModelState);
            
            if(_usersRepo.UserExistsByName(userDto.Forename, userDto.Surname))
            {
                ModelState.AddModelError("", "User already exists.");
                return StatusCode(422, ModelState);
            }            

            if (CustomiseString.HasEmptyString(userDto) == true)
            {
                ModelState.AddModelError("", "Blank spaces for names are not valid input.");
                return StatusCode(422, ModelState);
            }            
            var userMap = _mapper.Map<User>(userDto);

            if (! _usersRepo.CreateUser(userMap).Result)
            {
                ModelState.AddModelError("", "Something went wrong while saving user.");
                return StatusCode(500, ModelState);
            }
            return Ok("User successfully created.");
        }

        [HttpPut("edit/{userId}")]        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public IActionResult UpdateUser([FromBody] UserDto userDto, [FromRoute] string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (userDto == null) 
                return BadRequest(ModelState);

            if (Guid.Parse(userId) != userDto.UserId)
            {
                ModelState.AddModelError("", "Incorrect id input.");
                return StatusCode(422, ModelState);
            }
           
            if (!_usersRepo.UserExistsById(userDto.UserId)) 
                return NotFound();

            if (CustomiseString.HasEmptyString(userDto) == true)
            {
                ModelState.AddModelError("", "Blank spaces are not valid input.");
                return StatusCode(422, ModelState);
            }

            var userMap = _mapper.Map<User>(userDto);

            if (!_usersRepo.UpdateUser(userMap).Result)
            {
                ModelState.AddModelError("", "Something went wrong while updating user.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("delete/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUser([FromRoute] string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_usersRepo.UserExistsById(new Guid(userId))) 
                return NotFound();

            var user = _usersRepo.GetUserById(userId);
                        
            if (!_usersRepo.DeleteUser(user!).Result)
            {
                ModelState.AddModelError("", "Something went wrong deleting user.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
