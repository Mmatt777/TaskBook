namespace TasksBook.Domain.Entities;

public record class ToDoTask
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }      
    public int PercentComplete { get; set; }       
    public bool IsDone { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
