using System;

namespace XenoGears.Ogre.Exploration
{
    [Flags]
    public enum Explore
    {
        Fields = 1,
        Properties = 2,
        FieldsAndProperties = 3,

        Public = 4,
        Private = 8,
        PublicAndPrivate = 12,
    }
}