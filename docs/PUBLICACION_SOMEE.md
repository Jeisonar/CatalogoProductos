# Publicación de Nexo en Somee
Aplicación: https://nexocatalogojeison.somee.com/
Código: https://github.com/Jeisonar/CatalogoProductos

## Plataforma
Paquete gratuito de Windows Server 2022, IIS 10, ASP.NET Framework 4.x en modo integrado y SQL Server 2022 Express. La aplicación compila para .NET Framework 4.7.2.

## Base remota
La base del alojamiento se llama NexoCatalogo. Se ejecutó INSTALAR_SOMEE.sql para crear productos y usuarios y cargar diez ejemplos. La comprobación posterior al instalador produjo 10 productos, 224 unidades y 536500.00 de valor.

Se agregó desde la aplicación el Producto de demostración Nexo, con precio 1250.50 y 2 unidades. La comprobación posterior produjo 11 productos, 226 unidades y 539001.00 de valor. Ese registro se conserva para la demostración.

## Archivos y conexión
Se importó el ZIP compilado de Release en la raíz del sitio. El Web.config del servidor usa CatalogoDB con autenticación SQL y la base del alojamiento. Las credenciales no se guardan en GitHub ni en el ZIP público. La configuración del repositorio mantiene la conexión local de Windows para desarrollo.

## Acceso HTTPS
Se emitió un certificado gratuito de Let's Encrypt para el dominio. El panel señala renovación manual a partir del 4 de enero de 2027. Las cookies de autenticación de Release requieren HTTPS. Para ingresar, crear una cuenta desde /CrearCuenta y luego abrir /Login.

## Verificación en producción
- Registro de cuenta y rechazo de contraseña incorrecta.
- Login correcto, logout y rutas protegidas.
- Validación de campos obligatorios.
- Guardado confirmado en SQL y persistencia después de recargar.
- Búsqueda sin resultados y filtro por Tecnología.
- Resumen de productos, unidades y valor.
- Diseño en teléfono de 390 píxeles sin desbordamiento de página.

## Volver a publicar
Compilar Release con WebPublish, publicar sus archivos y conservar la conexión remota configurada. Un ZIP público debe permanecer sin contraseñas. Aplicar scripts SQL únicamente sobre la base asignada por Somee. El instalador local con CREATE DATABASE no se usa en el alojamiento.

## Fuentes
https://somee.com/FreeAspNetHosting.aspx
https://somee.com/Faq/126
