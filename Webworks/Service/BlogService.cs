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

            //var posts = await context.BlogPosts
            //    .Include(p => p.Category)
            //    .Include(p => p.BlogPostContents)
            //    .Select(p => new BlogPostDTO
            //    {
            //        PostId = p.PostId,
            //        Title = p.Title,
            //        Content = p.Content,
            //        PublicationDate = p.PublicationDate,
            //        CategoryId = p.CategoryId,
            //        CategoryName = p.Category.CategoryName
            //    })
            //    .ToListAsync();

            //return posts;
            return null;
        }

        public async Task<PagedResult<BlogPostExcerptDTO>> GetPagedPostsAsync(PagedPostsRequest pagedPostsRequest)
        {
            var query = context.BlogPosts
                        .Include(p => p.Category)
                        .Include(p => p.BlogPostContents)
                            .ThenInclude(c => c.Language)
                        .AsQueryable();


            if (pagedPostsRequest.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == pagedPostsRequest.CategoryId.Value);
            }

            


            var totalSize = await context.BlogPosts.CountAsync();
            var result = await query
                .Skip(pagedPostsRequest.StartIndex)
                .Take(pagedPostsRequest.PageSize)
                .Select(p => new {
                    p.Id,
                    p.PublicationDate,
                    p.CategoryId,
                    CategoryName = p.Category.Name,
                    Contents = p.BlogPostContents
                })
                .ToListAsync();

            var items = result.Select(p =>
            {
                var content = p.Contents
                    .OrderByDescending(c => c.Language.LanguageCode == pagedPostsRequest.LanguageCode)
                    .ThenByDescending(c => c.Language.LanguageCode == "en")
                    .FirstOrDefault();

                return new BlogPostExcerptDTO
                {
                    PostId = p.Id,
                    Title = content?.Title ?? string.Empty,
                    Excerpt = content?.Excerpt ?? string.Empty,
                    PublicationDate = p.PublicationDate,
                    CategoryId = p.CategoryId,
                    CategoryName = p.CategoryName
                };
            }).ToList();

            return new PagedResult<BlogPostExcerptDTO>
            {
                Items = items,
                PageNumber = pagedPostsRequest.PageNumber,
                RecordNumber = pagedPostsRequest.PageSize,
                TotalCount = totalSize
            };
        }

        public async Task<BlogPostDTO?> GetPostByIdAsync(long postId, string languageCode = "en")
        {
            /*var blogPost = await (from post in context.BlogPosts
                             join category in context.Categories
                                on post.CategoryId equals category.Id
                             join content in context.BlogPostContents
                                on post.Id equals content.BlogPostId
                             where post.Id == postId
                             select new BlogPostDTO
                             {
                                 PostId = post.Id,
                                 Title = post.Title,
                                 Content = post.Content,
                                 PublicationDate = post.PublicationDate,
                                 CategoryId = post.CategoryId,
                                 CategoryName = category.Name
                             }).FirstAsync();*/

            var post = await context.BlogPosts
                .Include(p => p.Category)
                .Include(p => p.BlogPostContents)
                    .ThenInclude(c => c.Language)
                .Where(p => p.Id == postId)
                .Select(p => new
                {
                    Post = p,
                    Content = p.BlogPostContents
                        .Where(c => c.Language.LanguageCode == languageCode)
                        .FirstOrDefault()
                        ?? p.BlogPostContents
                            .Where(c => c.Language.LanguageCode == "en")
                            .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (post is null)
            {
                return null;
            }

            return new BlogPostDTO
            {
                PostId = post.Post.Id,
                Title = post.Content?.Title ?? string.Empty,
                Content = post.Content?.Content ?? string.Empty,
                PublicationDate = post.Post.PublicationDate,
                CategoryId = post.Post.CategoryId,
                CategoryName = post.Post.Category.Name
            };
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await context.Categories.Select(c => new CategoryDTO
            {
                CategoryId = c.Id,
                CategoryName = c.Name
            }).ToListAsync();

            return categories;
        }

        public async Task<Task> CreateCategory(CreateCategoryRequest createCategory)
        {
            var category = new Models.Category
            {
                Name = createCategory.CategoryName
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return Task.CompletedTask;
        }
    }
}
