namespace PhotoCollage.Core.ValueObjects;

public readonly record struct ExcludedFolderId(int Value)
{
    public static implicit operator int(ExcludedFolderId f) => f.Value;
}

