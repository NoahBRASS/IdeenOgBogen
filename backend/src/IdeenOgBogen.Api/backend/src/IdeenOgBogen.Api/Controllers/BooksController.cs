using Microsoft.AspNetCore.Mvc;

namespace IdeenOgBogen.Api.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    [HttpGet]
    public IActionResult GetBooks()
    {
        var books = new[]
        {
            new
            {
                Id = 1,
                Title = "Harry Potter",
                Price = 199
            },
            new
            {
                Id = 2,
                Title = "The Hobbit",
                Price = 149
            }
        };

        return Ok(books);
    }
}