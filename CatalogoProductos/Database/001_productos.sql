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
