using Applications;
using Core.Models;
using Core.Models.Request;
using DALPostgresSQL;
using Hash;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Test.Services
{
    public class CardItemServiceTest
    {
        private readonly AppDbContext _context;
        private readonly Argon2Hasher _hasher;
        private readonly UserService _UserService;
        private readonly CardService _cardService;
        private readonly CardItemService _service;

        public CardItemServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _hasher = new Argon2Hasher();
            _UserService = new UserService(_context, _hasher);
            _cardService = new CardService(_context);
            _service = new CardItemService(_context, _cardService);
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

            await _cardService.Create(newCards, userId);

            var res = await _cardService.GetCardsAsync(10, 0, userId, "");

            return res.FirstOrDefault();
        }

        [Fact]
        public async Task Create_Should_good()
        {
            var user = await AddUserToDbTest();
            var card = await CreatCartToDbTest(user.Id.ToString());

            var newReq = new CardItemRequest
            {
                CardId = card.Id.ToString(),
                Title = "Test Item",
                Collor = "123"

            };

            var res = await _service.Create(newReq, user.Id.ToString());

            Assert.True(res != null);
        }

        [Fact]
        public async Task Create_Should_false()
        {
            var user = await AddUserToDbTest();

            var newReq = new CardItemRequest
            {
                CardId = Guid.NewGuid().ToString(),
                Title = "Test Item"
            };

            var res = await _service.Create(newReq, user.Id.ToString());

            Assert.True(res == null);
        }

        [Fact]
        public async Task Change_Should_good()
        {
            var user = await AddUserToDbTest();
            var card = await CreatCartToDbTest(user.Id.ToString());

            var newReq = new CardItemRequest
            {
                CardId = card.Id.ToString(),
                Title = "Initial Item"
            };
            await _service.Create(newReq, user.Id.ToString());
            var items = await _service.GetItemsAsync(10, 0, user.Id.ToString(), card.Id.ToString());
            var item = items.FirstOrDefault();

            var changeReq = new CardItemRequest
            {
                Id = item.Id.ToString(),
                CardId = card.Id.ToString(),
                Title = "Changed Title"
            };

            var res = await _service.Change(changeReq, user.Id.ToString());

            Assert.True(res);
        }

        [Fact]
        public async Task Change_Should_false()
        {
            var user = await AddUserToDbTest();
            var card = await CreatCartToDbTest(user.Id.ToString());

            var changeReq = new CardItemRequest
            {
                Id = Guid.NewGuid().ToString(),
                CardId = card.Id.ToString(),
                Title = "Changed Title"
            };

            var res = await _service.Change(changeReq, user.Id.ToString());

            Assert.False(res);
        }

        [Fact]
        public async Task Delete_Should_good()
        {
            var user = await AddUserToDbTest();
            var card = await CreatCartToDbTest(user.Id.ToString());

            var newReq = new CardItemRequest
            {
                CardId = card.Id.ToString(),
                Title = "Item To Delete"
            };
            await _service.Create(newReq, user.Id.ToString());
            var items = await _service.GetItemsAsync(10, 0, user.Id.ToString(), card.Id.ToString());
            var item = items.FirstOrDefault();

            var deleteReq = new CardItemRequest
            {
                Id = item.Id.ToString(),
                CardId = card.Id.ToString()
            };

            var res = await _service.Delete(deleteReq, user.Id.ToString());

            Assert.True(res);
        }

        [Fact]
        public async Task Delete_Should_false()
        {
            var user = await AddUserToDbTest();
            var card = await CreatCartToDbTest(user.Id.ToString());

            var deleteReq = new CardItemRequest
            {
                Id = Guid.NewGuid().ToString(),
                CardId = card.Id.ToString()
            };

            var res = await _service.Delete(deleteReq, user.Id.ToString());

            Assert.False(res);
        }

        [Fact]
        public async Task IsActive_Should_good()
        {
            var user = await AddUserToDbTest();
            var card = await CreatCartToDbTest(user.Id.ToString());

            var newReq = new CardItemRequest
            {
                CardId = card.Id.ToString(),
                Title = "Toggle Item"
            };
            await _service.Create(newReq, user.Id.ToString());
            var items = await _service.GetItemsAsync(10, 0, user.Id.ToString(), card.Id.ToString());
            var item = items.FirstOrDefault();

            var activeReq = new CardItemRequest
            {
                Id = item.Id.ToString(),
                CardId = card.Id.ToString()
            };

            var res = await _service.IsCompleted(activeReq, user.Id.ToString());

            Assert.True(res);
        }

        [Fact]
        public async Task IsActive_Should_false()
        {
            var user = await AddUserToDbTest();
            var card = await CreatCartToDbTest(user.Id.ToString());

            var activeReq = new CardItemRequest
            {
                Id = Guid.NewGuid().ToString(),
                CardId = card.Id.ToString()
            };

            var res = await _service.IsCompleted(activeReq, user.Id.ToString());

            Assert.False(res);
        }

        [Fact]
        public async Task GetItemsAsync_Should_good()
        {
            var user = await AddUserToDbTest();
            var card = await CreatCartToDbTest(user.Id.ToString());

            var res = await _service.GetItemsAsync(10, 0, user.Id.ToString(), card.Id.ToString());

            Assert.NotNull(res);
        }
    }
}