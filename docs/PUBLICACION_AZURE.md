# Publicar Nexo en Azure
Destino: Azure App Service **Windows** con .NET Framework 4.x y Azure SQL Database.

## Base de datos
1. Crear una base SQL en Azure y conectarse desde SSMS mediante el método configurado en el servidor.
2. Seleccionar la base de Azure y ejecutar, en orden:
   - CatalogoProductos/Database/001_productos.sql
   - CatalogoProductos/Database/002_usuarios.sql
   - CatalogoProductos/Database/003_datos_prueba.sql
3. Ejecutar 004_consultas.sql para comprobar los diez ejemplos y sus resúmenes.
El instalador INSTALAR_LOCAL.sql utiliza master y CREATE DATABASE: es para SQL Server local. En Azure SQL se usan los scripts separados sobre la base existente.

## Aplicación
1. Crear App Service para Windows y comprobar la disponibilidad del nivel gratuito F1 en la suscripción y región.
2. Compilar Release y publicar los archivos compilados mediante Visual Studio o despliegue ZIP.
3. Configurar la cadena de conexión **CatalogoDB** en las variables de conexión de App Service. Debe apuntar al servidor y base de Azure, con cifrado y validación de certificado. No utilizar la autenticación de Windows de localhost.
4. Habilitar HTTPS. Web.Release.config exige cookies seguras.
5. Configurar el acceso de red desde App Service al servidor SQL.
6. Abrir /CrearCuenta, crear una cuenta e iniciar sesión.
7. Registrar un producto, recargar y comprobar que persiste; probar búsqueda, filtro y resumen.

## Acceso del docente
Entregar la URL HTTPS del App Service, el enlace del repositorio y el informe. El docente puede crear su cuenta de acceso al catálogo compartido.

## Estado
Paquete Release compilado y scripts verificados. El despliegue en la suscripción se completa después de autorizar Azure y comprobar las cuotas y condiciones del nivel disponible. No se ha creado una URL pública todavía.

## Documentación
- https://learn.microsoft.com/en-us/azure/app-service/configure-language-dotnet-framework
- https://learn.microsoft.com/en-us/azure/azure-sql/database/free-offer
