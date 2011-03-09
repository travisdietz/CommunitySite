using System;
using System.Web.Mvc;
using System.Web.Routing;
using CommunitySite.Core.Data;
using CommunitySite.Core.Domain;
using CommunitySite.Core.Services.Authentication;
using CommunitySite.Web.UI;
using CommunitySite.Web.UI.Controllers;
using CommunitySite.Web.UI.Models;
using FakeItEasy;
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

    public class When_navigating_to_the_registration_page
    {
        Establish context = () =>
            {
                _memberRepository = A.Fake<MemberRepository>();
                _authenticationService = A.Fake<AuthenticationService>();
                _controller = new AccountController(_memberRepository, _authenticationService);
            };

        Because of = () => _results = _controller.Register();

        It should_load_the_registration_page = () =>
            _results.AssertViewRendered().ForView("Register");

        It should_load_an_empty_registration_form = () =>
            _results.AssertViewRendered().ViewData.Model.ShouldBe(typeof(RegistrationModel));

        static AccountController _controller;
        static ActionResult _results;
        static MemberRepository _memberRepository;
        static AuthenticationService _authenticationService;
    }

    public class When_submitting_valid_and_complete_registration_information
    {
        Establish context = () =>
            {
                _memberRepository = A.Fake<MemberRepository>();
                _authenticationService = A.Fake<AuthenticationService>();
                _registrationModel = new RegistrationModel { Username = "username" };
                _controller = new AccountController(_memberRepository, _authenticationService);
                _controller.ModelState.Clear();
            };

        Because of = () => _results = _controller.Register(_registrationModel);

        It should_create_a_new_member = () =>
            A.CallTo(() => _memberRepository.Save(A<Member>.That.Matches(x=>x.Username == "username"))).MustHaveHappened();

        It should_log_the_new_member_in = () =>
            A.CallTo(() => _authenticationService.SignIn("username"));

        It should_take_the_user_to_the_member_profile_page = () =>
            _results.AssertActionRedirect().ToAction("Profile");

        static AccountController _controller;
        static RegistrationModel _registrationModel;
        static ActionResult _results;
        static MemberRepository _memberRepository;
        static AuthenticationService _authenticationService;
    }
}