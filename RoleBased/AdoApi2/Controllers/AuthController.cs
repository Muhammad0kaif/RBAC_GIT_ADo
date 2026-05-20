using Microsoft.AspNetCore.Mvc;
using AdoApi2.Services;
using PocoClasses.Dto;

namespace AdoApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService authService) : ControllerBase
    {

        #region LOGIN

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await authService.Login(dto);

            if (result == null)
                return Unauthorized("Invalid credentials");

            return Ok(result);
        }

        #endregion

        #region REGISTER

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await authService.Register(dto);

            if (!result)
                return BadRequest("User already exists");

            return Ok("User Registered Successfully");
        }

        #endregion

        #region REFRESH TOKEN

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var result = await authService.RefreshToken(refreshToken);

            if (result == null)
                return Unauthorized("Invalid or expired refresh token");

            return Ok(result);
        }

        #endregion

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await authService.GetRoles();
            return Ok(roles);
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(UserDto dto)
        {
            var result = await authService.CreateUser(dto);

            if (!result)
                return BadRequest("User not created");

            return Ok("User created successfully");
        }
    }
}