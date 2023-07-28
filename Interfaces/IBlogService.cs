using ToDoList.Models;

namespace ToDoList.Interfaces
{
    public interface IBlogService 
    {
        List<Blogs> GetAll();
        List<Blogs> GetBlogsByUserId(string userId);
        Blogs GetBlogById(int id, string userId);
        void AddBlog(Blogs todo, string userId);
        void UpdateBlog(Blogs todo, string userId);
        void DeleteBlog(int id, string userId);


    }
}
