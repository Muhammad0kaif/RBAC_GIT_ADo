using System;
using System.Collections.Generic;
using System.Text;

namespace PocoClasses.Dto
{
    public class AuditLogDto
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public string Action { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        public string? IP { get; set; }
    }
}
