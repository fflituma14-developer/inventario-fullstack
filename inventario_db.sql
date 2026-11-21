-- Crear la base de datos si no existe
IF DB_ID('InventarioDB') IS NULL
BEGIN
    CREATE DATABASE InventarioDB;
END
GO

-- Seleccionar la base de datos
USE InventarioDB;
GO

CREATE TABLE dbo.Usuario
(
    IdUsuario INT IDENTITY(1,1) NOT NULL,

    NombreUsuario VARCHAR(50) NOT NULL,
    NombreCompleto NVARCHAR(150) NOT NULL,
    Correo VARCHAR(254) NOT NULL,

    PasswordHash NVARCHAR(512) NOT NULL,

    Estado CHAR(1) NOT NULL
        CONSTRAINT DF_Usuario_Estado DEFAULT 'A',

    IntentosFallidos INT NOT NULL
        CONSTRAINT DF_Usuario_IntentosFallidos DEFAULT 0,

    BloqueadoHasta DATETIME2(0) NULL,

    FechaCreacion DATETIME2(0) NOT NULL
        CONSTRAINT DF_Usuario_FechaCreacion DEFAULT SYSDATETIME(),

    FechaActualizacion DATETIME2(0) NULL,

    CONSTRAINT PK_Usuario
        PRIMARY KEY (IdUsuario),

    CONSTRAINT UQ_Usuario_NombreUsuario
        UNIQUE (NombreUsuario),

    CONSTRAINT UQ_Usuario_Correo
        UNIQUE (Correo),

    CONSTRAINT CK_Usuario_Estado
        CHECK (Estado IN ('A', 'I', 'E')),

    CONSTRAINT CK_Usuario_IntentosFallidos
        CHECK (IntentosFallidos >= 0)
);




CREATE TABLE Producto (
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,

    Nombre VARCHAR(150) NOT NULL,
    Descripcion VARCHAR(500) NULL,

    -- A = Activo
    -- I = Inactivo
    -- E = Eliminado lógicamente
    Estado CHAR(1) NOT NULL DEFAULT 'A',

    -- Auditoría
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NULL,
    FechaEliminacion DATETIME NULL,

    UsuarioCreacion INT NULL,
    UsuarioActualizacion INT NULL,
    UsuarioEliminacion INT NULL,

    CONSTRAINT CK_Producto_Estado
        CHECK (Estado IN ('A', 'I', 'E'))
);

INSERT INTO Producto (Nombre, Descripcion)
VALUES
('Monitor Samsung 4K', 'Monitor de 50 pulgadas'),
('Laptop Lenovo', 'Laptop Core i7'),
('Teclado Logitech', 'Teclado inalámbrico');

CREATE TABLE Proveedor (
    IdProveedor INT IDENTITY(1,1) PRIMARY KEY,

    RazonSocial VARCHAR(200) NOT NULL,
    NombreComercial VARCHAR(150) NULL,
    RUC VARCHAR(13) NOT NULL,

    Correo VARCHAR(150) NULL,
    Telefono VARCHAR(20) NULL,
    Direccion VARCHAR(250) NULL,

    -- A = Activo
    -- I = Inactivo
    -- E = Eliminado lógicamente
    Estado CHAR(1) NOT NULL DEFAULT 'A',

    -- Auditoría
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NULL,
    FechaEliminacion DATETIME NULL,

    UsuarioCreacion INT NULL,
    UsuarioActualizacion INT NULL,
    UsuarioEliminacion INT NULL,

    -- Restricciones
    CONSTRAINT UQ_Proveedor_RUC UNIQUE (RUC),

    CONSTRAINT CK_Proveedor_Estado
        CHECK (Estado IN ('A', 'I', 'E')),

    CONSTRAINT CK_Proveedor_RUC
        CHECK (LEN(RUC) = 13 AND RUC NOT LIKE '%[^0-9]%')
);


INSERT INTO Proveedor
    (RazonSocial, NombreComercial, RUC, Correo, Telefono)
VALUES
    ('Logitec S.A.', 'Logitec',
     '1790012345001', 'ventas@proveedora.com', '0991111111'),

    ('Distribuidora Electronica B S.A.', 'Electronica SYS',
     '1790012345002', 'ventas@proveedorb.com', '0992222222'),

    ('Importadora Digital C S.A.', 'Xtratech',
     '1790012345003', 'ventas@proveedorc.com', '0993333333');
    
    
CREATE TABLE LoteProducto (
    IdLote INT IDENTITY(1,1) PRIMARY KEY,

    -- Relaciones
    IdProducto INT NOT NULL,
    IdProveedor INT NOT NULL,

    -- Información del lote
    NumeroLote VARCHAR(50) NOT NULL,
    PrecioCompra DECIMAL(18,2) NOT NULL,
    CantidadStock INT NOT NULL DEFAULT 0,
    FechaIngreso DATETIME NOT NULL DEFAULT GETDATE(),

    -- A = Activo
    -- I = Inactivo
    -- E = Eliminado lógicamente
    Estado CHAR(1) NOT NULL DEFAULT 'A',

    -- Auditoría
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NULL,
    FechaEliminacion DATETIME NULL,

    UsuarioCreacion INT NULL,
    UsuarioActualizacion INT NULL,
    UsuarioEliminacion INT NULL,

    -- Claves foráneas
    CONSTRAINT FK_LoteProducto_Producto
        FOREIGN KEY (IdProducto)
        REFERENCES Producto(IdProducto),

    CONSTRAINT FK_LoteProducto_Proveedor
        FOREIGN KEY (IdProveedor)
        REFERENCES Proveedor(IdProveedor),

    -- Validaciones
    CONSTRAINT CK_LoteProducto_Estado
        CHECK (Estado IN ('A', 'I', 'E')),

    CONSTRAINT CK_LoteProducto_PrecioCompra
        CHECK (PrecioCompra >= 0),

    CONSTRAINT CK_LoteProducto_CantidadStock
        CHECK (CantidadStock >= 0),

    -- Evitar repetir un número de lote para
    -- el mismo producto y proveedor
    CONSTRAINT UQ_LoteProducto
        UNIQUE (IdProducto, IdProveedor, NumeroLote)
);


