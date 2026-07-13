using Core.Models;
using Core.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Card
{
    public interface ICards : ICardGeneral<CardRequest>
    {
        Task<ToDoCard> Create(CardRequest cardItemRequest, string userID);
        Task<ToDoCard> GetCardById(string cardId, string userID);
        Task<List<ToDoCard>> GetCardsAsync(int size, int from, string userID, string? category);
    }
}
