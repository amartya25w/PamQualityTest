using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArconConnector.BusinessEntities
{
    public class UserDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public int SessionId { get; set; }
        public int SessionTimeOutTime { get; set; }
        public bool IsSessionAlive { get; set; }
    }
}
