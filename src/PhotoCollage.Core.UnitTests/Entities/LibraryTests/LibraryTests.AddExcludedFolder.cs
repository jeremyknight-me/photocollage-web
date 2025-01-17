using Ardalis.Result;

namespace PhotoCollage.Core.UnitTests.Entities.LibraryTests;

public partial class LibraryTests
{
    [Fact]
    public void AddExcludedFolder_EmptyRelativePath_ReturnsInvalidResult()
    {
        // Act
        var result = this.library.AddExcludedFolder("");

        // Assert
        Assert.False(result.IsSuccess);
        var validationError = Assert.Single(result.ValidationErrors);
        Assert.Equal("relativePath", validationError.Identifier);
        Assert.Equal("Folder relative path cannot be null or empty.", validationError.ErrorMessage);
    }

    [Fact]
    public void AddExcludedFolder_RelativePathAlreadyExists_ReturnsInvalidResult()
    {
        // Arrange
        this.library.AddExcludedFolder("/test/existing");

        // Act
        var result = this.library.AddExcludedFolder("/test/existing");

        // Assert
        Assert.False(result.IsSuccess);
        var validationError = Assert.Single(result.ValidationErrors);
        Assert.Equal("relativePath", validationError.Identifier);
        Assert.Equal("Folder relative path cannot be duplicated.", validationError.ErrorMessage);
    }

    [Fact]
    public void AddExcludedFolder_RelativePathExistsInExcludedFolder_ReturnsNoContentResult()
    {
        // Arrange
        this.library.AddExcludedFolder("/parent");

        // Act
        var result = this.library.AddExcludedFolder("/parent/child");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.NoContent, result.Status);
    }

    [Fact]
    public void AddExcludedFolder_AddsExcludedFolder_ReturnsSuccessResult()
    {
        // Act
        var result = this.library.AddExcludedFolder("/test/new");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.IsType<Result>(result);
        var excludedFolder = Assert.Single(this.library.ExcludedFolders);
        Assert.Equal("test/new", excludedFolder.RelativePath);
    }
}
