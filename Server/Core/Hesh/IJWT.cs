using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Hesh
{
    public enum TokenType
    {
        Access,
        Refresh
    }

    public interface IJWT
    {
        string GenerateToken(string userId, string email, TokenType type);
        string? DecodeToken(string tokenString);
        bool ValidateToken(string tokenString);
    }
}
