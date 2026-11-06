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
