namespace MyBudgetly.Domain.Common;
public abstract class BaseEntity
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    protected BaseEntity()
    { }
    protected BaseEntity(Guid id, DateTime createdAt)
    {
        Id = id;
        CreatedAt = createdAt;
    }
}
