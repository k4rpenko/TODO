using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class UserModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Salt { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public List<string> RefreshToken { get; set; } = new();

        [EmailAddress]
        public string Email { get; set; } = null!;

        public string Name { get; set; } = null!;

        public ICollection<ToDoCard> Cards { get; set; } = new List<ToDoCard>();
    }
}