using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using MongoDB.Driver;

namespace API.Data.MongoRepository
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMapper _mapper;

        public MongoUserRepository(IConfiguration configuration, IMapper mapper)
        {
            var conString = configuration["MongoDbConfig:ConnectionString"];
            _mongoClient = new MongoClient(conString);
            _mapper = mapper;
        }

        private IMongoCollection<AppUser> GetMongoCollection()
        {
            return _mongoClient.GetDatabase("DatingAppDb").GetCollection<AppUser>("User");
        }

        public async Task AddUserAsync(AppUser user)
        {
            await GetMongoCollection().InsertOneAsync(user);
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            var user = await GetUserByUserNameAsync(username);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<MemberDto>(user);
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));


            var userFilter = Builders<AppUser>.Filter.Ne(x =>
                x.UserName, userParams.CurrentUserName);

            var genderFilter = Builders<AppUser>.Filter.Eq(x => x.Gender, userParams.Gender);

            var dobFilter1 = Builders<AppUser>.Filter.Gte(x => x.DateOfBirth, minDob);

            var dobFilter2 = Builders<AppUser>.Filter.Lte(x => x.DateOfBirth, maxDob);

            var filter = userFilter & genderFilter & dobFilter1 & dobFilter2;

            var createDateSortDefinition = Builders<AppUser>.Sort.Ascending(x => x.Created);
            var lastActiveSortDefinition = Builders<AppUser>.Sort.Ascending(x => x.LastActive);

            var sortDefinition = userParams.OrderBy == "created" ? createDateSortDefinition : lastActiveSortDefinition;

            var totalCount = await GetMongoCollection().CountDocumentsAsync(filter);

            var cursor = GetMongoCollection().Find(filter).Sort(sortDefinition)
                .Skip((userParams.PageNumber - 1) * userParams.PageSize)
                .Limit(userParams.PageSize);

            var users = await cursor.ToListAsync();

            var members = new List<MemberDto>();

            foreach (var user in users)
            {
                members.Add(_mapper.Map<MemberDto>(user));
            }

            return new PagedList<MemberDto>(members, (int)totalCount, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return (await GetMongoCollection().FindAsync(x => x.Id == id)).FirstOrDefault();
        }

        public async Task<AppUser> GetUserByUserNameAsync(string username)
        {
            var filter = Builders<AppUser>.Filter.Eq(x => x.UserName, username);
            var cursor = await GetMongoCollection().FindAsync(filter);
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return (await GetMongoCollection().FindAsync(x => true)).ToList();
        }

        public void Update(AppUser user)
        {
            GetMongoCollection().ReplaceOne(x => x.Id == user.Id, user);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await GetUserByUserNameAsync(username) != null;
        }
    }
}
