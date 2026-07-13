using Core.Card;
using Core.Models;
using Core.Models.Request;
using DALPostgresSQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications
{
    public class CardItemService : ICardItems
    {
        private readonly AppDbContext _context;
        private readonly ICards _cards;

        public CardItemService(AppDbContext context, ICards cards)
        {
            _context = context;
            _cards = cards;
        }

        public async Task<bool> Change(CardItemRequest cardItemRequest, string userID)
        {
            if(cardItemRequest == null || userID == null) return false;

            var itemId = Guid.Parse(cardItemRequest.Id);

            var cardItem = _context.ToDoItems.FirstOrDefault(x => x.Id == itemId);

            if(cardItem == null) return false;
        
            cardItem.Title = cardItemRequest.Title ?? cardItem.Title;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ToDoItem> Create(CardItemRequest cardItemRequest, string userID)
        {
            if (cardItemRequest == null || userID == null) return null;

            var card = await _cards.GetCardById(cardItemRequest.CardId, userID);

            if (card == null) return null;

            var newItem = new ToDoItem
            {
                Id = Guid.NewGuid(),
                ToDoCardId = card.Id,
                Title = cardItemRequest.Title,
                IsCompleted = false
            };

            _context.ToDoItems.Add(newItem);

            await _context.SaveChangesAsync();

            newItem.ToDoCard = null;

            return newItem;
        }

        public async Task<bool> Delete(CardItemRequest cardItemRequest, string userID)
        {
            if (cardItemRequest == null || userID == null) return false;

            var card = await _cards.GetCardById(cardItemRequest.CardId, userID);

            if (card == null) return false;

            var item = await GetCardItemById(cardItemRequest, userID);

            if (item == null) return false;

            _context.ToDoItems.Remove(item);

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> IsCompleted(CardItemRequest cardItemRequest, string userID)
        {
            if (cardItemRequest == null || userID == null) return false;
                
            var item = await GetCardItemById(cardItemRequest, userID);

            if (item == null) return false;

            item.IsCompleted = !item.IsCompleted;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ToDoItem>> GetItemsAsync(int size, int from, string userID, string cardID)
        {

            var items =  _context.ToDoItems.Where(x => x.ToDoCardId.ToString() == cardID).Skip(from).Take(size).ToList();


            return items;
        }

        private async Task<ToDoItem> GetCardItemById(CardItemRequest cardItemRequest, string userID)
        {
            if (cardItemRequest == null || userID == null) return null;

            var itemId = Guid.Parse(cardItemRequest.Id);

            var item = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == itemId);

            if (item == null) return null;

            return item;
        }
    }
}
