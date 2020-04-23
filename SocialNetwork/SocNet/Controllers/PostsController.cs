using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocNet.Models;
using SocNet.Services;

namespace SocNet.Controllers
{
    public class PostsController : Controller
    {
        private readonly PostService _postService;
        private readonly UserService _userService;
        //private readonly UserService _userService;
        public PostsController()
        {
            //_logger = logger;
            //_userService = userService;
            _postService = new PostService();
            _userService = new UserService();
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public IActionResult Index()
        {
            var user = _userService.GetByEmail(User.Identity.Name).Result;
            var posts = _postService.GetAllByProfile(user).ToList();
            return View(posts);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Like(string id)
        {
            var user = _userService.GetByEmail(User.Identity.Name).Result;
            _postService.Like(user.UserName, id);
            var posts = _postService.GetAllByProfile(user).ToList();
            return View("_Posts", posts);
            //return PartialView("_Posts", posts);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(string id, string content)
        {
            var user = _userService.GetByEmail(User.Identity.Name).Result;
            _postService.AddComment(id, content, user.UserName);
            var posts = _postService.GetAllByProfile(user).ToList();
            return View("_Posts", posts);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPost(string title, string content)
        {
            var user = _userService.GetByEmail(User.Identity.Name).Result;
            _postService.AddPost(content, title, user.UserName);
            var posts = _postService.GetAllByProfile(user).ToList();
            return View("_Posts", posts);
        }
    }
}