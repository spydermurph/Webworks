using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webworks.Context;
using Webworks.Contracts;
using Webworks.DTOs;

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
        public async Task<ActionResult<PagedResult<BlogPostDTO>>> GetPagedPosts([FromQuery] QueryParameters queryParameters)
        {
            var posts = await blogService.GetPagedPostsAsync(queryParameters);
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PagedResult<BlogPostDTO>>> GetPost(int id)
        {
            var posts = await blogService.GetPostByIdAsync(id);
            return Ok(posts);
        }
    }
}
