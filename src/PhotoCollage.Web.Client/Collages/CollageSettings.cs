namespace PhotoCollage.Web.Client.Collages;

public sealed record CollageSettings
{
    public bool HasBorder { get; init; } = true;
    public bool IsGrayscale { get; init; } = false;
    public int MaximumRotation { get; init; } = 15;
    public int MaximumSize { get; init; } = 500;
}
