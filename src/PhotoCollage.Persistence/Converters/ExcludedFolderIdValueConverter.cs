using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoCollage.Core.ValueObjects;

namespace PhotoCollage.Persistence.Converters;

internal sealed class ExcludedFolderIdValueConverter : ValueConverter<ExcludedFolderId, int>
{
    public ExcludedFolderIdValueConverter()
        : this(null)
    {
    }

    public ExcludedFolderIdValueConverter(ConverterMappingHints? mappingHints = null)
        : base(id => id.Value, value => new ExcludedFolderId(value), mappingHints)
    {
    }
}
