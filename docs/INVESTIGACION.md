# Nexo

Catálogo de productos conectado a una base de datos
5 de noviembre de 2026

Nexo permite registrar y consultar productos mediante páginas separadas de inicio, registro y listado. La información se conserva en SQL Server. Este informe explica las tecnologías utilizadas, la comunicación entre sus componentes, las consultas SQL y la verificación de los requisitos.

## 1 Nombre y descripción del stack

Se seleccionó un stack de Microsoft compuesto por C#, ASP.NET Web Forms sobre .NET Framework 4.7.2, ADO.NET y SQL Server. La interfaz utiliza HTML y CSS. Web Forms ejecuta código en el servidor y genera el HTML que recibe el navegador [1]. El entorno local utiliza SQL Server 2025 y el alojamiento utiliza SQL Server 2022 Express. SSMS permite administrar la base local.

| Componente | Tecnología y función en Nexo |
| --- | --- |
| Lenguaje | C# procesa eventos, valida datos y controla las operaciones. |
| Frontend | HTML y CSS presentan formularios, navegación, tablas y diseño adaptable. |
| Backend | ASP.NET Web Forms ejecuta las páginas y sus archivos de código C#. |
| Acceso a datos | ADO.NET con System.Data.SqlClient conecta la aplicación con SQL Server. |
| Base de datos | SQL Server conserva productos y cuentas de usuario. |
| Herramientas | Visual Studio permite desarrollar y compilar; SSMS administra la base. |
| Publicación | Somee con Windows Server 2022, IIS 10, ASP.NET 4.8 y SQL Server 2022 Express. |

Web Forms pertenece a ASP.NET clásico sobre .NET Framework. El proyecto desarrollado no utiliza ASP.NET Core, MVC ni Razor Pages. La consigna permite elegir otras tecnologías de complejidad equivalente.

## 2 Justificación y comunicación de los componentes

### Justificación de la elección

Facilidad de aprendizaje. Se aprovecha el proyecto abierto en Visual Studio y un modelo de eventos que relaciona acciones visibles con métodos concretos: Guardar_Click, Buscar_Click y Entrar_Click. Separar cada pantalla ayuda a comprender sus responsabilidades.

Documentación. Microsoft publica documentación de Web Forms, ADO.NET y SQL Server. Somee documenta la compatibilidad de su alojamiento [1–4]. Estos recursos permiten consultar el funcionamiento de las páginas y las consultas.

Compatibilidad. Las tecnologías del servidor trabajan dentro del entorno .NET de Windows. ADO.NET proporciona el puente entre la aplicación y el origen de datos [2]. Somee permite ejecutar ASP.NET 4.x y alojar la base SQL en su paquete de Windows [3].

Adecuación al alcance. Un catálogo pequeño necesita registrar, listar, buscar y resumir información. SQL Server representa cada producto como una fila y permite ejecutar estas operaciones sin incorporar un ORM ni una API independiente.

### Flujo al registrar un producto

1. El navegador solicita Registrar y muestra el formulario generado por ASP.NET.

2. Al pulsar Guardar producto, envía una solicitud HTTP POST al servidor.

3. Registrar.aspx.cs comprueba campos obligatorios, longitudes y valores numéricos.

4. ADO.NET abre una conexión mediante CatalogoDB y ejecuta un INSERT parametrizado.

5. SQL Server almacena la fila y asigna automáticamente el identificador.

6. El servidor redirige a Productos y muestra la confirmación. El listado se obtiene mediante una nueva consulta SELECT.

### Persistencia y responsabilidades

El navegador presenta la información; el servidor valida y ejecuta consultas; SQL Server conserva los datos. Al recargar, la aplicación vuelve a consultar la tabla. No depende de una lista fija en el código ni de localStorage. Los ejemplos se cargan mediante un script SQL y no se reinician con cada visita.

## 3 Diseño de la base de datos

La base se llama catalogo_productos. Su tabla principal es dbo.productos y contiene seis campos, incluido su identificador.

| Campo | Tipo SQL | Responsabilidad |
| --- | --- | --- |
| id | INT IDENTITY | Clave primaria generada automáticamente. |
| nombre | NVARCHAR(100) | Nombre obligatorio y no vacío. |
| categoria | NVARCHAR(50) | Clasificación obligatoria del producto. |
| precio | DECIMAL(10,2) | Precio en colones, mayor o igual a cero. |
| existencias | INT | Unidades disponibles, mayor o igual a cero. |
| descripcion | NVARCHAR(300) | Detalle opcional del producto. |

### Diez registros de prueba

| Producto | Categoría | Precio CRC | Unidades |
| --- | --- | --- | --- |
| Cuaderno | Útiles escolares | 1 500,00 | 25 |
| Lápiz | Útiles escolares | 300,00 | 50 |
| Lapicero azul | Útiles escolares | 500,00 | 40 |
| Borrador | Útiles escolares | 400,00 | 30 |
| Regla | Útiles escolares | 700,00 | 20 |
| Mouse | Tecnología | 6 500,00 | 12 |
| Teclado | Tecnología | 12 000,00 | 8 |
| Memoria USB | Tecnología | 5 000,00 | 15 |
| Botella reutilizable | Accesorios | 4 500,00 | 18 |
| Mochila | Accesorios | 18 000,00 | 6 |

dbo.usuarios almacena nombre, correo único, hash de contraseña, salt, iteraciones, intentos fallidos y fechas de bloqueo y creación, además del identificador. Las cuentas acceden al catálogo compartido; no existe un inventario separado por usuario.

## 4 Registro y consulta de productos

### Registrar información

```sql
INSERT INTO dbo.productos
 (nombre, categoria, precio, existencias, descripcion)
VALUES (@nombre, @categoria, @precio, @stock, @descripcion);
```

Guardar_Click asigna parámetros con tipos y longitudes explícitos. El precio usa DECIMAL con precisión 10 y escala 2; existencias se envía como INT. ExecuteNonQuery ejecuta la escritura. Los valores se envían como datos y no se concatenan al texto SQL.

Antes de ejecutar el comando, el servidor exige nombre y categoría, verifica existencias enteras y comprueba un precio no negativo con hasta dos decimales. Si la validación falla, se conserva el formulario y se muestra un mensaje. Si el guardado termina correctamente, se redirige a Productos para evitar repetir el INSERT al recargar.

### Listar buscar y filtrar

```sql
SELECT id, nombre, categoria, precio,
       existencias, descripcion
FROM dbo.productos
WHERE (@q = N'' OR CHARINDEX(@q, nombre) > 0)
  AND (@categoria = N'' OR categoria = @categoria)
ORDER BY id DESC;
```

Productos.aspx.cs recoge el texto y la categoría seleccionada. CatalogData.Query crea un SqlCommand, añade los parámetros y utiliza SqlDataAdapter para llenar un DataTable. GridView representa esas filas como una tabla HTML.

Con @q y @categoria vacíos, se devuelven todos los productos. Con @q igual a Mouse y @categoria igual a Tecnología, se devuelve Mouse. Un texto que no coincide con ningún nombre produce cero filas y activa el mensaje de ausencia de resultados.

### Ejecutar las consultas en SSMS

Database/004_consultas.sql contiene consultas demostrativas listas para ejecutar. Se debe seleccionar catalogo_productos antes de ejecutarlo. Incluye listado, búsqueda con variables, resumen general y agrupación por categoría. Estas consultas no modifican los productos.

## 5 Resúmenes e inicio de sesión

### Resumen general

```sql
SELECT COUNT(*) AS cantidad,
 COALESCE(SUM(CAST(existencias AS BIGINT)), 0) AS unidades,
 COALESCE(SUM(precio *
   CAST(existencias AS DECIMAL(19,0))), 0) AS valor
FROM dbo.productos;
```

COUNT cuenta las filas. SUM acumula las unidades y el valor de cada precio multiplicado por existencias. COALESCE devuelve cero si no hay registros. Con los diez ejemplos, el resultado es 10 productos, 224 unidades y 536 500,00 colones de inventario. No representa ventas ni dinero recibido.

### Agrupación por categoría

```sql
SELECT categoria, COUNT(*) AS cantidad,
 SUM(CAST(existencias AS BIGINT)) AS unidades
FROM dbo.productos
GROUP BY categoria
ORDER BY categoria;
```

GROUP BY reúne las filas por categoría y devuelve un resumen por cada grupo [4]. Se calcula sobre todo el catálogo y no cambia con el filtro del listado.

| Categoría | Productos | Unidades |
| --- | --- | --- |
| Accesorios | 2 | 24 |
| Tecnología | 3 | 35 |
| Útiles escolares | 5 | 165 |

### Acceso mediante cuentas

CrearCuenta registra usuarios y Login comprueba sus credenciales. Las contraseñas se guardan como un hash derivado con PBKDF2 HMAC SHA256 y salt aleatoria. Forms Authentication identifica al usuario mediante una cookie y protege las páginas del catálogo.

Cinco intentos incorrectos bloquean la cuenta cinco minutos. Salir elimina la autenticación. La configuración de publicación exige cookies seguras con HTTPS. Nexo utiliza una identidad visual propia y no procesa pagos ni utiliza cuentas de PayPal.

## 6 Pantallas y verificación funcional

Inicio muestra el propósito y los resúmenes; Registrar contiene el formulario; Mis productos presenta el listado y los filtros. Login y CrearCuenta gestionan el acceso.

| Comprobación | Resultado verificado |
| --- | --- |
| Página inicial | Propósito del catálogo, resumen y acceso a las tareas. |
| Formulario | Acepta productos válidos y rechaza campos obligatorios vacíos y negativos. |
| Base de datos | Tabla de seis campos y diez registros originales. |
| Listado | Consulta datos almacenados mediante SQL y ADO.NET. |
| Búsqueda y filtro | Permite buscar por nombre y seleccionar una categoría. |
| Resumen | Cuenta productos y suma unidades y valor; agrupa por categoría. |
| Mensajes | Confirma guardados, informa errores y señala búsquedas sin resultados. |
| Persistencia | El producto permanece al recargar; no se duplica por la recarga. |
| Diseño adaptable | Revisión visual en 1440 y 390 píxeles; tabla desplazable en teléfono. |
| Acceso | Creación de cuenta, login, logout, rutas protegidas y bloqueo temporal. |
| Instalación | Tablas nuevas y semilla ejecutada dos veces sin duplicar ejemplos. |
| Publicación HTTPS | Aplicación accesible en Somee con certificado válido y conexión SQL remota. |

Las pruebas locales utilizaron registros temporales que se retiraron. En Somee se conservaron los diez ejemplos y un producto de demostración: 11 productos, 226 unidades y 539 001,00 colones. El guardado y la persistencia se comprobaron desde la web publicada. Los resúmenes de la sección 5 corresponden a los diez ejemplos iniciales.

### Alcance

Nexo permite registrar y consultar productos. No incorpora edición, eliminación, compras ni recuperación de contraseña. Estos procesos no son necesarios para demostrar el registro, consulta y persistencia solicitados.

## 7 Instalación publicación y fuentes

### Instalación local

1. Abrir CatalogoProductos.slnx en Visual Studio con las herramientas de ASP.NET y el destino .NET Framework 4.7.2. Restaurar los paquetes NuGet y compilar.

2. En SSMS, ejecutar Database/INSTALAR_LOCAL.sql. Crea catalogo_productos si no existe, crea ambas tablas y carga únicamente los ejemplos que faltan. No elimina ni sobrescribe datos existentes.

3. Verificar CatalogoDB en Web.config. Localmente utiliza localhost y autenticación de Windows. Iniciar SQL Server y ejecutar mediante IIS Express.

4. Abrir CrearCuenta, registrar una cuenta e iniciar sesión para acceder al catálogo.

### Publicación y acceso del docente

La plataforma utilizada es Somee, en un paquete gratuito de Windows/IIS con ASP.NET 4.x y SQL Server 2022 Express [3]. Se cargó el paquete compilado Release y se instaló la base remota NexoCatalogo. CatalogoDB se configuró con autenticación SQL del alojamiento. Sus credenciales se conservan únicamente en el servidor, fuera de GitHub.

La aplicación está publicada con HTTPS. El docente puede abrir el enlace, crear su cuenta e iniciar sesión para consultar o registrar productos. El repositorio es público e incluye el código, los scripts y este informe. El certificado gratuito se renueva manualmente desde Somee; el panel indica renovación a partir del 4 de enero de 2027.

[Aplicación publicada](https://nexocatalogojeison.somee.com/)

[Repositorio del código fuente](https://github.com/Jeisonar/CatalogoProductos)

### Fuentes oficiales

Consultadas el 5 de noviembre de 2026. Los métodos, pantallas y resultados descritos corresponden al código y las pruebas de Nexo.

[[1] Microsoft Learn  Qué es ASP NET Web Forms](https://learn.microsoft.com/en-us/aspnet/web-forms/what-is-web-forms)

[[2] Microsoft Learn  Recuperar y modificar datos con ADO NET](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/retrieving-and-modifying-data)

[[3] Somee  Alojamiento gratuito ASP NET y SQL Server](https://somee.com/FreeAspNetHosting.aspx)

[[4] Microsoft Learn  GROUP BY en Transact SQL](https://learn.microsoft.com/en-us/sql/t-sql/queries/select-group-by-transact-sql)
