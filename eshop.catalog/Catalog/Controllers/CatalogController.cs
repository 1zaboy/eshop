using Catalog.Models;
using Catalog.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<Book>>> GetBookAsync()
    {
        return Ok(await _catalogService.GetBooksAsync());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBookByIdAsync([FromRoute] int id)
    {
        return Ok(await _catalogService.GetBookByIdAsync(id));
    }

    [HttpPut("")]
    public async Task<IActionResult> AddBookAsync(Book book)
    {
        await _catalogService.AddBookAsync(book);
        return Ok();
    } 
    
    [HttpPost("{id}")]
    public async Task<IActionResult> UpdateBookAsync(int id, Book book)
    {
        await _catalogService.UpdateBookAsync(id, book);
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBookAsync([FromRoute] int id)
    {
        await _catalogService.DeleteBookAsync(id);
        return Ok();
    }
}