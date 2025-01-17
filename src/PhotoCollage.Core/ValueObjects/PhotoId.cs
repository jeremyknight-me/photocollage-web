namespace PhotoCollage.Core.ValueObjects;

public readonly record struct PhotoId(long Value)
{
    public static implicit operator long(PhotoId p) => p.Value;
    public static implicit operator PhotoId(long l) => new(l);
}
