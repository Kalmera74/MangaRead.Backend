namespace MangaRead.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; private init; }
    public DateTime CreatedAt { get; private init; }
    public DateTime UpdatedAt { get; private set; }


    protected Entity(Guid id)
    {
        Id = id;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    protected void SetUpdatedAt()
    {
        UpdatedAt = DateTime.Now;
    }
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var otherEntity = (Entity)obj;
        return Id == otherEntity.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    //public static bool operator ==(Entity? entity1, Entity? entity2)
    //{
    //    if (ReferenceEquals(entity1, null) && ReferenceEquals(entity2, null))
    //    {
    //        return true;
    //    }
    //    if (ReferenceEquals(entity1, null) || ReferenceEquals(entity2, null))
    //    {
    //        return false;
    //    }
    //    return entity1.Equals(entity2);
    //}

    //public static bool operator !=(Entity? entity1, Entity? entity2)
    //{
    //    if (entity1 == null)
    //    {
    //        return true;
    //    }
    //    if (entity2 == null)
    //    {
    //        return true;
    //    }
    //    return !(entity1 == entity2);
    //}
}