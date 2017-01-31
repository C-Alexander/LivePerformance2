using System.Linq;
using LivePerformance2.Enumerations;
using LivePerformance2.Exceptions;
using LivePerformance2.Models;
using LivePerformance2.Repositories;
using LivePerformance2.Services;
using LivePerformance2.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LivePerformance2.Controllers
{
    //not exactly secure, normally you'd use Identity and an Authorize attribute. But this is mostly a POC and frankly, worrying about security in this is out of scope

    //alex said, right before some kid on infralab figured out the requests and deleted every user with Charles
    public class UserController : Controller
    {
        private IUserRepository _repository;
        private ILoginService _loginService;

        /// <summary>
        /// Instantiates the Usercontroller with a repo of choice. Use memory repository for unit testing
        /// </summary>
        /// <param name="emailSender">The email service to use</param>
        /// <param name="databaseManager">The database manager to use</param>
        /// <param name="provider">The encryption provider to use</param>
//        public UserController(IUserRepository repo)
//        {
//            repository = repo;
//        }
//This SHOULD work but, Asp.net core offers dependency injection etc. I feel this may cause bugs!!
        public UserController(IDatabaseService databaseService, IUserRepository userRepository, ILoginService loginService)
        {
            _repository = userRepository;
            _loginService = loginService;
        }

        public IActionResult Index()
        {
            if (_loginService.IsLoggedIn(HttpContext.Session))
            {
                return View("../Home/Index");
            }
            return RedirectToAction("CreateUser");
        }

        [HttpPost]
        public IActionResult DeleteUser(int userId)
        {

            User user = new User();
            user.Id = userId;
            _repository.Delete(user);
            return View("../Home/Index");
        }

        [HttpPost]
        // ReSharper disable once UnusedMember.Local
        private IActionResult UpdateUser(User user)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(user);
            }
            return View("../Home/Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (_loginService.IsLoggedIn(HttpContext?.Session))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel user)
        {
            if (_loginService.IsLoggedIn(HttpContext?.Session))
            {
                return RedirectToAction("Index", "Home");
            }
            if (_repository.UserNameExists(user.Username))
            {
                ViewData.ModelState.TryAddModelError("Username", "There is already a user with this name in the system.");
            }
            if (ModelState.IsValid)
            {
                User newUser = new User()
                {
                    Password = user.Password,
                    Username = user.Username,
                    Email = user.Email
                };
                _repository.Add(newUser);
                _loginService.Login(HttpContext?.Session, newUser.Username, newUser.Password);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (_loginService.IsLoggedIn(HttpContext?.Session))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel user)
        {
            if (_loginService.IsLoggedIn(HttpContext?.Session))
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                if (!_loginService.Login(HttpContext?.Session, user.Username, user.Password))
                {
                    return View();
                }
            }
            catch (UsernameNotFoundException e)
            {
                Log.Information(e.ToString());
                ModelState.TryAddModelError("Username", "This user does not exist");
            }
            catch (WrongPasswordException e)
            {
                Log.Information(e.ToString());
                ModelState.TryAddModelError("Password", "This user does not exist");
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Logout()
        {
            if (_loginService.IsLoggedIn(HttpContext?.Session))
            {
                _loginService.Logout(HttpContext?.Session);
            }
            return RedirectToAction("Login");
        }
    }
}
