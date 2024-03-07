using Platform.Infrastructure.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleCQRS.UAM.Domain.Commands
{
    public class CreateUserCommand : Command
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? PreferedLanguage { get; set; }

        public string? Password { get; set; }

        public Guid RoleId { get; set; }
    }
}
