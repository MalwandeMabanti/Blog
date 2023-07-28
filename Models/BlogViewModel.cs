using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class BlogViewModel
    {

        //[Required]
        public int Id { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string? Title { get; set; }

        //[Required]
        //[StringLength(15000, ErrorMessage = "Body cannot be longer than 15000 characters.")]
        public string? Description { get; set; }

        //[Required]
        public IFormFile? Image { get; set; }
    }
}
