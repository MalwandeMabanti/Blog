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

        public List<Blog> GetAll()
        {
            var cacheKey = "GetAllBlogs";
            if (!cacheService.TryGetValue(cacheKey, out List<Blog> blogs))
            {
                blogs = this.context.Set<Blog>().ToList();
                cacheService.Set(cacheKey, blogs, TimeSpan.FromMinutes(20));
            }

            return blogs;
           
        }


        public List<Blog> GetBlogsByUserId(string userId)
        {
            return this.context.Set<Blog>().Where(todo => todo.UserId == userId).ToList();
        }

        public Blog GetBlogById(int id, string userId)
        {
            return this.context.Set<Blog>().SingleOrDefault(todo => todo.Id == id && todo.UserId == userId);
        }

        public void AddBlog(Blog todo, string userId)
        {
            todo.UserId = userId;
            this.context.Set<Blog>().Add(todo);
            this.context.SaveChanges();
        }

        public void UpdateBlog(Blog todo, string userId)
   {
            var existingTodo = this.context.Set<Blog>().SingleOrDefault(t => t.Id == todo.Id && t.UserId == userId);
            if (todo.ImageUrl == null) 
            {
                todo.ImageUrl = existingTodo?.ImageUrl;
            }

            if (existingTodo != null)
            {
                this.context.Entry(existingTodo).CurrentValues.SetValues(todo);
                this.context.SaveChanges();
            }
        }

        public void DeleteBlog(int id, string userId)
        {
            var todo = this.context.Set<Blog>().SingleOrDefault(t => t.Id == id && t.UserId == userId);
            if (todo != null)
            {
                this.context.Set<Blog>().Remove(todo);
                this.context.SaveChanges();
            }
        }
    }
}
