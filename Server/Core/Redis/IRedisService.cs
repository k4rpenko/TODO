using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Redis
{
    public interface IRedisService
    {
        Task<bool> isAuthRedisUser(string ip);
    }
}
