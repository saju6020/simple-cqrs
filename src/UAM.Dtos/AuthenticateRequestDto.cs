using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCQRS.UAM.Dtos
{
    public class AuthenticateRequestDto
    {
        public string? UserName { get;set; }

        public string? Password { get; set; }
    }
}
