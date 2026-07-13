using Core.Models;
using Core.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.User
{
    public interface IUser
    {
        Task<UserModel> AddUserToDB(string Name, string Password, string Email);
        Task<bool> DeleteUserFromDB(string userId);
        Task<string> CheckUserInDB(string email);
        Task<bool> UpdateUserInDB(string? Name, string? Email, string userId);
        Task<bool> CheckPassword(string password, string userPasswordHash, string userSalt);
        Task<UserModel> GetUserById(string userId);
        Task<bool> AddRefreshToken(string userId, string RefreshToken);
        Task<bool> DeleteRefreshToken(string userId, string RefreshToken);
        Task<bool> isHaveToken(string userId, string RefreshToken);
    }
}
