using ToDoList.Models;

namespace ToDoList.Interfaces
{
    public interface IBlogService 
    {
        List<Blog> GetAll();
        List<Blog> GetBlogsByUserId(string userId);
        Blog GetBlogById(int id, string userId);
        void AddBlog(Blog todo, string userId);
        void UpdateBlog(Blog todo, string userId);
        void DeleteBlog(int id, string userId);


    }
}
