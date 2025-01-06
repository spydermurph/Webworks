using Webworks.DTOs;

namespace Webworks.Contracts
{
    public interface IBlogService
    {
        public Task<List<BlogPostDTO>> GetAllPostsAsync();
        public Task<BlogPostDTO> GetPostByIdAsync(long postId);
        public Task<PagedResult<BlogPostDTO>> GetPagedPostsAsync(QueryParameters queryParameters);
    }
}
