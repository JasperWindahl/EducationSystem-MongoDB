using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Authentication
{
    public class TokenDetails
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
    }
}
