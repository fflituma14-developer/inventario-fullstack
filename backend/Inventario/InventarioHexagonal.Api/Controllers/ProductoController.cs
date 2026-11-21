using Microsoft.AspNetCore.Mvc;
using InventarioHexagonal.Application.Services;
using InventarioHexagonal.Domain.Entities;
using InventarioHexagonal.Api.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace InventarioHexagonal.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/productos")]
public class ProductoController : ControllerBase
{
    private readonly ProductoService _productoService;

    public ProductoController(ProductoService productoService)
    {
        _productoService = productoService;
    }

    // GET: api/productos
    [HttpGet]
    public async Task<ActionResult<List<Producto>>> GetAllAsync()
    {
        var productos = await _productoService.GetAllAsync();

        return Ok(productos);
    }

    // GET: api/productos/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Producto>> GetByIdAsync(int id)
    {
        var producto = await _productoService.GetByIdAsync(id);

        if (producto == null)
            return NotFound(new { mensaje = "Producto no encontrado" });

        return Ok(producto);
    }

    // POST: api/productos
    [HttpPost]
    public async Task<ActionResult<Producto>> CreateAsync(
        [FromBody] CrearProductoRequest request)
    {
        var producto = new Producto
        {
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            Estado = "A"
        };

        var nuevoProducto = await _productoService.CreateAsync(producto);

        return StatusCode(201, nuevoProducto);
    }

    // PUT: api/productos/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] ActualizarProductoRequest request)
    {
        var producto = new Producto
        {
            IdProducto = id,
            Nombre = request.Nombre,
            Descripcion = request.Descripcion
        };

        var actualizado = await _productoService.UpdateAsync(producto);

        if (!actualizado)
            return NotFound(new { mensaje = "Producto no encontrado" });

        return Ok(new { mensaje = "Producto actualizado correctamente" });
    }

    // DELETE: api/productos/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var eliminado = await _productoService.DeleteAsync(id);

        if (!eliminado)
            return NotFound(new { mensaje = "Producto no encontrado" });

        return Ok(new { mensaje = "Producto eliminado correctamente" });
    }
}