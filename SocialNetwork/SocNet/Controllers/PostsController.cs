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
            var user = _userService.GetByEmail(User.Identity.Name);
            var posts = new List<PostViewModel>();
            foreach (var post in _postService.GetAll().Result)
            {
                if (user.Result.Followers.Contains(post.Author))
                    posts.Add(post);
            }
            return View(posts);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Like(string id)
        {
            var user = _userService.GetByEmail(User.Identity.Name);
            _postService.Like(user.Result.UserName, id);
            var posts = await _postService.GetAll();
            return View("_Posts", posts);
            //return PartialView("_Posts", posts);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(string id, string content)
        {
            var user = _userService.GetByEmail(User.Identity.Name);
            _postService.AddComment(id, content, user.Result.UserName);
            var posts = await _postService.GetAll();
            return View("_Posts", posts);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPost(string title, string content)
        {
            _postService.AddPost(content, title, User.Identity.Name);
            var posts = await _postService.GetAllByProfileAsync(User.Identity.Name);
            return View("_Posts", posts);
        }
    }
}