using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoCollage.Core.ValueObjects;

namespace PhotoCollage.Persistence.Converters;

internal sealed class PhotoIdValueConverter : ValueConverter<PhotoId, long>
{
    public PhotoIdValueConverter()
        : this(null)
    {    
    }

    public PhotoIdValueConverter(ConverterMappingHints? mappingHints = null)
        : base(id => id.Value, value => new PhotoId(value), mappingHints)
    {
    }
}
