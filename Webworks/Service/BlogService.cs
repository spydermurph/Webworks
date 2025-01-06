using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using Webworks.Context;
using Webworks.Contracts;
using Webworks.DTOs;

namespace Webworks.Service
{
    public class BlogService: IBlogService
    {
        private readonly WebworksContext context;

        public BlogService(WebworksContext context)
        {
            this.context = context;
        }

        public async Task<bool> Exists(int postId)
        {
            var post = await GetPostByIdAsync(postId);
            return post is not null;
        }

        public async Task<List<BlogPostDTO>> GetAllPostsAsync()
        {
            /*var posts = from post in context.Posts 
                        join category in context.Categories
                        on post.CategoryId equals category.CategoryId
                        select new BlogPostDTO
                        {y
                            PostId = post.PostId,
                            Title = post.Title,
                            Content = post.Content,
                            PublicationDate = post.PublicationDate,
                            CategoryId = post.CategoryId,
                            CategoryName = category.CategoryName
                        };*/

            var posts = await context.Posts
                .Include(p => p.Category)
                .Select(p => new BlogPostDTO
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    PublicationDate = p.PublicationDate,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                })
                .ToListAsync();

            return posts;
        }

        public async Task<PagedResult<BlogPostDTO>> GetPagedPostsAsync(QueryParameters queryParameters)
        {
            
            var totalSize = await context.Posts.CountAsync();
            var items = await context.Posts
                .Skip(queryParameters.StartIndex)
                .Take(queryParameters.PageSize)
                .Include(p => p.Category)
                .Select(p => new BlogPostDTO
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    PublicationDate = p.PublicationDate,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                })
                .ToListAsync();

            return new PagedResult<BlogPostDTO>
            {
                Items = items,
                PageNumber = queryParameters.PageNumber,
                RecordNumber = queryParameters.PageSize,
                TotalCount = totalSize
            };
        }

        public async Task<BlogPostDTO> GetPostByIdAsync(long postId)
        {
            var blogPost = await (from post in context.Posts
                             join category in context.Categories
                             on post.CategoryId equals category.CategoryId
                             where post.PostId == postId
                             select new BlogPostDTO
                             {
                                 PostId = post.PostId,
                                 Title = post.Title,
                                 Content = post.Content,
                                 PublicationDate = post.PublicationDate,
                                 CategoryId = post.CategoryId,
                                 CategoryName = category.CategoryName
                             }).FirstAsync();

            return blogPost;
        }
    }
}
