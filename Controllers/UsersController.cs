using AutoMapper;
using Azure.Identity;
using KartuliAPI1.Auth;
using KartuliAPI1.Auth.Model;
using KartuliAPI1.Data.Dtos.Recipes;
using KartuliAPI1.Data.Dtos.Users;
using KartuliAPI1.Data.Entities;
using KartuliAPI1.Data.Repositories;
using KartuliAPI1.Data.Repositories.KartuliAPI1.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace KartuliAPI1.Controllers

{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ForumRestUser> _userManager;
        private readonly JwtTokenService _jwtService;


        public UsersController(IUsersRepository usersRepository, IMapper mapper, JwtTokenService jwtService, UserManager<ForumRestUser> userManager)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _jwtService = jwtService;
            _userManager = userManager;
        }


        [HttpGet]

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            return (await _usersRepository.GetAll()).Select(o => _mapper.Map<UserDto>(o));

        }

        [HttpGet("{UserId}")]
        public async Task<ActionResult<UserDto>> Get(int UserId)
        {

            var user = await _usersRepository.Get(UserId);
            if (user == null) return NotFound($"User with ID '{UserId}' not found.");

            return Ok(_mapper.Map<UserDto>(user));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto)
        {
            var user = await _usersRepository.Authenticate(loginDto.Username, loginDto.Password);

            if (user == null)
                return Unauthorized();

            var token = _jwtService.CreateAccessToken(user.Username, new List<string> { user.Role });

            return Ok(new { Token = token, Role = user.Role });
        }

        public async Task<ActionResult<UserDto>> Post(CreateUsersDto usersDto)
        {
            var user = _mapper.Map<Users>(usersDto);

            if (user == null)
            {
                return BadRequest("Failed to create the user. Check your input and try again.");
            }

            var newUser = new ForumRestUser
            {
                Email = user.Email,
                UserName = user.Username
            };

            await _userManager.AddToRoleAsync(newUser, "SimpleUser");

            await _usersRepository.Create(user);

            var userDto = _mapper.Map<UserDto>(user);

            return Created($"api/users/{user.UserId}", userDto);
        }



        [HttpPost]
        public async Task<ActionResult<UserDto>> PostAnother(CreateUsersDto usersDto)
        {
            var user = _mapper.Map<Users>(usersDto);

            // Check if user is null after mapping
            if (user == null)
            {
                return BadRequest("Failed to create the user. Check your input and try again.");
            }

            await _usersRepository.Create(user);

            var newUser = new ForumRestUser
            {
                Email = user.Email,
                UserName = user.Username
            };

            await _userManager.AddToRoleAsync(newUser, "SimpleUser");

            return Created($"api/users/{user.UserId}", _mapper.Map<UserDto>(user));
        }



    [HttpPatch("{UserId}")]
    public async Task<ActionResult<UserDto>> Put(int UserId, UpdateUsersDto usersDto)
    {
        var user = await _usersRepository.Get(UserId);
        if (user == null) return NotFound($"User with ID '{UserId}' not found.");

        // Update user with data from usersDto
        _mapper.Map<UpdateUsersDto, Users>(usersDto, user);

        // Save the changes to the repository
        await _usersRepository.Patch(user);

        return Ok(_mapper.Map<UserDto>(user));
    }




    [HttpDelete("{UserId}")]

        public async Task<ActionResult<UserDto>> Delete(int UserId)
        {

            var user = await _usersRepository.Get(UserId);
            if (user == null) return NotFound($"User with ID '{UserId}' not found.");


            await _usersRepository.Delete(user);

            return NoContent();


        }


        [HttpGet("{userId}/recipes")]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetUserRecipes(int userId)
        {
            var recipes = await _usersRepository.GetUserRecipes(userId);
            return Ok(recipes.Select(r => _mapper.Map<RecipeDto>(r)));
        }




    }
}
