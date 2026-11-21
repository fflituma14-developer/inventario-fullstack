using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using InventarioHexagonal.Api.DTOs;
using InventarioHexagonal.Application.Services;
using InventarioHexagonal.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace InventarioHexagonal.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/lotes")]
public class LoteProductoController : ControllerBase
{
    private readonly LoteProductoService _service;

    public LoteProductoController(LoteProductoService service)
    {
        _service = service;
    }

    // GET: api/lotes
    [HttpGet]
    public async Task<ActionResult<List<LoteProducto>>> GetAllAsync()
    {
        var lotes = await _service.GetAllAsync();

        return Ok(lotes);
    }

    // GET: api/lotes/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LoteProducto>> GetByIdAsync(int id)
    {
        var lote = await _service.GetByIdAsync(id);

        if (lote == null)
            return NotFound(new { mensaje = "Lote no encontrado" });

        return Ok(lote);
    }

    // POST: api/lotes
    [HttpPost]
    public async Task<ActionResult<LoteProducto>> CreateAsync(
        [FromBody] CrearLoteProductoRequest request)
    {
        var lote = new LoteProducto
        {
            IdProducto = request.IdProducto,
            IdProveedor = request.IdProveedor,
            NumeroLote = request.NumeroLote,
            PrecioCompra = request.PrecioCompra,
            CantidadStock = request.CantidadStock
        };

        try
        {
            var nuevoLote = await _service.CreateAsync(lote);
            return StatusCode(201, nuevoLote);

            /*return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = nuevo.IdLote },
                nuevo
            );*/
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx &&
                  (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            return Conflict(new
            {
                mensaje = "Ya existe un registro con esos datos."
            });
        }
    }
}