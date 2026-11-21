using InventarioHexagonal.Domain.Entities;

namespace InventarioHexagonal.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(Usuario usuario);
}