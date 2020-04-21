using AutoMapper;
using DAL.Models;
using DAL.Repositories;
using MongoDB.Bson;
using SocNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SocNet.Services
{
    public class UserService
    {
        private UserRepository userRepository;

        public UserService()
        {
            userRepository = new UserRepository();
        }
        public async Task<bool> Login(string email, string password)
        {
            string hashedPass = Hash(password);
            bool result = false;
            foreach (var user in userRepository.GetAll().Result)
            {
                if ((email == user.Email) && (Hash(password) == user.Password))
                {
                    result = true;
                    return result;
                }
            }
            return result;
        }
        public string Hash(string password)
        {
            var alg = SHA256.Create();
            byte[] bytes = alg.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();

            foreach (var b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
        public async Task Create(UserViewModel user)
        {
            var usr = new User();
            usr.FirstName = user.FirstName;
            usr.LastName = user.LastName;
            usr.Email = user.Email;
            usr.UserName = user.UserName;
            usr.Password = user.Password;
            usr.Followers = user.Followers;
            await userRepository.Create(usr);
        }

        public async Task<IEnumerable<UserViewModel>> GetAll()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserViewModel>()).CreateMapper();
            var result = mapper.Map<IEnumerable<UserViewModel>>(await userRepository.GetAll());
            return result;
        }

        public async Task<UserViewModel> GetByEmail(string email)
        {
            var user = await userRepository.GetByEmail(email);
            UserViewModel userresult = new UserViewModel();
            userresult.Id = user.Id;
            userresult.FirstName = user.FirstName;
            userresult.LastName = user.LastName;
            userresult.Email = user.UserName;
            userresult.UserName = user.UserName;
            userresult.Password = user.Password;
            userresult.Followers = user.Followers;
            return userresult;
        }
        public async Task<bool> IsExist(string email)
        {
            bool result = false;
            foreach(var item in GetAll().Result)
            {
                if (item.Email == email)
                {
                    result = true;
                    return result;
                }
                    
            }
            return result;
        }

        public async Task Update(UserViewModel user)
        {
            var usr = new User();
            usr.FirstName = user.FirstName;
            usr.LastName = user.LastName;
            usr.Email = user.Email;
            usr.UserName = user.UserName;
            usr.Password = user.Password;
            await userRepository.Update(usr);
        }

        public async Task Remove(string id)
        {
            await userRepository.Remove(id);
        }
        public async Task<IEnumerable<UserViewModel>> GetFoll(List<string> followings)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserViewModel>()).CreateMapper();
            var result = mapper.Map<IEnumerable<UserViewModel>>(await userRepository.GetFoll(followings));
            return result;
        }
        public async Task Follow(string userName, string FolUserId)
        {
            if (IsFollow(userName, FolUserId))
            {
                await userRepository.Unfollow(userName, FolUserId);
            }
            else
            {
                await userRepository.Follow(userName, FolUserId);
            }
        }
        
        public bool IsFollow(string userName, string userId)
        {
            return userRepository.IsFollow(userName, userId);
        }
    }
}
