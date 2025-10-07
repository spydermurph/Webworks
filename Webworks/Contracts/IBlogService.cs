using Webworks.DTOs;
using Webworks.DTOs.Repsonses;
using Webworks.DTOs.Requests;

namespace Webworks.Contracts
{
    public interface IBlogService
    {
        public Task<List<BlogPostDTO>> GetAllPostsAsync();
        public Task<BlogPostDTO> GetPostByIdAsync(long postId);
        public Task<PagedResult<BlogPostDTO>> GetPagedPostsAsync(PagedPostsRequest pagedPostsRequest);
        public Task <List<CategoryDTO>> GetAllCategoriesAsync();
        public Task<Task> CreateCategory(CreateCategoryRequest createCategory);
    }
}
