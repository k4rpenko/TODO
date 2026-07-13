using Applications;
using Core.Models;
using DALPostgresSQL;
using Hash;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Test.Services
{
    public class UserServiceTest
    {
        private readonly AppDbContext _context;
        private readonly Argon2Hasher _hasher;
        private readonly UserService _service;

        public UserServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _hasher = new Argon2Hasher();
            _service = new UserService(_context, _hasher);
        }

        private async Task<UserModel> AddUserToDbTest()
        {
            var user = await _service.AddUserToDB(
                "Kar",
                "String123",
                "kar@gmail.com");

            return user;
        }

        [Fact]
        public async Task AddUserToDB_Should_Add_User()
        {
            var user = await AddUserToDbTest();

            Assert.NotNull(user);
            Assert.Single(_context.Users);
        }

        [Fact]
        public async Task CheckUserInDB_Should_good()
        {
            var user = await AddUserToDbTest();

            var res = await _service.CheckUserInDB("kar@gmail.com");

            Assert.NotNull(res);
        }

        [Fact]
        public async Task CheckUserInDB_Should_false()
        {
            var user = await AddUserToDbTest();

            var res = await _service.CheckUserInDB("karTest@gmail.com");

            Assert.Null(res);
        }

        [Fact]
        public async Task CheckPassword_Should_good()
        {
            var user = await AddUserToDbTest();

            var res = await _service.CheckPassword("String123", user.PasswordHash, user.Salt);

            Assert.True(res);
        }

        [Fact]
        public async Task CheckPassword_Should_false()
        {
            var user = await AddUserToDbTest();

            var res = await _service.CheckPassword("String1234567", user.PasswordHash, user.Salt);

            Assert.False(res);
        }

        [Fact]
        public async Task UpdateUserInDB_Should_good()
        {
            var user = await AddUserToDbTest();

            var res = await _service.UpdateUserInDB("Max", "", user.Id.ToString());

            user = await _service.GetUserById(user.Id.ToString());

            Assert.True(user.Name == "Max");
        }

        [Fact]
        public async Task UpdateUserInDB_Should_false()
        {
            var user = await AddUserToDbTest();

            var res = await _service.UpdateUserInDB("", "", user.Id.ToString());

            user = await _service.GetUserById(user.Id.ToString());

            Assert.False(user.Name == "Max");
        }

        [Fact]
        public async Task GetUserById_Should_good()
        {
            var user = await AddUserToDbTest();

            var res = await _service.GetUserById(user.Id.ToString());

            Assert.NotNull(res);
        }

        [Fact]
        public async Task GetUserById_Should_false()
        {
            var user = await AddUserToDbTest();

            var res = await _service.GetUserById("123");

            Assert.Null(res);
        }

        [Fact]
        public async Task AddRefreshToken_Should_good()
        {
            var user = await AddUserToDbTest();

            var res = await _service.AddRefreshToken(user.Id.ToString(), "123");

            Assert.True(res);
        }

        [Fact]
        public async Task AddRefreshToken_Should_false()
        {
            var user = await AddUserToDbTest();

            var res = await _service.AddRefreshToken(user.Id.ToString(), "");

            Assert.False(res);
        }

    }
}