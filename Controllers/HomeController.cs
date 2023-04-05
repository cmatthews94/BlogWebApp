using BlogWebApp.Database;
using BlogWebApp.Interfaces;
using BlogWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogPostDbActions _blogPostDbActions;
        private readonly IUserAccountActions _userAccountActions;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IBlogPostDbActions blogPostDbActions, IUserAccountActions userAccountActions)
        {
            _logger = logger;
            _blogPostDbActions = blogPostDbActions;
            _userAccountActions = userAccountActions;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult GetBlogPostDataFromView()
        {
            BlogPost newBlogPost = new BlogPost()
            {
                Title = Request.Form["title"],
                Author = Request.Form["author"],
                Content = Request.Form["content"],
                UploadDateTime = DateTime.Now,
            };
            _blogPostDbActions.AddNewBlogPostDataToDb(newBlogPost);
            return Redirect("Privacy");
        }
        [HttpPost]
        public ActionResult NewUserRegistration()
        {
            UserAccount newUserAccountInfo = new UserAccount()
            {
                Username = Request.Form["username"],
                Password = Request.Form["password"],
            };
            try
            {
                _userAccountActions.AddNewUserAccount(newUserAccountInfo);
                return Redirect("Privacy");
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return Redirect("Index");
            }
        }

        [HttpPost]
        public ActionResult Login()
        {
            UserAccount loginToTest = new UserAccount()
            {
                Username = Request.Form["Username"],
                Password = Request.Form["Password"],
            };
            try
            {
                _userAccountActions.GetUserAccountByUsername(loginToTest.Username);
                _userAccountActions.CheckIfPasswordCorrect(loginToTest.Username, loginToTest.Password);
                return Redirect("Privacy");
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return Redirect("Index");
            }
        }
    }
}