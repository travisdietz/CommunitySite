using System.Web.Routing;
using CommunitySite.Web.UI;
using CommunitySite.Web.UI.Controllers;
using Machine.Specifications;
using MvcContrib.TestHelper;

namespace CommunitySite.Specifications.Registration
{
    public class When_a_user_wishes_to_register_for_membership
    {
        Establish context = () => MvcApplication.RegisterRoutes(RouteTable.Routes);

        It should_navigate_to_the_registration_page = () =>
            "~/account/register".ShouldMapTo<AccountController>(ctrl => ctrl.Register());
    }
}