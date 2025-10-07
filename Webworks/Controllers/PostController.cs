using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webworks.Context;
using Webworks.Contracts;
using Webworks.DTOs;
using Webworks.DTOs.Repsonses;
using Webworks.DTOs.Requests;

namespace Webworks.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IBlogService blogService;

        public PostController(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        /*[HttpGet]
        public async Task<ActionResult<List<BlogPostDTO>>> GetPosts()
        {
            var posts = await blogService.GetAllPostsAsync();

            return posts;
        }*/

        [HttpGet]
        public async Task<ActionResult<PagedResult<BlogPostDTO>>> GetPagedPosts([FromQuery] PagedPostsRequest pagedPostsRequest)
        {
            var posts = await blogService.GetPagedPostsAsync(pagedPostsRequest);
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPostDTO>> GetPost(int id)
        {
            var posts = await blogService.GetPostByIdAsync(id);
            return Ok(posts);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            var categories = await blogService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest createCategory)
        {
            await blogService.CreateCategory(createCategory);
            return Ok();
        }
    }
}
