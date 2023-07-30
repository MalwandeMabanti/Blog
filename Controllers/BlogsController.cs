//using Microsoft.AspNetCore.Components;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.WebSockets;
using System.Security.Claims;

using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.Controllers
{

    
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBlogService _blogService;
        private readonly IAzureBlobService _azureBlobService;

        public BlogsController(IConfiguration configuration, IBlogService blogService, IAzureBlobService azureBlobService) 
        {
            _configuration = configuration;
            _blogService = blogService;
            _azureBlobService = azureBlobService;
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<Blogs>> GetAll() 
        {
            var allBlogs = _blogService.GetAll();

            return Ok(allBlogs);
        }



        // GET: api/blogs
        [Authorize]
        [HttpGet("myblogs")]
        public ActionResult<IEnumerable<Blogs>> GetBlogs()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //string userId = "User.FindFirstValue(ClaimTypes.NameIdentifier)";
            var blogs = _blogService.GetBlogsByUserId(userId);

            return Ok(blogs);
        }


        // GET: api/blogs/{id}
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Blogs> GetBlog(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var blog = _blogService.GetBlogById(id, userId);

            if (blog == null)
            {
                return NotFound();
            }

            return Ok(blog);
        }

        // PUT: api/blogs/{id}
        [Authorize]
        [HttpPost, Consumes("multipart/form-data")]
        public async Task<ActionResult<Blogs>> PostBlog([FromForm]BlogViewModel blogViewModel)
        {
            if (!this.ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }


            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //string userId = "User.FindFirstValue(ClaimTypes.NameIdentifier)";
            string imageUrl = "";

            string containerName = _configuration.GetValue<string>("Azure:BlobStorage:ContainerName");

            if (blogViewModel.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(blogViewModel.Image.FileName);
                imageUrl = await _azureBlobService.UploadFileAsync(containerName, blogViewModel.Image, fileName);
            }


            Blogs blog = new Blogs
            {
                Title = blogViewModel.Title,
                Description = blogViewModel.Description,
                UserId = userId,
                ImageUrl = imageUrl
            };

            _blogService.AddBlog(blog, userId);

            
            blog.UserId = "";

            return CreatedAtAction(nameof(GetBlog), new { id = blog.Id }, blog);
        }

        // PUT: api/blogs/{id}
        [Authorize]
        [HttpPut("{id}"), Consumes("multipart/form-data")]
        public async Task<IActionResult> PutBlog(int id, [FromForm]BlogViewModel blogViewModel)
        {
            
            

            if (id != blogViewModel.Id) 
            {
                return BadRequest();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //string userId = "User.FindFirstValue(ClaimTypes.NameIdentifier)";


            string? imageUrl = null;

            
            string containerName = _configuration.GetValue<string>("Azure:BlobStorage:ContainerName");

            if (blogViewModel.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(blogViewModel.Image.FileName);
                imageUrl = await _azureBlobService.UploadFileAsync(containerName, blogViewModel.Image, fileName);
            }

          

            Blogs blog = new Blogs
            {
                Id = blogViewModel.Id,
                Title = blogViewModel.Title,
                Description = blogViewModel.Description,
                UserId = userId,
                ImageUrl = imageUrl
            };


            _blogService.UpdateBlog(blog, userId);

            blog.UserId = "";

            return NoContent();
        }

        // DELETE: api/blogs/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _blogService.DeleteBlog(id, userId);

            return NoContent();
        }


    }
}
