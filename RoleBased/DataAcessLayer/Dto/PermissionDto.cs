using System;
using System.Collections.Generic;
using System.Text;

namespace PocoClasses.Dto
{
    public class PermissionDto
    {
        public string PageName { get; set; } = string.Empty;

        public bool CanRead { get; set; }

        public bool CanWrite { get; set; }
    }
}
