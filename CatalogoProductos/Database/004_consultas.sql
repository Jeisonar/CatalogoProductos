-- Consultas demostrativas. No modifican la base.
-- 1. Listar: retorna los productos guardados.
SELECT id,nombre,categoria,precio,existencias,descripcion
FROM dbo.productos ORDER BY id DESC;

-- 2. Buscar y filtrar: con los ejemplos retorna Mouse.
DECLARE @q NVARCHAR(100)=N'Mouse';
DECLARE @categoria NVARCHAR(50)=N'Tecnología';
SELECT id,nombre,categoria,precio,existencias,descripcion
FROM dbo.productos
WHERE (@q=N'' OR CHARINDEX(@q,nombre)>0)
 AND (@categoria=N'' OR categoria=@categoria)
ORDER BY id DESC;

-- 3. Resumir: con los 10 ejemplos, 10 productos, 224 unidades y 536500.00 colones.
SELECT COUNT(*) cantidad,
 COALESCE(SUM(CAST(existencias AS BIGINT)),0) unidades,
 COALESCE(SUM(precio*CAST(existencias AS DECIMAL(19,0))),0) valor
FROM dbo.productos;

-- 4. Agrupar: Accesorios 2/24, Tecnología 3/35, Útiles escolares 5/165.
SELECT categoria,COUNT(*) cantidad,SUM(CAST(existencias AS BIGINT)) unidades
FROM dbo.productos GROUP BY categoria ORDER BY categoria;
