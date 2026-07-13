using Core.Card;
using Core.Models;
using Core.Models.Request;
using DALPostgresSQL;
using Microsoft.EntityFrameworkCore;

namespace Applications
{
    public class CardService : ICards
    {
        private readonly AppDbContext _context;

        public CardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Change(CardRequest cardRequest, string userID)
        {
            if (cardRequest == null || userID == null) return false;

            var card = await GetCardById(cardRequest.Id, userID);

            if (card == null) return false;

            card.Title = cardRequest.Title ?? card.Title;
            card.Description = cardRequest.Description ?? card.Description;
            card.Hashtags = cardRequest.Hashtag ?? card.Hashtags;
            card.Collor = cardRequest.Collor ?? card.Collor;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ToDoCard> Create(CardRequest cardRequest, string userID)
        {
            if (cardRequest == null || userID == null || string.IsNullOrEmpty(cardRequest.Title)) return null;

            var newCard = new ToDoCard
            {
                Title = cardRequest.Title,
                Description = cardRequest.Description,
                Hashtags = cardRequest.Hashtag,
                UserId = Guid.Parse(userID),
                Collor = cardRequest.Collor,
                Design = cardRequest.Design
            };

            _context.ToDoCards.Add(newCard);
            await _context.SaveChangesAsync();

            return newCard;
        }

        public async Task<bool> Delete(CardRequest cardRequest, string userID)
        {
            if (cardRequest == null || string.IsNullOrEmpty(cardRequest.Id) || userID == null) return false;

            var card = await GetCardById(cardRequest.Id, userID);

            if (card == null) return false;

            _context.ToDoCards.Remove(card);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ToDoCard> GetCardById(string cardId, string userID)
        {
            if (cardId == null || userID == null) return null;

            if (!Guid.TryParse(cardId, out var cardGuid) || !Guid.TryParse(userID, out var userGuid))
            {
                return null;
            }

            var res = await _context.ToDoCards.FirstOrDefaultAsync(c => c.Id == cardGuid && c.UserId == userGuid);

            if (res == null) return null;

            return res;
        }

        public async Task<List<ToDoCard>> GetCardsAsync(int size, int from, string userID, string? category)
        {
            if (size == null || from == null || userID == null) return null;

            var userGuid = Guid.Parse(userID);

            var cards = await _context.ToDoCards
                .Where(c =>
                    c.UserId == userGuid &&
                    (
                        string.IsNullOrWhiteSpace(category) ||
                        c.Hashtags.Contains(category) ||
                        c.Title.Contains(category) ||
                        c.Description.Contains(category)
                    )
                )
                .Skip(from)
                .Take(size)
                .Select(c => new ToDoCard
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Hashtags = c.Hashtags,
                    UserId = c.UserId,
                    Collor = c.Collor,
                    Design = c.Design
                })
                .ToListAsync();

            if (cards == null) return null;

            return cards;
        }
    }
}
