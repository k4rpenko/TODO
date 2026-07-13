using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Core.Models.Request
{
    public class CardRequest
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Collor { get; set; }
        public int Design { get; set; }
        public List<string>? Hashtag { get; set; }
    }
}
