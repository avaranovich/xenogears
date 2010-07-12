using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Unsafe
{
    [DebuggerNonUserCode]
    public static class SizeofHelper
    {
        // this takes into account auxiliary data such as: sync roots, vtables and so on
        public static int SizeOfData(this Type t)
        {
            throw new NotImplementedException();
        }

        // this takes into account auxiliary data such as: sync roots, vtables and so on
        public static int SizeOfData(this Object o)
        {
            throw new NotImplementedException();
        }

        // this doesn't take into account auxiliary data such as: sync roots, vtables and so on
        public static int SizeOfUsefulData(this Type t)
        {
            t.AssertNotNull();
            if (t.IsValueType)
            {
                // todo. will crash for structures that have reference-type fields
                return Marshal.SizeOf(t);
            }
            else if (t.IsArray)
            {
                // no idea - since arrays can have different size
                throw AssertionHelper.Fail();
            }
            else if (t.IsClass)
            {
                // todo. to be implemented
                throw AssertionHelper.Fail();
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }

        // this doesn't take into account auxiliary data such as: sync roots, vtables and so on
        public static int SizeOfUsefulData(this Object o)
        {
            o.AssertNotNull();
            var t = o.GetType();

            if (t.IsValueType)
            {
                // todo. will crash for structures that have a reference-type fields
                return Marshal.SizeOf(t);
            }
            else if (t.IsArray)
            {
                // todo. won't work for arrays that have differently sized elements
                var elcount = o.AssertCast<Array>().Dims().Product();
                return elcount * t.GetElementType().SizeOfUsefulData();
            }
            else if (t.IsClass)
            {
                // todo. to be implemented
                throw AssertionHelper.Fail();
            }
            else
            {
                throw AssertionHelper.Fail();
            }
        }
    }
}
