using AcerPro.Business.Repository;
using AcerPro.Dto;
using AcerPro.Dto.Models;
using AcerPro.Dto.ViewModels;
using AcerPro.Extensions;
using AcerPro.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AcerPro.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository<User, UserViewModel> _userRepository;
        private readonly IRepository<Error, ErrorViewModel> _errorRepository;

        public AccountController(
            IRepository<User, UserViewModel> userRepository,
            IRepository<Error, ErrorViewModel> errorRepository)
        {
            _userRepository = userRepository;
            _errorRepository = errorRepository;
        }

        public ActionResult LoginView()
        {
            return View();
        }

        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var encryptedPassword = model.Password.ToMd5();
                    var user = _userRepository.Take(q => q.Email == model.Email && q.Password == encryptedPassword);
                    if (user == null)
                    {
                        ModelState.AddModelError("UserNotFound", $"User not found registered with email {model.Email}");
                        return View(nameof(LoginView), model);
                    }

                    var currentSession = new UserViewModel { Name = user.Name, Email = user.Email, Id = user.Id };
                    Session.Add(Constants.SessionKey, currentSession);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UnexpectedError", "An unexpected error occurred");
                _errorRepository.Save(new ErrorViewModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace });
            }

            return View(nameof(LoginView), model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult RegisterView()
        {
            return View();
        }

        public ActionResult Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isUserExists = _userRepository.Table.Any(q => q.Email == model.Email);
                    if (isUserExists)
                    {
                        ModelState.AddModelError("UserAlreadyExists", $"A registered user already exists by {model.Email}");
                        return View(nameof(RegisterView), model);
                    }

                    var userViewModel = new UserViewModel
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password.ToMd5(),
                    };

                    var currentSession = new UserViewModel { Name = model.Name, Email = model.Email };
                    Session.Add(Constants.SessionKey, currentSession);

                    _userRepository.Save(userViewModel);

                    currentSession.Id = userViewModel.Id;

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UnexpectedError", "An unexpected error occurred");
                _errorRepository.Save(new ErrorViewModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace });
            }

            return View(nameof(RegisterView), model);
        }
    }
}