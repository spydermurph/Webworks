namespace Webworks.DTOs.Requests
{
    public class CreateBlogPostRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public long CategoryId { get; set; }
    }
}