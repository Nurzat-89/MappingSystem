namespace MappingSystem.Core;

/// <summary>
/// Defines an interface for a mapping system.
/// </summary>
public interface IMapper
{
    /// <summary>
    /// Determines whether the mapper can handle the mapping between the specified source and target types.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="targetType">The target type.</param>
    bool CanHandle(string sourceType, string targetType);

    /// <summary>
    /// Validates the specified source object to determine if it is compatible with the mapper's source type.
    /// </summary>
    bool ValidateType(object source);
    
    /// <summary>
    /// Maps the specified source object to a target object.
    /// </summary>
    /// <param name="source">The source object to be mapped.</param>
    /// <returns>The mapped target object.</returns>
    object Map(object source);
}

/// <summary>
/// Represents a generic interface for mapping objects of type <typeparamref name="TSource"/> 
/// to objects of type <typeparamref name="TTarget"/>.
/// </summary>
/// <typeparam name="TSource">The type of the source object to be mapped.</typeparam>
/// <typeparam name="TTarget">The type of the target object resulting from the mapping.</typeparam>
public interface IMapper<in TSource, out TTarget> : IMapper
{
    /// <summary>
    /// Maps an object of type <typeparamref name="TSource"/> to an object of type <typeparamref name="TTarget"/>.
    /// </summary>
    TTarget Map(TSource source);
    
    /// <summary>
    /// Validates the specified source object to determine if it meets the requirements for mapping.
    /// </summary>
    /// <param name="source">The source object to validate.</param>
    bool Validate(TSource source);
}