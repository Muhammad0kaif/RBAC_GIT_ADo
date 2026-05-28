using System;
using System.Collections.Generic;
using System.Text;

namespace PocoClasses
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string? ProfilePicture { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public bool MustChangePassword { get; set; }
        public int FailedLoginAttempts { get; set; }

        public bool IsLocked { get; set; }

        public DateTime? LockedAt { get; set; }
        public Guid? DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public Department? Department { get; set; }
    }
} 
