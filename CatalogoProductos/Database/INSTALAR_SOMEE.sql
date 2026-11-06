-- Ejecutar sobre la base asignada por Somee.
-- No crea ni elimina bases. Conserva tablas y registros existentes.
SET XACT_ABORT ON;
BEGIN TRANSACTION;
-- Seleccionar la base catalogo_productos antes de ejecutar.
IF OBJECT_ID(N'dbo.productos', N'U') IS NULL
BEGIN
 CREATE TABLE dbo.productos (
  id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_productos PRIMARY KEY,
  nombre NVARCHAR(100) NOT NULL,
  categoria NVARCHAR(50) NOT NULL,
  precio DECIMAL(10,2) NOT NULL CONSTRAINT CK_productos_precio CHECK(precio>=0),
  existencias INT NOT NULL CONSTRAINT CK_productos_existencias CHECK(existencias>=0),
  descripcion NVARCHAR(300) NULL,
  CONSTRAINT CK_productos_nombre CHECK(LEN(LTRIM(RTRIM(nombre)))>0),
  CONSTRAINT CK_productos_categoria CHECK(LEN(LTRIM(RTRIM(categoria)))>0)
 );
END;

-- Ejecutar en catalogo_productos. No modifica productos existentes.
IF OBJECT_ID(N'dbo.usuarios',N'U') IS NULL
CREATE TABLE dbo.usuarios (
 id INT IDENTITY(1,1) PRIMARY KEY,
 nombre NVARCHAR(80) NOT NULL,
 correo NVARCHAR(160) NOT NULL UNIQUE,
 password_hash VARBINARY(32) NOT NULL,
 password_salt VARBINARY(16) NOT NULL,
 iteraciones INT NOT NULL,
 intentos_fallidos INT NOT NULL DEFAULT 0,
 bloqueado_hasta DATETIME2 NULL,
 creado_en DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- Insertar únicamente ejemplos que todavía no existan.
-- No se borran ni se sobrescriben productos registrados.
INSERT INTO dbo.productos(nombre,categoria,precio,existencias,descripcion)
SELECT ejemplo.nombre,ejemplo.categoria,ejemplo.precio,ejemplo.existencias,ejemplo.descripcion
FROM (VALUES
 (N'Cuaderno',N'Útiles escolares',1500.00,25,N'Cuaderno de 100 hojas'),
 (N'Lápiz',N'Útiles escolares',300.00,50,N'Lápiz de grafito'),
 (N'Lapicero azul',N'Útiles escolares',500.00,40,N'Lapicero de tinta azul'),
 (N'Borrador',N'Útiles escolares',400.00,30,N'Borrador blanco'),
 (N'Regla',N'Útiles escolares',700.00,20,N'Regla de 30 centímetros'),
 (N'Mouse',N'Tecnología',6500.00,12,N'Mouse USB'),
 (N'Teclado',N'Tecnología',12000.00,8,N'Teclado USB en español'),
 (N'Memoria USB',N'Tecnología',5000.00,15,N'Memoria de 32 GB'),
 (N'Botella reutilizable',N'Accesorios',4500.00,18,N'Botella de 750 ml'),
 (N'Mochila',N'Accesorios',18000.00,6,N'Mochila para uso diario')
) AS ejemplo(nombre,categoria,precio,existencias,descripcion)
WHERE NOT EXISTS(
 SELECT 1 FROM dbo.productos p
 WHERE p.nombre=ejemplo.nombre AND p.categoria=ejemplo.categoria
);

COMMIT TRANSACTION;
SELECT COUNT(*) AS cantidad_productos FROM dbo.productos;
