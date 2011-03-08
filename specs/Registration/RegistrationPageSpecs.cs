using System.Web.Mvc;
using System.Web.Routing;
using CommunitySite.Core.Data;
using CommunitySite.Core.Data.NHibernate;
using CommunitySite.Core.Domain;
using CommunitySite.Core.Services.Authentication;
using CommunitySite.Web.UI.Controllers;
using CommunitySite.Web.UI.Models;
using FakeItEasy;
using Machine.Specifications;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using Web.UI;

namespace CommunitySite.Specifications.Registration
{
    [Subject("01 - Membership")]
    public class When_a_user_wishes_to_register
    {
        Establish context = () => MvcApplication.RegisterRoutes(RouteTable.Routes);

        It should_navigate_to_the_registration_page = () =>
            "~/account/register".ShouldMapTo<AccountController>(ctrl => ctrl.Register());
    }

    [Subject("01 - Membership")]
    public class When_navigating_to_the_registration_page : 
        With_an_account_controller_context
    {
        Because of = () => _result = _ctrl.Register();

        It should_load_the_registration_form = () =>
            _result.AssertViewRendered().ForView("Register");

        It should_empty_the_registration_form = () =>
            _result.AssertViewRendered()
            .ViewData.Model.ShouldBe(typeof(RegistrationModel));

        static ActionResult _result;
    }

    [Subject("01 - Membership")]
    public class When_a_user_submits_valid_and_complete_registration_information
        : With_an_account_controller_context
    {
        Establish context = () =>
            {
                _registrationModel = new RegistrationModel{Username = "myusername"};
                _ctrl.ModelState.Clear();
            };

        Because of = () => _result = _ctrl.Register(_registrationModel);

        It should_create_a_member_for_that_user = () =>
            _memberRepository.AssertWasCalled(m => m.Save(Arg<Member>.Is.Anything));

        It should_sign_them_in_as_the_new_member = () =>
            _authenticationService.AssertWasCalled(svc => 
                svc.SignIn(_registrationModel.Username));

        It should_take_them_to_the_member_profile_page = () =>
            _result.AssertActionRedirect().ToAction("Profile");

        static RegistrationModel _registrationModel;
        static ActionResult _result;
        static Member _newMember;
        static Member _member;
    }

    public class When_saving_a_new_member
    {
        Establish context = () =>
            {
                _member = new Member();
                _repository = MockRepository.GenerateMock<Repository>();
                _memberRepository = new NHibernateMemberRepository(_repository);
            };

        Because of = () => _memberRepository.Save(_member);

        It should_save_the_member_information_to_storage =()=>
            _repository.AssertWasCalled(repo=>repo.Save(_member));

        static NHibernateMemberRepository _memberRepository;
        static Member _member;
        static Repository _repository;
    }

    public class With_an_account_controller_context
    {
        Establish context = () =>
        {
            _memberRepository = MockRepository.GenerateMock<MemberRepository>();
            _authenticationService = MockRepository.GenerateMock<AuthenticationService>();
            _ctrl = new AccountController(_memberRepository, _authenticationService);
        };

        protected static MemberRepository _memberRepository;
        protected static AuthenticationService _authenticationService;
        protected static AccountController _ctrl;
    }
}