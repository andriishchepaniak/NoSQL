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
    public class UserRepository : IRepository<User>
    {
        DbContext<User> dbContext;

        public UserRepository()
        {
            dbContext = new DbContext<User>("users");
        }

        public async Task Create(User entity)
        {
            await dbContext.Collection.InsertOneAsync(entity);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            //var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            //return mapper.Map<IEnumerable<User>, List<UserDTO>>(userRepository.GetAll());
            BsonDocument filter = new BsonDocument();
            return await dbContext.Collection.Find(filter).ToListAsync();
        }
        public async Task<IEnumerable<User>> GetFoll(List<string> followings)
        {
            
            var filter = Builders<User>.Filter.In("UserName", followings);
            var users = await dbContext.Collection.Find(filter).ToListAsync();
            return users;
            //var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            //return mapper.Map<IEnumerable<User>, List<UserDTO>>(userRepository.GetAll());
            //BsonDocument filter = new BsonDocument();
            //return await dbContext.Collection.Find(filter).ToListAsync();
        }

        public async Task<User> GetById(string id)
        {
            return await dbContext.Collection.Find(new BsonDocument("_id", new ObjectId(id))).SingleOrDefaultAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await dbContext.Collection.Find(u => u.Email == email).SingleOrDefaultAsync();
        }
        public async Task<User> GetByUserName(string userName)
        {
            return await dbContext.Collection.Find(u => u.UserName == userName).SingleOrDefaultAsync();
        }

        public async Task Remove(string id)
        {
            await dbContext.Collection.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }

        public async Task Update(User entity)
        {
            await dbContext.Collection.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(entity.Id)), entity);
        }
        public async Task<IEnumerable<User>> GetAllFolUser(string userName)
        {
            var result = await dbContext.Collection.Find(u => u.Followers.Contains(userName)).ToListAsync();

            return result;
        }
        public async Task Follow(string userName, string FolUserId)
        {
            var user = GetByUserName(userName).Result;
            var folluser = GetById(FolUserId);
            var filter = Builders<User>.Filter.Eq(el => el.UserName, userName);
            var update = Builders<User>.Update
                    .Push<string>(el => el.Followers, folluser.Result.UserName);

            await dbContext.Collection.FindOneAndUpdateAsync(filter, update);

            //var filter2 = Builders<User>.Filter.Eq(el => el.UserName, FolUserName);
            //var update2 = Builders<User>.Update
            //        .Push<string>(el => el.Subs, userName);

            //await _context.Users.FindOneAndUpdateAsync(filter2, update2);
        }
        
        public async Task Unfollow(string userName, string FolUserId)
        {
            var user = GetByUserName(userName).Result;
            var folluser = GetById(FolUserId);
            var filter = Builders<User>.Filter.Eq(el => el.UserName, userName);
            var update = Builders<User>.Update
                    .Pull<string>(el => el.Followers, folluser.Result.UserName);

            await dbContext.Collection.FindOneAndUpdateAsync(filter, update);

            //var filter2 = Builders<User>.Filter.Eq(el => el.UserName, FolUserName);
            //var update2 = Builders<User>.Update
            //        .Pull<string>(el => el.Subs, userName);

            //await _context.Users.FindOneAndUpdateAsync(filter2, update2);
        }
        public bool IsFollow(string userName, string userId)
        {
            var user = GetByUserName(userName).Result;
            //var folluser = GetById(userId);

            var folluser = dbContext.Collection.Find(p => p.Id == userId).Single();
            if(user.Followers.Contains(folluser.UserName))
            {
                return true;
            }
            else return false;
            //return post.Followers.AsEnumerable().Any(f => f == user.UserName);
        }
    }
}
