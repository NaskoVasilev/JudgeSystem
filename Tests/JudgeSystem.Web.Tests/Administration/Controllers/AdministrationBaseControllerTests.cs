using JudgeSystem.Common;
using JudgeSystem.Web.Areas.Administration.Controllers;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Administration.Controllers
{
    public class AdministrationBaseControllerTests
    {

        [Fact]
        public void ShouldBeAllowedOnlyForAdministratorsAndBeInAreaAdministration() =>
           MyController<AdministrationBaseController>
           .Instance()
           .ShouldHave()
           .Attributes(attributes => attributes
                .RestrictingForAuthorizedRequests(GlobalConstants.AdministratorRoleName)
                .SpecifyingArea(GlobalConstants.AdministrationArea));
    }
}
