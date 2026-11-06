using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
namespace CatalogoProductos { public partial class Productos : CatalogPage {
 protected void Page_Load(object sender,EventArgs e) {
  if(!IsPostBack) { Cargar();if(Session["ProductoGuardado"]!=null) {pnlMensaje.Visible=true;pnlMensaje.CssClass="notice success";litMensaje.Text=(string)Session["ProductoGuardado"];Session.Remove("ProductoGuardado");} }
 }
 private void Cargar() {
  try {
   var selected=ddlCategoria.SelectedValue;
   ddlCategoria.Items.Clear();ddlCategoria.Items.Add(new ListItem("Todas las categorías",""));
   foreach(DataRow row in CatalogData.Query("SELECT DISTINCT categoria FROM dbo.productos ORDER BY categoria").Rows) ddlCategoria.Items.Add(new ListItem((string)row[0],(string)row[0]));
   if(ddlCategoria.Items.FindByValue(selected)!=null)ddlCategoria.SelectedValue=selected;
   var data=CatalogData.Query(@"SELECT id,nombre,categoria,precio,existencias,descripcion FROM dbo.productos
    WHERE (@q=N'' OR CHARINDEX(@q,nombre)>0) AND (@categoria=N'' OR categoria=@categoria) ORDER BY id DESC",
    new SqlParameter("@q",SqlDbType.NVarChar,100){Value=txtBusqueda.Text.Trim()},new SqlParameter("@categoria",SqlDbType.NVarChar,50){Value=ddlCategoria.SelectedValue});
   gvProductos.DataSource=data;gvProductos.DataBind();litResultados.Text=data.Rows.Count+" producto(s) encontrados";
  } catch(SqlException ex){Trace.Warn("SQL","Consulta de productos",ex);pnlMensaje.Visible=true;pnlMensaje.CssClass="notice error";litMensaje.Text="No se pudo consultar el catálogo. Intentá de nuevo en unos momentos.";}
 }
 protected void Buscar_Click(object sender,EventArgs e){pnlMensaje.Visible=false;Cargar();}
 protected void Limpiar_Click(object sender,EventArgs e){txtBusqueda.Text="";ddlCategoria.SelectedIndex=0;pnlMensaje.Visible=false;Cargar();}
}}