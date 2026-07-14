using Applications;
using Core.Models;
using Core.Models.Request;
using DALPostgresSQL;
using Hash;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services
{
    public class CardServiceTest
    {
        private readonly AppDbContext _context;
        private readonly Argon2Hasher _hasher;
        private readonly UserService _UserService;
        private readonly CardService _service;

        public CardServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _hasher = new Argon2Hasher();
            _UserService = new UserService(_context, _hasher);
            _service = new CardService(_context);
        }

        private async Task<UserModel> AddUserToDbTest()
        {
            var user = await _UserService.AddUserToDB(
                "Kar",
                "String123",
                "kar@gmail.com");

            return user;
        }

        private async Task<ToDoCard> CreatCartToDbTest(string userId)
        {
            var newCards = new CardRequest
            {
                Title = "Test",
                Collor = "123"
            };

            await _service.Create(newCards, userId);

            var res = await _service.GetCardsAsync(10, 0, userId, "");

            return res.FirstOrDefault();
        }

        [Fact]
        public async Task Create_Should_good()
        {
            var user = await AddUserToDbTest();

            var newCards = new CardRequest
            {
                Title = "Test",
                Collor = "123"
            };

            var res = await _service.Create(newCards, user.Id.ToString());

            Assert.True(res != null);
        }

        [Fact]
        public async Task Create_Should_false()
        {
            var user = await AddUserToDbTest();

            var newCards = new CardRequest
            {
                Title = ""
            };

            var res = await _service.Create(newCards, user.Id.ToString());

            Assert.True(res == null);
        }

        [Fact]
        public async Task Change_Should_good()
        {
            var user = await AddUserToDbTest();

            var card = await CreatCartToDbTest(user.Id.ToString());

            var newReq = new CardRequest
            {
                Id = card.Id.ToString()
            };

            var res = await _service.Change(newReq, user.Id.ToString());

            Assert.True(res);
        }

        [Fact]
        public async Task Change_Should_false()
        {
            var user = await AddUserToDbTest();

            var card = await CreatCartToDbTest(user.Id.ToString());

            var newReq = new CardRequest
            {
                Id = "0"
            };

            var res = await _service.Change(newReq, user.Id.ToString());

            Assert.False(res);
        }

        [Fact]
        public async Task Delete_Should_good()
        {
            var user = await AddUserToDbTest();

            var card = await CreatCartToDbTest(user.Id.ToString());

            var cardReq = new CardRequest
            {
                Id = card.Id.ToString()
            };

            var res = await _service.Delete(cardReq, user.Id.ToString());

            Assert.True(res);
        }

        [Fact]
        public async Task Delete_Should_false()
        {
            var user = await AddUserToDbTest();

            var card = await CreatCartToDbTest(user.Id.ToString());
            card.Id = Guid.NewGuid();

            var cardReq = new CardRequest
            {
                Id = card.Id.ToString()
            };

            var res = await _service.Delete(cardReq, user.Id.ToString());

            Assert.False(res);
        }

        [Fact]
        public async Task GetCardById_Should_good()
        {
            var user = await AddUserToDbTest();

            var card = await CreatCartToDbTest(user.Id.ToString());

            var res = await _service.GetCardById(card.Id.ToString(), user.Id.ToString());

            Assert.NotNull(res);
        }

        [Fact]
        public async Task GetCardById_Should_false()
        {
            var user = await AddUserToDbTest();

            var card = await CreatCartToDbTest(user.Id.ToString());
            card.Id = Guid.NewGuid();

            var res = await _service.GetCardById(card.Id.ToString(), user.Id.ToString());

            Assert.Null(res);
        }
    }
}
