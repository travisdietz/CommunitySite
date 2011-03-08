using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommunitySite.Core.Data;
using CommunitySite.Core.Domain;
using CommunitySite.Core.Services.Authentication;
using CommunitySite.Web.UI.Models;

namespace CommunitySite.Web.UI.Controllers
{
    public class AccountController : Controller
    {
        readonly MemberRepository _memberRepository;
        readonly AuthenticationService _authenticationService;

        public AccountController(MemberRepository memberRepository, 
            AuthenticationService authenticationService)
        {
            _memberRepository = memberRepository;
            _authenticationService = authenticationService;
        }

        public ActionResult Register()
        {
            return View("Register", new RegistrationModel());
        }

        [HttpPost]
        public ActionResult Register(RegistrationModel registrationModel)
        {
            var member = new Member
                             {
                                 Username = registrationModel.Username,
                                 Password = registrationModel.Password,
                                 EmailAddress = registrationModel.EmailAddress
                             };
            _memberRepository.Save(member);
            _authenticationService.SignIn(member.Username);
            return RedirectToAction("Profile");
        }

        public ActionResult Profile()
        {
            return View();
        }
    }
}
