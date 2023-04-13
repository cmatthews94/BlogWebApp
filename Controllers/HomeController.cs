using BlogWebApp.Database;
using BlogWebApp.Interfaces;
using BlogWebApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Http.Abstractions;

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

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult PostCreationPage()
        {
            try
            {
                if(!HttpContext.Session.GetInt32("UserId").HasValue)
                {
                    throw new Exception("Not logged in, please login to continue :)");
                }
                var currentUser = _userAccountActions.GetUserById(HttpContext.Session.GetInt32("UserId").Value);

                TempData["CurrentUsername"] = currentUser.Username;
                TempData["CurrentUserId"] = currentUser.UserId;

                return View(currentUser);

            }
            catch(Exception e)
            {
                TempData["LoginFailError"] = e.Message;
                return RedirectToAction("Login");
            }
        }
        public IActionResult ExistingPostsPage()
        {
            try
            {
                if (!HttpContext.Session.GetInt32("UserId").HasValue)
                {
                    throw new Exception("Not logged in, please login to continue :)");
                }
                var currentUser = _userAccountActions.GetUserById(HttpContext.Session.GetInt32("UserId").Value);
                List<BlogPost> listOfUserPosts = _blogPostDbActions.GetAllBlogPostsForUsername(_userAccountActions.GetUserById(HttpContext.Session.GetInt32("UserId").Value).Username);
                ViewBag.BlogPostList = listOfUserPosts;
                return View();
            }
            catch(Exception e)
            {
                TempData["LoginFailError"] = e.Message;
                return RedirectToAction("Login");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult CreateNewBlogPost(string title, string content)
        {
            BlogPost newBlogPost = new BlogPost()
            {
                Title = title,
                Author = _userAccountActions.GetUserById(HttpContext.Session.GetInt32("UserId").Value).Username,
                Content = content,
                UploadDateTime = DateTime.Now,
            };
            try
            {
                _blogPostDbActions.AddNewBlogPostDataToDb(newBlogPost);
                return Redirect("PostCreationPage");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                TempData["PostCreationFailError"] = e.Message;
                return Redirect("PostCreationPage");
            }
        }

        [HttpPost]
        public ActionResult NewUserRegistration(string username, string password)
        {
            UserAccount newUserAccountInfo = new UserAccount()
            {
                Username = username,
                Password = password,
            };
            try
            {
                _userAccountActions.AddNewUserAccount(newUserAccountInfo);
                HttpContext.Session.SetInt32("UserId", _userAccountActions.GetUserIdFromUsername(username));

                return Redirect("PostCreationPage");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                TempData["RegistrationFailError"] = e.Message;
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            try
            {
                _userAccountActions.CheckIfPasswordCorrect(username, password);

                HttpContext.Session.SetInt32("UserId", _userAccountActions.GetUserIdFromUsername(username));
                TempData["CurrentUser"] = _userAccountActions.GetUserById(HttpContext.Session.GetInt32("UserId").Value).Username;

                return RedirectToAction("PostCreationPage");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                TempData["LoginFailError"] = e.Message;
                return View("Login");

            }
        }
        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("Login");
        }

        [HttpGet]
        public ActionResult FindNullClick()
        {
            List<int> nullPostIds = _blogPostDbActions.GetListOfNullPosts();
            foreach(int i in nullPostIds)
            {
                Debug.WriteLine(i);
            }
            return Redirect("Login");
        }
        [HttpGet]
        public ActionResult DeleteNullClick()
        {
            List<int> nullPostIds = _blogPostDbActions.GetListOfNullPosts();
            foreach (int i in nullPostIds)
            {
                Debug.WriteLine($"deleted post with id {i}");
                _blogPostDbActions.DeleteBlogPostById(i);
            }
            return Redirect("Login");
        }
        [HttpPost]
        public ActionResult RemoveUserByName(string username)
        {
            HttpContext.Session.Clear();
            Debug.WriteLine($"removing account with username: {username}");
            _userAccountActions.DeleteUserByUsername(username);
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult DeletePostById(int postIdToRemove)
        {
            var currentUser = _userAccountActions.GetUserById(HttpContext.Session.GetInt32("UserId").Value);
            var foundPostAuthor = _blogPostDbActions.GetAuthorFromPostId(postIdToRemove);

            if(currentUser.Username == foundPostAuthor)
            {
                _blogPostDbActions.DeleteBlogPostById(postIdToRemove);

            }
            else
            {
                TempData["DeletePostError"] = "that post id is not associated with that username";
            }

            TempData["CurrentUsername"] = currentUser.Username;
            TempData["CurrentUserId"] = currentUser.UserId;

            return RedirectToAction("ExistingPostsPage");
        }
    }
}