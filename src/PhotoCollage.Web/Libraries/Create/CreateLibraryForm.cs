using System.ComponentModel.DataAnnotations;

namespace PhotoCollage.Web.Libraries.Create;

internal class CreateLibraryForm
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public string? Description { get; set; }

    internal CreateLibraryCommand ToCommand()
        => new()
        {
            Name = this.Name,
            Description = this.Description
        };
}
