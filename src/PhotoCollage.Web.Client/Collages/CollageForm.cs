namespace PhotoCollage.Web.Client.Collages;

internal sealed class CollageForm
{
    public int LibraryId { get; set; }

    public bool CanSubmit => this.LibraryId > 0;
}
