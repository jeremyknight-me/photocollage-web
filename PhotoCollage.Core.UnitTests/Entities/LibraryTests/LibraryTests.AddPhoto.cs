using Ardalis.Result;
using PhotoCollage.Core.ValueObjects;

namespace PhotoCollage.Core.UnitTests.Entities.LibraryTests;

public partial class LibraryTests
{
    [Fact]
    public void AddPhoto_EmptyRelativePath_ReturnsInvalidResult()
    {
        // Act
        var file = PhotoFile.Create("", "", 100);
        var result = this.library.AddPhoto(file);

        // Assert
        Assert.False(result.IsSuccess);
        var validationError = Assert.Single(result.ValidationErrors);
        Assert.Equal(nameof(PhotoFile.RelativePath), validationError.Identifier);
        Assert.Equal("Photo relative path cannot be null or empty.", validationError.ErrorMessage);
    }

    [Fact]
    public void AddPhoto_RelativePathAlreadyExists_ReturnsInvalidResult()
    {
        // Arrange
        var existingFile = PhotoFile.Create("/test/existing", "", 100);
        _ = this.library.AddPhoto(existingFile);

        // Act
        var file = PhotoFile.Create("/test/existing", "", 100);
        var result = this.library.AddPhoto(file);

        // Assert
        Assert.False(result.IsSuccess);
        var validationError = Assert.Single(result.ValidationErrors);
        Assert.Equal(nameof(PhotoFile.RelativePath), validationError.Identifier);
        Assert.Equal("Photo relative path cannot be duplicated.", validationError.ErrorMessage);
    }

    [Fact]
    public void AddPhoto_RelativePathExistsInExcludedFolder_ReturnsNoContentResult()
    {
        // Arrange
        this.library.AddExcludedFolder("/test/existing");

        // Act
        var file = PhotoFile.Create("/test/existing", "", 100);
        var result = this.library.AddPhoto(file);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.NoContent, result.Status);
    }

    [Fact]
    public void AddPhoto_AddsPhoto_ReturnsSuccessResult()
    {
        // Act
        var file = PhotoFile.Create("/test/new", "", 100);
        var result = this.library.AddPhoto(file);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.Ok, result.Status);
        var photo = Assert.Single(this.library.Photos);
        Assert.Equal("test/new", photo.RelativePath);
        Assert.Equal(100, photo.SizeBytes);
    }
}
