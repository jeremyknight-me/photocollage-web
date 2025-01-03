namespace PhotoCollage.Core.ValueObjects;

public readonly record struct LibraryId(int Value)
{
    public static implicit operator int(LibraryId l) => l.Value;
}
