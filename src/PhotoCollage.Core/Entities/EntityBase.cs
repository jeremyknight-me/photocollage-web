namespace PhotoCollage.Core.Entities;

public abstract class EntityBase<TId> : EntityBase
    where TId : struct, IEquatable<TId>
{
    public TId Id { get; private set; }
}

public abstract class EntityBase
{
    public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset DateModified { get; private set; } = DateTimeOffset.UtcNow;
}
