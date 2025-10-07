namespace Webworks.DTOs.Requests
{
    public class PagedPostsRequest : QueryParameters
    {
        public long? CategoryId { get; set; }
        public string? Title { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
