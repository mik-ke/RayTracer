using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;

namespace RayTracer.UI.WPF.Extensions;

public class EnumCollectionExtension : MarkupExtension
{
    public Type? EnumType { get; set; }
    
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(EnumType);

        return CreateEnumValueList(EnumType);
    }
    
    private static List<object> CreateEnumValueList(Type enumType)
    {
        return Enum.GetNames(enumType)
            .Select(name => Enum.Parse(enumType, name))
            .ToList();
    }
}