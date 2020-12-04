using FreesqlDemo.API.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreesqlDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public IActionResult Get()
        {
            //_context.Add(new User
            //{
            //    Age = 18,
            //    Name = "张三",
            //    Books = new List<Book>
            //    {
            //        new Book{
            //            Auther = "李四",
            //            Name = "Kali从入门到放弃",
            //            UserId = 1
            //        },
            //        new Book{
            //            Auther = "王五",
            //            Name = "C#从入门到放弃",
            //            UserId = 1
            //        }
            //    }
            //});

            //_context.SaveChanges();

            var user = this._context.User.Select.IncludeMany(d => d.Books, then => then.Where(sub => sub.Id == 1)).ToList().Select(d => new
            {
                d.Id,
                d.Name,
                d.Age,
                Books = d.Books.Select(b => new
                {
                    b.Name,
                    b.Id,
                    b.Auther,
                    b.UserId
                })
            }).ToList();
            var result = new { state = 1, data = user };
            return new JsonResult(result);
        }
    }
}
