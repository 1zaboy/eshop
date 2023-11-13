using Cart.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cart.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<ActionResult<List<string>>> Get()
    {
        return Ok(await _cartService.GetAll());
    }
    
    [HttpPut]
    public async Task<ActionResult<List<string>>> Save(string productCode)
    {
        await _cartService.Save(productCode);
        return Ok();
    }
    
    [HttpDelete]
    public async Task<ActionResult<List<string>>> Delete(string productCode)
    {
        await _cartService.Delete(productCode);
        return Ok();
    }
}