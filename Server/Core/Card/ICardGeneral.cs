using Core.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Card
{
    public interface ICardGeneral<T>
    {
        Task<bool> Change(T cardItemRequest, string userID);
        Task<bool> Delete(T cardItemRequest, string userID);
    }
}
