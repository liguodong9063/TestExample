using System.Collections.Generic;

namespace TestExample.Model
{
    public class PagedData<T>
    {
        public PagedData(IList<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }

        public static PagedData<T> Default => new PagedData<T>(new List<T>(), 0);

        public IList<T> Items { get; set; }

        public int TotalCount { get; set; }
    }
}
