using AutoMapper;
using AutoMapper.Internal;

namespace VgAutoDrill.Central.Core.AutoMapper;

public static class MappingExpressionExtensions
{
    public static IMappingExpression<TSource, TDestination> Advanced<TSource, TDestination>(this IProjectionExpression<TSource, TDestination> projection) =>
       (IMappingExpression<TSource, TDestination>)projection;
    public static TypeMap FindTypeMapFor<TSource, TDestination>(this IConfigurationProvider configurationProvider) => configurationProvider.Internal().FindTypeMapFor<TSource, TDestination>();
    public static IReadOnlyCollection<TypeMap> GetAllTypeMaps(this IConfigurationProvider configurationProvider) => configurationProvider.Internal().GetAllTypeMaps();
    public static TypeMap ResolveTypeMap(this IConfigurationProvider configurationProvider, Type sourceType, Type destinationType) => configurationProvider.Internal().ResolveTypeMap(sourceType, destinationType);
    public static void ForAllMaps(this IMapperConfigurationExpression configurationProvider, Action<TypeMap, IMappingExpression> configuration) => configurationProvider.Internal().ForAllMaps(configuration);
    public static void ForAllPropertyMaps(this IMapperConfigurationExpression configurationProvider, Func<PropertyMap, bool> condition, Action<PropertyMap, IMemberConfigurationExpression> memberOptions) =>
        configurationProvider.Internal().ForAllPropertyMaps(condition, memberOptions);
    public static void AddIgnoreMapAttribute(this IMapperConfigurationExpression configuration)
    {
        configuration.ForAllMaps((typeMap, mapExpression) => mapExpression.ForAllMembers(memberOptions =>
        {
            if (memberOptions.DestinationMember.Has<IgnoreMapAttribute>())
            {
                memberOptions.Ignore();
            }
        }));
        configuration.ForAllPropertyMaps(propertyMap => propertyMap.SourceMember?.Has<IgnoreMapAttribute>() == true,
            (_, memberOptions) => memberOptions.Ignore());
    }
}

/// <summary>
/// Ignore this member for validation and skip during mapping
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class IgnoreMapAttribute : Attribute
{
}
