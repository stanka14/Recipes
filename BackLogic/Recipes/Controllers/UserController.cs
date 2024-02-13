using Data.Models;
using Dtos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.ErrorHandling;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Recipes.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _context;

        public UserController(IUserService userService, IConfiguration config, IHttpContextAccessor context)
        {
            _userService = userService;
            _config = config;
            _context = context;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync([FromBody] UserDto userDto)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                await _userService.AddUserAsync(userDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("updateuser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto userDto)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                await _userService.UpdateUserAsync(userDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto changePasswordDto)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Invalid data.");

                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                    return NotFound();

                if (!await _userService.VerifyPasswordAsync(user, changePasswordDto.CurrentPassword))
                    return BadRequest("Current password is incorrect");

                await _userService.UpdatePasswordAsync(user, changePasswordDto.NewPassword);
                return Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDetailsDto login)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                IActionResult response = Unauthorized();

                var user = await AuthenticateUserAsync(login);

                if (user == null)
                {
                    return Unauthorized("Invalid username or password!");
                }

                var tokenString = GenerateJSONWebToken(user.Id);
                return Ok(new { token = tokenString, id = user.Id, username = user.UserName, firstname = user.FirstName, lastname = user.LastName });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            try
            {
                var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Not logged in.");
                }

                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Not authorized.");
                }

                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getuser")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Not authorized.");
                }

                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("notifications")]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Not authorized.");
                }

                var notifications = await _userService.GetAllNotifications(userId);

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("makeasread")]
        public async Task<IActionResult> MakeAsRead(string notificationId)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                await _userService.MakeNotificationAsRead(notificationId);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("deletenotification")]
        public async Task<IActionResult> DeleteNotification(string notificationId)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                await _userService.DeleteNotification(notificationId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("makeallasread")]
        public async Task<IActionResult> MakeAllAsRead(string userId)
        {
            var errorResponse = ValidateModelState();

            if (!errorResponse.Success)
            {
                return BadRequest(errorResponse.Message);
            }

            try
            {
                await _userService.MakeAllNotificationAsRead(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<ApplicationUser> AuthenticateUserAsync(LoginDetailsDto login)
        {
            return await _userService.AuthenticateUserAsync(login);
        }

        private string GenerateJSONWebToken(string userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private ErrorResponseDto ValidateModelState()
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            if (!errors.Any())
                return new ErrorResponseDto() { Success = true };

            var errorMessage = string.Join(" ", errors);
            return new ErrorResponseDto { Message = errorMessage, Success = false };
        }
    }
}
