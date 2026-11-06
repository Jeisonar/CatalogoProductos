# Nexo — Catálogo de productos
Aplicación ASP.NET Web Forms (C#, .NET Framework 4.7.2), SQL Server y ADO.NET.

## Pantallas
- Login: inicio de sesión con correo y contraseña.
- CrearCuenta: registro de usuarios. La cuenta se guarda en SQL Server.
- Default: resumen del inventario y agrupación por categoría.
- Registrar: formulario de productos.
- Productos: listado, búsqueda por nombre y filtro de categoría.

Las cuentas acceden al catálogo compartido del proyecto. No se incluyen credenciales predeterminadas. Crear la primera cuenta desde /CrearCuenta y luego entrar desde /Login.

## Ejecutar
Abrir CatalogoProductos.slnx en Visual Studio, comprobar que SQL Server esté iniciado y ejecutar con IIS Express. La conexión local está en CatalogoProductos/Web.config, entrada CatalogoDB, con autenticación de Windows a localhost y base catalogo_productos.
Para una instalación nueva, aplicar Database/002_usuarios.sql después de crear la base y la tabla productos.

## Acceso
Las páginas del catálogo exigen autenticación del servidor. Se utiliza Forms Authentication con cookies HttpOnly y SameSite=Lax; las contraseñas se derivan con PBKDF2-HMAC-SHA256 y salt aleatoria. Cinco intentos incorrectos bloquean la cuenta cinco minutos. Las consultas usan parámetros. El guardado de productos redirige al listado para evitar duplicados al recargar.

La transformación Release exige cookies seguras para publicación con HTTPS. La conexión de producción deberá configurarse para el SQL Server del alojamiento.

## Verificación realizada
Compilación correcta, registro con confirmación, credenciales correctas e incorrectas, cierre de sesión, rutas protegidas, bloqueo temporal, validación de productos, guardado y persistencia, búsqueda sin resultados, filtro de categoría y revisión visual en 1440 px y 390 px. Se retiraron la cuenta y el producto temporales; permanecen los diez productos originales.
