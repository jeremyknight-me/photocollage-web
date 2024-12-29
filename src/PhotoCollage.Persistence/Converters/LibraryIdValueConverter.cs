using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoCollage.Core.ValueObjects;

namespace PhotoCollage.Persistence.Converters;

internal sealed class LibraryIdValueConverter : ValueConverter<LibraryId, int>
{
    public LibraryIdValueConverter()
        : this(null)
    {
    }

    public LibraryIdValueConverter(ConverterMappingHints? mappingHints = null)
        : base(id => id.Value, value => new LibraryId(value), mappingHints)
    {
    }
}
