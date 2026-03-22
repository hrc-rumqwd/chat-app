namespace ChatApp.Infrastructure.Extensions
{
    public static class CollectionExtensions
    {
        public static List<T> ToPaginatedList<T>(this List<T> list, int pageIndex, int pageSize)
        {
            return list
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
