using System;
using System.Globalization;
namespace CatalogoProductos { public partial class _Default : CatalogPage {
 protected void Page_Load(object sender,EventArgs e) {
  if(IsPostBack)return;
  try {
   var row=CatalogData.Query(@"SELECT COUNT(*) cantidad, COALESCE(SUM(CAST(existencias AS BIGINT)),0) unidades,
    COALESCE(SUM(precio * CAST(existencias AS DECIMAL(19,0))),0) valor FROM dbo.productos").Rows[0];
   var culture=CultureInfo.GetCultureInfo("es-CR");
   litCantidad.Text=Convert.ToInt32(row["cantidad"]).ToString("N0",culture);
   litUnidades.Text=Convert.ToInt64(row["unidades"]).ToString("N0",culture);
   litValor.Text="₡ "+Convert.ToDecimal(row["valor"]).ToString("N2",culture);
   gvCategorias.DataSource=CatalogData.Query("SELECT categoria,COUNT(*) cantidad,SUM(CAST(existencias AS BIGINT)) unidades FROM dbo.productos GROUP BY categoria ORDER BY categoria");gvCategorias.DataBind();
  } catch(System.Data.SqlClient.SqlException ex) {Trace.Warn("SQL","Consulta de resumen",ex);pnlMensaje.Visible=true;litMensaje.Text="No se pudo consultar el inventario. Intentá de nuevo en unos momentos.";litCantidad.Text=litUnidades.Text=litValor.Text="—";}
 }
}}