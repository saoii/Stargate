using System.Reflection;

namespace Stargate.Services.Helpers;

[AttributeUsage(AttributeTargets.Field)]
public sealed class FriendlyTextAttribute : Attribute
{
    public string FriendlyText { get; }

    public FriendlyTextAttribute(string friendlyText)
    {
        FriendlyText = friendlyText;
    }
}

internal static class TextGenHelper
{
    internal static string GetFriendlyText<T>(T value) where T : Enum
    {
        Type type = value.GetType();
        FieldInfo? memberInfo = type.GetField(value.ToString());
        object[]? attribs = memberInfo?.GetCustomAttributes(typeof(FriendlyTextAttribute), false);
        if (attribs == null || attribs.Length == 0)
            return value.ToString();
        var attrib = (FriendlyTextAttribute)attribs[0];
        return attrib.FriendlyText;
    }
}