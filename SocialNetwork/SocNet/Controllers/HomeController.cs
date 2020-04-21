using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocNet.Models;
using SocNet.Services;

namespace SocNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PostService _postService;
        //private readonly UserService _userService;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //_userService = userService;
            _postService = new PostService();
        }
        //public async Task<IActionResult> IndexAsync()
        //{
        //    var posts = await db.GetAll();
        //    return View(posts);
        //}
        [HttpGet]
        [Route("/")]
        [Route("/News")]
        public IActionResult Index()
        {
            //var user = await _userService.GetByEmail(User.Identity.Name);
            //var posts = await _postService.GetAll();
            //var model = new IndexViewModel { Posts = posts };
            return View(); //return View(posts);
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
    }
}
