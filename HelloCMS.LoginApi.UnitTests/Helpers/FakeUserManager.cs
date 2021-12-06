using HelloCMS.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloCMS.Identity.UnitTests.Helpers
{
    public class FakeUserManager : UserManager<AppIdentityUser>
    {
        public FakeUserManager()
               : base(new Mock<IUserStore<AppIdentityUser>>().Object,
                     new Mock<IOptions<IdentityOptions>>().Object,
                     new Mock<IPasswordHasher<AppIdentityUser>>().Object,
                     new IUserValidator<AppIdentityUser>[0],
                     new IPasswordValidator<AppIdentityUser>[0],
                     new Mock<ILookupNormalizer>().Object,
                     new Mock<IdentityErrorDescriber>().Object,
                     new Mock<IServiceProvider>().Object,
                     new Mock<ILogger<UserManager<AppIdentityUser>>>().Object)
        {

        }
    }

}
