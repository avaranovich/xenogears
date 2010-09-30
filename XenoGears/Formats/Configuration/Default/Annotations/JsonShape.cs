using System;

namespace XenoGears.Formats.Configuration.Default.Annotations
{
    [Flags]
    public enum JsonShape
    {
        Primitive = 1,
        Object = 2,
        List = 4,
        Hash = 8,
    }
}