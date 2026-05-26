using System;
using System.Collections.Generic;
using System.Text;

namespace PocoClasses.Dto
{
   
        public class PasswordHistoryDto
        {
            public Guid Id { get; set; }

            public Guid UserId { get; set; }

            public string UserName { get; set; } = string.Empty;

            public string Email { get; set; } = string.Empty;

            public string PasswordHash { get; set; } = string.Empty;

            public DateTime CreatedAt { get; set; }
        }

   
}
