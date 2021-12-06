using HelloCMS.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloCMS.Identity.UnitTests.Helpers
{
    public class FakeRoleManager
        : RoleManager<AppIdentityRole>
    {
        public FakeRoleManager()
            : base(new Mock<IRoleStore<AppIdentityRole>>().Object,
                  new Mock<IEnumerable<IRoleValidator<AppIdentityRole>>>().Object,
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<ILogger<RoleManager<AppIdentityRole>>>().Object)
        {

        }
    }
}
