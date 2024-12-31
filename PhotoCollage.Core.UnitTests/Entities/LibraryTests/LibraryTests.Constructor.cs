using PhotoCollage.Core.Entities;

namespace PhotoCollage.Core.UnitTests.Entities.LibraryTests;

public partial class LibraryTests
{
    private readonly Library library;

    public LibraryTests()
    {
        this.library = Library.Create("Test Library");
    }
}
