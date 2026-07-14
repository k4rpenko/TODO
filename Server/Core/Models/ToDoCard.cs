namespace Core.Models
{
    public class ToDoCard
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public UserModel User { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Collor { get; set; } = null!;

        public int Design { get; set; }

        public List<string>? Hashtags { get; set; } = new();

        public List<ToDoItem> Items { get; set; } = new();
    }
}