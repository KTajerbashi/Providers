namespace UnitOfWork.WebApp.Common;

public interface IEntity
{
    int Id { get; }
    Guid EntityId { get; }
    bool IsDeleted { get; }
    bool IsActive { get; }
    void Delete();
    void Restore();
}
public abstract class Entity : IEntity
{
    public int Id { get; set; }

    public Guid EntityId { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsActive { get; set; }

    public void Delete()
    {
        IsDeleted = true;
        IsActive = false;
    }

    public void Restore()
    {
        IsDeleted = false;
        IsActive = true;
    }
}
public abstract class AggregateRoot : Entity
{

}
