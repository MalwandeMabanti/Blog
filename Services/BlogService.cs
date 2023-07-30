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
            //var cacheKey = "GetAllBlogs";
            //if (!cacheService.TryGetValue(cacheKey, out List<Blogs> blogs))
            //{
            //    blogs = this.context.Set<Blogs>().ToList();
            //    cacheService.Set(cacheKey, blogs, TimeSpan.FromMinutes(20));
            //}
            List<Blogs> blogs = this.context.Set<Blogs>().ToList();

            return blogs;
           
        }


        public List<Blogs> GetBlogsByUserId(string userId)
        {
            return this.context.Set<Blogs>().Where(todo => todo.UserId == userId).ToList();
        }

        public Blogs GetBlogById(int id, string userId)
        {
            return this.context.Set<Blogs>().SingleOrDefault(todo => todo.Id == id && todo.UserId == userId);
        }

        public void AddBlog(Blogs todo, string userId)
        {
            todo.UserId = userId;
            this.context.Set<Blogs>().Add(todo);
            this.context.SaveChanges();
        }

        public void UpdateBlog(Blogs todo, string userId)
   {
            var existingTodo = this.context.Set<Blogs>().SingleOrDefault(t => t.Id == todo.Id && t.UserId == userId);
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
            var todo = this.context.Set<Blogs>().SingleOrDefault(t => t.Id == id && t.UserId == userId);
            if (todo != null)
            {
                this.context.Set<Blogs>().Remove(todo);
                this.context.SaveChanges();
            }
        }
    }
}
