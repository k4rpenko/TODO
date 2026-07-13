using Core.Hash;
using Core.Models;
using Core.User;
using DALPostgresSQL;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Applications
{
    public class UserService : IUser
    {
        private readonly AppDbContext _context;
        private readonly IArgon2Hasher _argon2;

        public UserService(AppDbContext context, IArgon2Hasher argon2)
        {
            _context = context;
            _argon2 = argon2;
        }
        public async Task<UserModel> AddUserToDB(string Name, string Password, string Email)
        {
            var KeyG = _argon2.GenerateKey().Replace("-", "").ToLower();
            var passwordHash = _argon2.Hash(Password, KeyG);

            var newUser = new UserModel
            {
                Name = Name,
                PasswordHash = passwordHash,
                Email = Email,
                Salt = KeyG
            };

            _context.Users.Add(newUser);

            await _context.SaveChangesAsync();

            var record = await _context.Users.FindAsync(newUser.Id);

            return record != null ? newUser : null;
        }

        public async Task<string> CheckUserInDB(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return null;

            return user.Id.ToString();
        }

        public async Task<bool> CheckPassword(string password, string userPasswordHash, string userSalt)
        {
            var passwordHash = _argon2.Hash(password, userSalt);
            return passwordHash == userPasswordHash;
        }

        public async Task<bool> DeleteUserFromDB(string userId)
        {
            var user = await GetUserById(userId);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);

            return true;
        }

        public async Task<bool> UpdateUserInDB(string? Name, string? Email, string userId)
        {
            var user = await GetUserById(userId);

            if (user == null)
            {
                return false;
            }

            user.Name = Name ?? user.Name;
            user.Email = Email ?? user.Email;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserModel> GetUserById(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<bool> AddRefreshToken(string userId, string RefreshToken)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(RefreshToken)) return false;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);

            if (user == null) return false;

            user.RefreshToken.Add(RefreshToken);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteRefreshToken(string userId, string RefreshToken)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(RefreshToken)) return false;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);

            user.RefreshToken.Remove(RefreshToken);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> isHaveToken(string userId, string RefreshToken)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(RefreshToken)) return false;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);

            bool isHave = user.RefreshToken.Contains(RefreshToken);

            return isHave;
        }
    }
}
