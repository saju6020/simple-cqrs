using Platform.Infrastructure.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCQRS.UAM.Domain.Commands
{
    public class CreateTokenCommand : Command
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
