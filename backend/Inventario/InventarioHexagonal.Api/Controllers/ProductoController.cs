
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventarioHexagonal.Application.Services;
using InventarioHexagonal.Domain.Entities;
using InventarioHexagonal.Api.DTOs;

namespace InventarioHexagonal.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/productos")]
public class ProductoController : ControllerBase
{
    private readonly ProductoService _productoService;
    private readonly ILogger<ProductoController> _logger;

    public ProductoController(
        ProductoService productoService,
        ILogger<ProductoController> logger)
    {
        _productoService = productoService;
        _logger = logger;
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
        {
            _logger.LogWarning(
                "No se encontró el producto {IdProducto}",
                id);

            return NotFound(new
            {
                mensaje = "Producto no encontrado"
            });
        }

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

        _logger.LogInformation(
            "Producto creado correctamente. IdProducto: {IdProducto}",
            nuevoProducto.IdProducto);

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
            Descripcion = request.Descripcion,
            Estado = request.Estado
        };

        var actualizado = await _productoService.UpdateAsync(producto);

        if (!actualizado)
        {
            _logger.LogWarning(
                "No se pudo actualizar. Producto {IdProducto} no encontrado",
                id);

            return NotFound(new
            {
                mensaje = "Producto no encontrado"
            });
        }

        _logger.LogInformation(
            "Producto {IdProducto} actualizado correctamente. Estado: {Estado}",
            id,
            request.Estado);

        return Ok(new
        {
            mensaje = "Producto actualizado correctamente"
        });
    }

    // DELETE: api/productos/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var eliminado = await _productoService.DeleteAsync(id);

        if (!eliminado)
        {
            _logger.LogWarning(
                "No se pudo eliminar. Producto {IdProducto} no encontrado",
                id);

            return NotFound(new
            {
                mensaje = "Producto no encontrado"
            });
        }

        _logger.LogInformation(
            "Producto {IdProducto} eliminado lógicamente",
            id);

        return Ok(new
        {
            mensaje = "Producto eliminado correctamente"
        });
    }
}
