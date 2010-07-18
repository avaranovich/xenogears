using System.Diagnostics;

namespace System
{
    [DebuggerNonUserCode]
    internal static class TupleImplHelper
    {
        internal static int CombineHashCodes(params int[] codes)
        {
            int num = codes.Length - 1;
            while (num > 1)
            {
                for (int i = 0; (i * 2) <= num; i++)
                {
                    int index = i * 2;
                    int num4 = index + 1;
                    if (index == num)
                    {
                        codes[i] = codes[index];
                        num = i;
                    }
                    else
                    {
                        codes[i] = CombineHashCodes(codes[index], codes[num4]);
                        if (num4 == num)
                        {
                            num = i;
                        }
                    }
                }
            }
            return CombineHashCodes(codes[0], codes[1]);
        }

        private static int CombineHashCodes(int h1, int h2)
        {
            return (((h1 << 5) + h1) ^ h2);
        }
    }
}