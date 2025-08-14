using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArconAPIUtility
{
    public class APIAuthToken
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public string TokenType { get; set; }

        public double ExpiresIn { get; set; }

        public DateTime CreatedOn { get; set; }

        public string APIUserName { get; set; }

        public string APIPassword { get; set; }
    }
}
