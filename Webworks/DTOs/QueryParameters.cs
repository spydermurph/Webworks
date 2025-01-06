namespace Webworks.DTOs
{
    public class QueryParameters
    {
        const int maxPageSize = 50;
        public int StartIndex { get; set; }
        public int PageNumber { get; set; }
        public int _pageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > maxPageSize ? maxPageSize : value;
        }
    }
}
