using AutoMapper;
using DAL.Models;
using DAL.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using SocNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocNet.Services
{
    public class PostService
    {
        private PostRepository postRepository;
        private UserRepository userRepository;
        public PostService()
        {
            postRepository = new PostRepository();
            userRepository = new UserRepository();
        }
        public async Task Create(PostViewModel entity)
        {
            Post post = new Post();
            post.Id = entity.Id;
            post.Title = entity.Title;
            post.Content = entity.Content;
            post.Author = entity.Author;
            post.Likes = entity.Likes;
            post.Comments = entity.Comments;
            await postRepository.Create(post);
        }

        public async Task<IEnumerable<PostViewModel>> GetAll()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Post, PostViewModel>()).CreateMapper();
            var result = mapper.Map<IEnumerable<PostViewModel>>(await postRepository.GetAll());
            return result;
        }
        public IEnumerable<PostViewModel> GetAllByProfile(UserViewModel user)
        {
            var result = new List<PostViewModel>();
            foreach (var post in GetAll().Result)
            {
                if ((user.Followers.Contains(post.Author)) || (user.UserName == post.Author))
                {
                    result.Add(post);
                }

            }
            return result;
        }

        public async Task<PostViewModel> GetById(string id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Post, PostViewModel>()).CreateMapper();
            var result = mapper.Map<PostViewModel>(await postRepository.GetById(id));
            return result;
        }

        public async Task Remove(string id)
        {
            await postRepository.Remove(id);
        }

        public async Task Update(PostViewModel entity)
        {
            Post post = new Post();
            post.Id = entity.Id;
            post.Title = entity.Title;
            post.Content = entity.Content;
            post.Author = entity.Author;
            post.Likes = entity.Likes;
            post.Comments = entity.Comments;
            await postRepository.Update(post);
        }
        public void AddPost(string content, string title, string userName)
        {
            postRepository.AddPost(content, title, userName);
        }
        public void Like(string userName, string postId)
        {
            if (IsLiked(userName, postId))
            {
                postRepository.RemoveLike(userName, postId);
            }
            else
            {
                postRepository.AddLike(userName, postId);
            }
        }
        public void AddComment(string postId, string content, string userName)
        {
            postRepository.AddComment(postId, content, userName);
        }

        public bool IsLiked(string userName, string postId)
        {
            return postRepository.IsLiked(userName, postId);
        }
    }
}
