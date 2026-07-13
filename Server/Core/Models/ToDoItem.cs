namespace Core.Models
{
    public class ToDoItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ToDoCardId { get; set; }
        public ToDoCard ToDoCard { get; set; } = null!;

        public string Title { get; set; } = null!;

        public bool IsCompleted { get; set; }
    }
}