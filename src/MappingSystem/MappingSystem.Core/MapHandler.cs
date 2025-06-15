namespace MappingSystem.Core;

/// <summary>
/// Provides functionality for mapping data between different types.
/// </summary>
public class MapHandler(IEnumerable<IMapper> mappers)
{
    private readonly List<IMapper> _mappers = mappers.ToList();

    /// <summary>
    /// Map data between various source and target formats based on to provided sourceType and targetType.
    /// </summary>
    /// <param name="data">The data to be mapped.</param>
    /// <param name="sourceType">The type of the source data.</param>
    /// <param name="targetType">The type to which the data should be mapped.</param>
    public object Map(object data, string sourceType, string targetType)
    {
        var mapper = _mappers.FirstOrDefault(m => m.CanHandle(sourceType, targetType)) ??
                     throw new InvalidOperationException($"No mapper found for {sourceType} -> {targetType}");
        
        mapper.ValidateType(data);
        return mapper.Map(data);
    }
}