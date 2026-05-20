using AdoApi2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocoClasses;
using PocoClasses.Dto;
using System.Security.Claims;

namespace AdoApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserService service) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await service.GetUsers());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await service.GetUserById(id);
            return user == null ? NotFound() : Ok(user);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized();

            var user = await service.GetUserById(userId);

            return user == null ? NotFound() : Ok(user);
        }

        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(User user)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized();

            user.Id = userId;

            await service.UpdateUser(user);

            return Ok("Profile Updated");
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized();

            var result = await service.ChangePassword(userId, dto);

            if (!result)
                return BadRequest("Password change failed");

            return Ok("Password changed successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(User user)
        {
            await service.CreateUserByAdmin(user);

            return Ok("User created and email sent successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, User user)
        {
            user.Id = id;
            await service.UpdateUser(user);
            return Ok("User Updated");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await service.DeleteUser(id);
            return Ok("User Deleted successfully");
        }

        [Authorize]
        [HttpPost("{id}/upload")]
        public async Task<IActionResult> UploadProfilePicture(Guid id, IFormFile file)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userIdClaim, out Guid loggedInUserId))
                return Unauthorized();

            bool isAdmin = roleClaim == "Admin";
            bool isOwnUser = loggedInUserId == id;

            if (!isAdmin && !isOwnUser)
                return Forbid();

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            if (file.Length > 5 * 1024 * 1024)
                return BadRequest("File size must be less than 5MB");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Only JPG and PNG files are allowed");

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "profile-pictures"
            );

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{id}{extension}";

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var dbPath = $"/uploads/profile-pictures/{fileName}";

            await service.UpdateProfilePicture(id, dbPath);

            return Ok(new
            {
                message = "Profile picture uploaded successfully",
                profilePicture = dbPath
            });
        }
    }
}