namespace MVCview.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public int RoleId { get; set; }
        public int FailedLoginAttempts { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedAt { get; set; }
        public Guid? DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }
}
