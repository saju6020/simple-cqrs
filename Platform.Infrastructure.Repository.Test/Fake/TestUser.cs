using Platform.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Infrastructure.Repository.Test.Fake
{
    internal class TestUser : BaseEntity
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Designation { set; get; }
    }
}
