using InventarioHexagonal.Application.Interfaces;
using InventarioHexagonal.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace InventarioHexagonal.Infrastructure.Services;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<Usuario> _hasher = new();

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(new Usuario(), password);
    }

    public bool VerifyPassword(string hash, string password)
    {
        var result = _hasher.VerifyHashedPassword(
            new Usuario(),
            hash,
            password
        );

        return result != PasswordVerificationResult.Failed;
    }
}