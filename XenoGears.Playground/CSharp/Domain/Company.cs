using System;
using System.Collections.Generic;

namespace XenoGears.Playground.CSharp.Domain
{
    public class Company : IComparable<Company>
    {
        public String Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public DateTime FoundationDate { get; set; }

        public MyList<Employee> Employees { get; set; }

        public DayOfWeek DayOfWeek { get { return FoundationDate.DayOfWeek; } }

        public Employee BestEmployee { get; set; }

        public Guid Guid { get; set; }

        public Company()
        {
            Employees = new MyList<Employee>();
        }

        // test stuff
        public int this[int index1, int index2]
        {
            get
            {
                return index1 * index2;
            }
        }

        // test stuff
        public int GetIntImpl(){ return 0; }
        public delegate int DGetInt();
        public DGetInt D { get { return GetIntImpl; } }

        public int[] Array1D = new int[]{0, 1};
        public int[][] Array2DJagged = new int[][] { new int[]{0, 1} };
        public int[,] Array2DRect = new int[,] { {0, 1}, {0, 1} };

        public bool LolMethod(char c) { return true; }
        public bool LolMethod(int? i_nullable) { return true; }
        public bool LolMethod(int @int, params int[] ints) { return true; }

        // test stuff
        public int CompareTo(Company other) { throw new NotImplementedException(); }

        public bool LolMethod2(String[] s) { return s.Length == 1 && s[0] == "lol"; }
        public Decimal Decimal() { return 0; }

        public TypeB B { get; set; }
        public TypeC C { get; set; }
        public TypeE E { get; set; }
        public TypeF F { get; set; }

        public bool LolMethod3(params TypeA[] a) { return false; }
        public bool LolMethod3(TypeB b, TypeA a) { return false; }
        public bool LolMethod3(TypeA a, TypeB b) { return false; }

        public int LolMethod4<T>() { throw new NotImplementedException(); }

        public bool LolMethod5(Func<Employee, int> f) { throw new NotImplementedException(); }
        public Func<Employee, bool> LolMethod5(Func<Company, int> f) { throw new NotImplementedException(); }

        public bool LolMethod6<T>(T t) where T : Employee { throw new NotImplementedException(); }

        public bool LolMethod7<U>(U[] t) { throw new NotImplementedException(); }

        public bool LolMethod8<U>(List<U> t) { throw new NotImplementedException(); }

        public bool LolMethod9<U>(U u, Func<U, bool> f) { return f(u); }
        public bool LolMethod9a<U>(U u) {return true; }

        public bool LolMethod10<U, S>(U u, Func<U, S> f) where S : struct { throw new NotImplementedException(); }
        public bool LolMethod10a<U, S>(U u, Func<U, S?> f) where S : struct { return f(u) != null; }

        public bool LolMethod11<U, S>(U u, Func<U, bool> f) { throw new NotImplementedException(); }

        public bool LolMethod12(Func<Company, double> f) { return f(this) > 3; }
    }

    public class TypeA { }
    public class TypeB : TypeA { }
    public class TypeC : TypeA { }
    public class TypeE : TypeC { public static implicit operator TypeE(TypeF typec) { return null; } }
    public class TypeF : TypeC { public static implicit operator TypeF(TypeE typec) { return null; } }
}
