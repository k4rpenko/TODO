using Core.Models;
using Core.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Card
{
    public interface ICardItems : ICardGeneral<CardItemRequest>
    {
        Task<ToDoItem> Create(CardItemRequest cardItemRequest, string userID);
        Task<bool> IsCompleted(CardItemRequest cardItemRequest, string userID);
        Task<List<ToDoItem>> GetItemsAsync(int size, int from, string userID, string cardID);
    }
}
