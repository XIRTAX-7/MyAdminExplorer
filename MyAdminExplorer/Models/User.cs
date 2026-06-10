using System;
using System.Collections.Generic;

namespace MyAdminExplorer.Models
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public byte Role { get; set; }
        public bool Even { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int FromTime { get; set; }
        public int ToTime { get; set; }
        public List<string> ForbiddenList { get; set; } = new List<string>();
    }
}
