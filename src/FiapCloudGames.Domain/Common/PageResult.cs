
namespace FiapCloudGames.Domain.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Itens { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItens { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItens / (double)PageSize);

        
        public PagedResult(IEnumerable<T> itens, int pageNumber, int pageSize, int totalItems)
        {
            Itens = itens;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItens = totalItems;
        }

        public PagedResult()
        {
            
        }
    }
}
