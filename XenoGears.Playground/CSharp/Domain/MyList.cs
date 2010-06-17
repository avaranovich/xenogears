using System.Collections.Generic;
using System.Linq;
using XenoGears.Functional;

namespace XenoGears.Playground.CSharp.Domain
{
    public class MyList<T> : List<T>
    {
        public MyList()
        {
        }

        public MyList(IEnumerable<T> collection) 
            : base(collection)
        {
        }

        public static MyList<T> operator &(MyList<T> list1, MyList<T> list2)
        {
            return new MyList<T>(list1.Intersect(list2));
        }

        public static MyList<T> operator |(MyList<T> list1, MyList<T> list2)
        {
            return new MyList<T>(list1.Concat(list2));
        }

        public static implicit operator bool(MyList<T> list)
        {
            return !list.IsEmpty();
        }

        public static bool operator true(MyList<T> list)
        {
            return list;
        }

        public static bool operator false(MyList<T> list)
        {
            return !list;
        }
    }
}