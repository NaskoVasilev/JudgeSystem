using System.Linq;

namespace JudgeSystem.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IQueryable<T> GetPage<T>(this IQueryable<T> collection, int page, int entitesPerPage) =>
            collection.Skip((page - 1) * entitesPerPage).Take(entitesPerPage);
    }
}
