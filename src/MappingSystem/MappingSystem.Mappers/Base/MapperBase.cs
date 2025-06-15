using MappingSystem.Core;

namespace MappingSystem.Mappers.Base;

public abstract class MapperBase<TSource, TTarget> : IMapper<TSource, TTarget>
{
    public Type SourceType => typeof(TSource);
    
    public Type TargetType => typeof(TTarget);

    public abstract TTarget Map(TSource source);
    
    public abstract bool Validate(TSource source);

    public bool ValidateType(object source)
    {
        if (source is not TSource)
            throw new InvalidCastException($"Source object is not type of '{SourceType.FullName}'");
        return true;
    }

    object IMapper.Map(object source) => Map((TSource)source);

    public bool CanHandle(string sourceType, string targetType)
        => sourceType == SourceType.FullName && targetType == TargetType.FullName;
}