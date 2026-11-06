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
