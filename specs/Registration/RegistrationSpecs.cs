using System;
using System.Web.Mvc;
using System.Web.Routing;
using CommunitySite.Core.Data;
using CommunitySite.Core.Data.NHibernate;
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

    public class When_navigating_to_the_registration_page : With_a_member_registration_context
    {
        Because of = () => _results = _controller.Register();

        It should_load_the_registration_page = () =>
            _results.AssertViewRendered().ForView("Register");

        It should_load_an_empty_registration_form = () =>
            _results.AssertViewRendered().ViewData.Model.ShouldBe(typeof(RegistrationModel));

        static ActionResult _results;
    }

    public class When_submitting_valid_and_complete_registration_information 
        : With_a_member_registration_context
    {
        Establish context = () =>
        {
            _registrationModel = new RegistrationModel { Username = "username" };
            _controller.ModelState.Clear();
        };

        Because of = () => _results = _controller.Register(_registrationModel);

        It should_create_a_new_member = () =>
            A.CallTo(() => _memberRepository.Save(A<Member>.That.Matches(x=>x.Username == "username"))).MustHaveHappened();

        It should_log_the_new_member_in = () =>
            A.CallTo(() => _authenticationService.SignIn("username"));

        It should_take_the_user_to_the_member_profile_page = () =>
            _results.AssertActionRedirect().ToAction("Profile");
    }

    public class When_submitting_invalid_or_incomplete_registration_information
        : With_a_member_registration_context
    {
        Establish context = () =>
            {
                _registrationModel = new RegistrationModel {Username = "username"};
                _controller.ModelState.AddModelError("*", "Error");
            };

        Because of =()=> _results = _controller.Register(_registrationModel);

        It should_not_create_a_new_member = () =>
            A.CallTo(() => _memberRepository.Save(A<Member>.Ignored)).MustNotHaveHappened();

        It should_not_sign_the_new_member_in = () =>
            A.CallTo(() => _authenticationService.SignIn(A<String>.Ignored)).MustNotHaveHappened();

        It should_return_the_user_to_the_registration_page = () =>
            _results.AssertViewRendered().ForView("Register");

        It should_inform_the_user_about_the_problem_with_registration = () =>
            _controller.ModelState["*"].Errors[0].ShouldNotBeNull();

    }

    public class When_creating_a_new_member
    {
        Establish context = () =>
            {
                _member = new Member { Username = "username" };
                _repository = A.Fake<Repository>();
                _memberRepository = new NHibernateMemberRepository(_repository);
            };

        Because of = () => _memberRepository.Save(_member);

        It should_save_the_new_member_information = () =>
            A.CallTo(() => _repository.Save(_member)).MustHaveHappened();

        static NHibernateMemberRepository _memberRepository;
        static Member _member;
        static Repository _repository;
    }

    public class With_a_member_registration_context
    {
        Establish context = () =>
        {
            _memberRepository = A.Fake<MemberRepository>();
            _authenticationService = A.Fake<AuthenticationService>();
            _controller = new AccountController(_memberRepository, _authenticationService);
        };

        protected static AccountController _controller;
        protected static RegistrationModel _registrationModel;
        protected static MemberRepository _memberRepository;
        protected static AuthenticationService _authenticationService;
        protected static ActionResult _results;
    }
}