using InventarioHexagonal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace InventarioHexagonal.Infrastructure.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {        
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Producto> Productos { get; set; } = null!;    
    public DbSet<Proveedor> Proveedores { get; set; } = null!;
    public DbSet<LoteProducto> LotesProductos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Producto");
            entity.HasKey(p => p.IdProducto);
            entity.Property(p => p.FechaCreacion).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.ToTable("Proveedor");
            entity.HasKey(p => p.IdProveedor);
        });
    
        modelBuilder.Entity<LoteProducto>(entity =>
        {
            entity.ToTable("LoteProducto");

            entity.HasKey(l => l.IdLote);

            entity.Property(l => l.NumeroLote)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(l => l.PrecioCompra)
                .HasPrecision(18, 2);

            entity.Property(l => l.CantidadStock)
                .HasDefaultValue(0);

            entity.Property(l => l.FechaIngreso)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");

            entity.Property(l => l.Estado)
                .HasColumnType("char(1)")
                .HasDefaultValue("A");

            entity.Property(l => l.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");

            entity.Property(l => l.FechaActualizacion)
                .HasColumnType("datetime");

            entity.Property(l => l.FechaEliminacion)
                .HasColumnType("datetime");

            // Relación con Producto
            entity.HasOne<Producto>()
                .WithMany()
                .HasForeignKey(l => l.IdProducto)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación con Proveedor
            entity.HasOne<Proveedor>()
                .WithMany()
                .HasForeignKey(l => l.IdProveedor)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario", "dbo");
            entity.HasKey(u => u.IdUsuario);
            entity.Property(u => u.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(u => u.NombreCompleto)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(u => u.Correo)
                .HasMaxLength(254)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(u => u.PasswordHash)
                .HasMaxLength(512)
                .IsRequired();

            entity.Property(u => u.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("A");

            entity.Property(u => u.IntentosFallidos)
                .HasDefaultValue(0);

            entity.Property(u => u.BloqueadoHasta)
                .HasColumnType("datetime2(0)");

            entity.Property(u => u.FechaCreacion)
                .HasColumnType("datetime2(0)")
                .HasDefaultValueSql("SYSDATETIME()");

            entity.Property(u => u.FechaActualizacion)
                .HasColumnType("datetime2(0)");

            entity.HasIndex(u => u.NombreUsuario)
                .IsUnique();

            entity.HasIndex(u => u.Correo)
                .IsUnique();
        });
    }

    

}