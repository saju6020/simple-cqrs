using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCQRS.UAM.Dtos
{
    public class AuthenticateResponse
    {        
        public string? AccessToken { get; set; }        
        public string? RefreshToken { get; set; }
    }
}
