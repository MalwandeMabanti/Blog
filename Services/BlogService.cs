using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext context;
        private readonly ICacheService cacheService;

        public BlogService(ApplicationDbContext context, ICacheService cacheService)
            
        {
            this.context = context;
            this.cacheService = cacheService;
        }

        public List<Blogs> GetAll()
        {
            var cacheKey = "GetAllBlogs";
            if (!cacheService.TryGetValue(cacheKey, out List<Blogs> blogs))
            {
                blogs = this.context.Set<Blogs>().ToList();
                cacheService.Set(cacheKey, blogs, TimeSpan.FromMinutes(20));
            }
           
             
            return blogs;
           
        }


        public List<Blogs> GetBlogsByUserId(string userId)
        {
            return this.context.Set<Blogs>().Where(blog => blog.UserId == userId).ToList();
        }

        public Blogs GetBlogById(int id, string userId)
        {
            return this.context.Set<Blogs>().SingleOrDefault(blog => blog.Id == id && blog.UserId == userId);
        }

        public void AddBlog(Blogs blog, string userId)
        {
            blog.UserId = userId;
            this.context.Set<Blogs>().Add(blog);
            this.context.SaveChanges();
        }

        public void UpdateBlog(Blogs blog, string userId)
   {
            var existingBlog = this.context.Set<Blogs>().SingleOrDefault(t => t.Id == blog.Id && t.UserId == userId);
            if (blog.ImageUrl == null) 
            {
                blog.ImageUrl = existingBlog?.ImageUrl;
            }

            if (existingBlog != null)
            {
                this.context.Entry(existingBlog).CurrentValues.SetValues(blog);
                this.context.SaveChanges();
            }
        }

        public void DeleteBlog(int id, string userId)
        {
            var blog = this.context.Set<Blogs>().SingleOrDefault(t => t.Id == id && t.UserId == userId);
            if (blog != null)
            {
                this.context.Set<Blogs>().Remove(blog);
                this.context.SaveChanges();
            }
        }

        public IEnumerable<Blogs> SearchBlogs(string term)
        {
            return this.context.Blog.Where(b => b.Title.Contains(term)).ToList();
        }

    }
}
