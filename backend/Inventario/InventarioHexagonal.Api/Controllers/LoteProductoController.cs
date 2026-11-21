
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using InventarioHexagonal.Api.DTOs;
using InventarioHexagonal.Application.Services;
using InventarioHexagonal.Domain.Entities;

namespace InventarioHexagonal.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/lotes")]
public class LoteProductoController : ControllerBase
{
    private readonly LoteProductoService _service;
    private readonly ILogger<LoteProductoController> _logger;

    public LoteProductoController(
        LoteProductoService service,
        ILogger<LoteProductoController> logger)
    {
        _service = service;
        _logger = logger;
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
        {
            _logger.LogWarning(
                "No se encontró el lote {IdLote}",
                id);

            return NotFound(new
            {
                mensaje = "Lote no encontrado"
            });
        }

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

            _logger.LogInformation(
                "Lote {IdLote} registrado correctamente. Producto: {IdProducto}, Proveedor: {IdProveedor}",
                nuevoLote.IdLote,
                nuevoLote.IdProducto,
                nuevoLote.IdProveedor);

            return StatusCode(201, nuevoLote);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                "Datos inválidos al registrar lote: {Mensaje}",
                ex.Message);

            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                "Conflicto al registrar lote: {Mensaje}",
                ex.Message);

            return Conflict(new
            {
                mensaje = ex.Message
            });
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx &&
                  (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            _logger.LogWarning(
                "Intento de registrar lote duplicado. Producto: {IdProducto}, Proveedor: {IdProveedor}, NumeroLote: {NumeroLote}",
                lote.IdProducto,
                lote.IdProveedor,
                lote.NumeroLote);

            return Conflict(new
            {
                mensaje = "Ya existe un registro con esos datos."
            });
        }
    }
}
