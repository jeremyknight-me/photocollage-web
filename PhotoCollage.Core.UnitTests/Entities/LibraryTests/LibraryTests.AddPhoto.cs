using Ardalis.Result;

namespace PhotoCollage.Core.UnitTests.Entities.LibraryTests;

public partial class LibraryTests
{
    [Fact]
    public void AddPhoto_EmptyRelativePath_ReturnsInvalidResult()
    {
        // Act
        var result = this.library.AddPhoto("", 100);

        // Assert
        Assert.False(result.IsSuccess);
        var validationError = Assert.Single(result.ValidationErrors);
        Assert.Equal("relativePath", validationError.Identifier);
        Assert.Equal("Photo relative path cannot be null or empty.", validationError.ErrorMessage);
    }

    [Fact]
    public void AddPhoto_RelativePathAlreadyExists_ReturnsNoContentResult()
    {
        // Arrange
        this.library.AddExcludedFolder("/test/existing");

        // Act
        var result = this.library.AddPhoto("/test/existing", 100);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.NoContent, result.Status);
    }

    [Fact]
    public void AddPhoto_RelativePathExistsInExcludedFolder_ReturnsNoContentResult()
    {
        // Arrange
        this.library.AddExcludedFolder("/test/existing");

        // Act
        var result = this.library.AddPhoto("/test/existing", 100);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.NoContent, result.Status);
    }

    [Fact]
    public void AddPhoto_AddsPhoto_ReturnsSuccessResult()
    {
        // Act
        var result = this.library.AddPhoto("/test/new", 100);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.Ok, result.Status);
        var photo = Assert.Single(this.library.Photos);
        Assert.Equal("/test/new", photo.RelativePath);
        Assert.Equal(100, photo.SizeBytes);
    }
}
