using System;

namespace XenoGears.Formats
{
    public partial class Json
    {
        public String ToCompactString()
        {
            throw new NotImplementedException();
        }

        public String ToPrettyString()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return ToCompactString();
        }
    }
}
