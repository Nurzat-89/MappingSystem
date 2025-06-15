namespace MappingSystem.Core;

/// <summary>
/// 
/// </summary>
public interface IMapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="targetType"></param>
    /// <returns></returns>
    bool CanHandle(string sourceType, string targetType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    object Map(object source);
}