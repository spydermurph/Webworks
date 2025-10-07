using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using Webworks.Context;
using Webworks.Contracts;
using Webworks.DTOs;
using Webworks.DTOs.Repsonses;
using Webworks.DTOs.Requests;

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

        public async Task<PagedResult<BlogPostDTO>> GetPagedPostsAsync(PagedPostsRequest pagedPostsRequest)
        {
            var query = context.Posts.Include(p => p.Category).AsQueryable();

            if (pagedPostsRequest.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == pagedPostsRequest.CategoryId.Value);
            }


            var totalSize = await context.Posts.CountAsync();
            var items = await query
                .Skip(pagedPostsRequest.StartIndex)
                .Take(pagedPostsRequest.PageSize)
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
                PageNumber = pagedPostsRequest.PageNumber,
                RecordNumber = pagedPostsRequest.PageSize,
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

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await context.Categories.Select(c => new CategoryDTO
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            }).ToListAsync();

            return categories;
        }

        public async Task<Task> CreateCategory(CreateCategoryRequest createCategory)
        {
            var category = new Models.Category
            {
                CategoryName = createCategory.CategoryName
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return Task.CompletedTask;
        }
    }
}
