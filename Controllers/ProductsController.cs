using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projectauth.Models;
using Projectauth.Services;
using System.Security.Claims;

namespace Projectauth.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Only authenticated users can access
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service)
    {
        _service = service;
    }

    // 🔐 Create: Admin Only
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Product product)
    {
        product.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
        await _service.Create(product);
        return Ok("Product created.");
    }

    // 👁 Read: Any authenticated user
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    // 🔄 Update: Admin Only
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(string id, Product product)
    {
        var existing = await _service.GetById(id);
        if (existing == null) return NotFound("Product not found");

        product.Id = id;
        await _service.Update(id, product);
        return Ok("Product updated.");
    }

    // ❌ Delete: Admin Only
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        var existing = await _service.GetById(id);
        if (existing == null) return NotFound("Product not found");

        await _service.Delete(id);
        return Ok("Product deleted.");
    }
}
