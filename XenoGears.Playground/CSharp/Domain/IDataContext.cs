using System.Linq;

namespace XenoGears.Playground.CSharp.Domain
{
    public interface IDataContext
    {
        IQueryable<Company> Companies { get; }
        IQueryable<Employee> Employees { get; }
    }
}
