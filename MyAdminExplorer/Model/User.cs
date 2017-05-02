using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAdminExplorer.Model
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
        public List<string> ForbiddenList { get; set; }
    }
}
