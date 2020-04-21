using AutoMapper;
using DAL.DBWork;
using DAL.Interfaces;
using DAL.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class PostRepository : IRepository<Post>
    {
        DbContext<Post> dbContext;
        UserRepository _userRepository;
        public PostRepository()
        {
            dbContext = new DbContext<Post>("posts");
            _userRepository = new UserRepository();
        }

        public async Task Create(Post entity)
        {
            await dbContext.Collection.InsertOneAsync(entity);
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            BsonDocument filter = new BsonDocument();
            return await dbContext.Collection.Find(filter).ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetAllByProfileAsync(string userName)
        {
            var list = await dbContext.Collection.Find(p => p.Author == userName).ToListAsync();
            return list;
            
        }

        public async Task<Post> GetById(string id)
        {
            return await dbContext.Collection.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }

        public async Task Remove(string id)
        {
            await dbContext.Collection.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }

        public async Task Update(Post entity)
        {
            await dbContext.Collection.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(entity.Id)), entity);
        }
        public void AddPost(string content, string title, string userName)
        {
            var user = _userRepository.GetByUserName(userName);
            var post = new Post()
            {
                Author = userName,
                Title = title,
                Content = content,
                Comments = new List<Comment>(),
                Likes = new List<Like>(),

            };

            var builder = Builders<Post>.Filter;
            post.Id = Convert.ToString(ObjectId.GenerateNewId());
            dbContext.Collection.InsertOne(post);
        }
        public void AddLike(string userName, string postId)
        {
            //var like = _mapper.Map<Like>(_userService.GetByUserName(userName));
            Like like = new Like();     
            var user = _userRepository.GetByUserName(userName).Result;
            like.UserName = user.UserName;
            var filter = Builders<Post>.Filter.Eq(el => el.Id, postId);
            var update = Builders<Post>.Update
                    .Push<Like>(el => el.Likes, like);

            dbContext.Collection.FindOneAndUpdate(filter, update);
        }
        public void RemoveLike(string userName, string postId)
        {
            Like like = new Like();
            var user = _userRepository.GetByUserName(userName).Result;
            like.UserName = user.UserName;

            var filter = Builders<Post>.Filter.Eq(el => el.Id, postId);
            var update = Builders<Post>.Update
                    .Pull<Like>(el => el.Likes, like);

            dbContext.Collection.FindOneAndUpdate(filter, update);
        }
        public void AddComment(string postId, string content, string userName)
        {
            Comment comment = new Comment
            {
                Author = userName,
                Content = content                
            };

            var filter = Builders<Post>.Filter.Eq(el => el.Id, postId);
            var update = Builders<Post>.Update
                    .Push<Comment>(el => el.Comments, comment);

            dbContext.Collection.FindOneAndUpdate(filter, update);
        }

        public bool IsLiked(string userName, string postId)
        {
            var post = dbContext.Collection.Find(p => p.Id == postId).Single();
            return post.Likes.AsEnumerable().Any(p => p.UserName == userName);
        }
        
    }
}
