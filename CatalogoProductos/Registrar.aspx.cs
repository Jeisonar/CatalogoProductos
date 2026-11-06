using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
namespace CatalogoProductos { public partial class Registrar : CatalogPage {
 protected void Page_Load(object sender,EventArgs e){}
 protected void Guardar_Click(object sender,EventArgs e) {
  var nombre=txtNombre.Text.Trim();var categoria=txtCategoria.Text.Trim();var descripcion=txtDescripcion.Text.Trim();decimal precio;int stock;
  if(nombre.Length==0 || nombre.Length>100 || categoria.Length==0 || categoria.Length>50 || descripcion.Length>300 ||
   !decimal.TryParse(txtPrecio.Text.Trim().Replace(',','.'),NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out precio) || precio<0 || precio>99999999.99m || decimal.Round(precio,2)!=precio ||
   !int.TryParse(txtExistencias.Text.Trim(),NumberStyles.None,CultureInfo.InvariantCulture,out stock) || stock<0) {
   pnlMensaje.Visible=true;litMensaje.Text="Revisá los campos: nombre y categoría son obligatorios, el precio admite hasta dos decimales y las existencias deben ser enteras. No se admiten valores negativos. La descripción permite 300 caracteres.";return;
  }
  try {
   using(var c=CatalogData.Connection()) using(var cmd=new SqlCommand("INSERT INTO dbo.productos(nombre,categoria,precio,existencias,descripcion) VALUES(@nombre,@categoria,@precio,@stock,@descripcion)",c)) {
    cmd.Parameters.Add("@nombre",SqlDbType.NVarChar,100).Value=nombre;cmd.Parameters.Add("@categoria",SqlDbType.NVarChar,50).Value=categoria;
    var p=cmd.Parameters.Add("@precio",SqlDbType.Decimal);p.Precision=10;p.Scale=2;p.Value=precio;
    cmd.Parameters.Add("@stock",SqlDbType.Int).Value=stock;cmd.Parameters.Add("@descripcion",SqlDbType.NVarChar,300).Value=descripcion.Length==0?(object)DBNull.Value:descripcion;
    c.Open();cmd.ExecuteNonQuery();
   }
   Session["ProductoGuardado"]="Producto guardado correctamente. Ya forma parte de tu catálogo.";
   Response.Redirect("~/Productos",false);Context.ApplicationInstance.CompleteRequest();
  } catch(SqlException ex){Trace.Warn("SQL","Guardar producto",ex);pnlMensaje.Visible=true;litMensaje.Text="No se pudo guardar el producto. Revisá la conexión e intentá de nuevo.";}
 }
}}