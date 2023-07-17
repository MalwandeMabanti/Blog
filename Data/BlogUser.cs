using Microsoft.AspNetCore.Identity;

namespace ToDoList.Data
{
    public class BlogUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
