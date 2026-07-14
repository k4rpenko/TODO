using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Request
{
    public class CardItemRequest
    {
        public string? Id { get; set; }
        public string? CardId { get; set; }
        public string? Title { get; set; }
        public string? Collor { get; set; }
        public string? Comments { get; set; }
        public List<string>? Hashtag { get; set; }
    }
}
