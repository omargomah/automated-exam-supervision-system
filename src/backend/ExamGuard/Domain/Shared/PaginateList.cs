//using Mapster;
namespace Domain.Shared
{
    public class PaginateList<T>
    {
        private PaginateList(int page, int pageSize, int totalCount, List<T> items)
        {
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
            Items = items;
        }

        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public List<T> Items { get; }
        public bool ThereIsNextPage => Page * PageSize < TotalCount;
        public bool ThereIsPreviousPage => Page != 1;

        //public PaginateList<TDistention> MapToAnotherType<TDistention>()
        //    =>  new(Page, PageSize, TotalCount, Items.Adapt<List<TDistention>>());

        public static PaginateList<T> CreateAsync(List<T> items,int totalCount , int page , int pageSize)
            =>  new (page, pageSize, totalCount, items);
    }

}
