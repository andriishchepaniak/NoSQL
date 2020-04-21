using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SocNet.Models;
using SocNet.Services;

namespace SocNet.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        //private readonly UserService _userService;
        public UsersController()
        {
            //_logger = logger;
            //_userService = userService;
            _userService = new UserService();
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public async Task<IActionResult> IndexAsync()
        {
            //var user = await _userService.GetByEmail(User.Identity.Name);
            var posts = await _userService.GetAll();
            //var model = new IndexViewModel { Posts = posts };
            return View(posts.ToList());
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetFoll()
        {

            var me = _userService.GetByEmail(User.Identity.Name).Result;

            var users = new List<UserViewModel>();
            foreach (var user in _userService.GetAll().Result)
            {

                if (me.Followers.Contains(user.UserName))
                {
                    users.Add(user);
                }

            }
            //return View(friendsPosts);
            //var data = await _userService.GetFoll(me.Result.Followers);
            return View("Index", users);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Follow(string id)
        {
            
            var me = _userService.GetByEmail(User.Identity.Name).Result;
            await _userService.Follow(me.Email, id);
            var users = new List<UserViewModel>();
            foreach(var user in _userService.GetAll().Result)
            {

                if (me.Followers.Contains(user.UserName))
                {
                    users.Add(user);
                }
                
            }
            //return View(friendsPosts);
            //var data = await _userService.GetFoll(me.Result.Followers);
            return View("Index", users);
        }

        //[Authorize]
        //[HttpGet]
        //public async Task<IActionResult> Unfollow(string user)
        //{
        //    await _userService.UnfollowAsync(User.Identity.Name, user);

        //    var data = await _userService.GetByUserNameAsync(user);
        //    return PartialView("_ProfileData", data);
        //}
    }
}