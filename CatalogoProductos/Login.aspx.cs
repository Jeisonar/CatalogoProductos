using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
namespace CatalogoProductos { public partial class Login : CatalogPage {
 protected void Page_Load(object sender,EventArgs e) { if(!IsPostBack && Request.IsAuthenticated) {Response.Redirect("~/Default",false);Context.ApplicationInstance.CompleteRequest();} }
 protected void Entrar_Click(object sender,EventArgs e) {
  pnlMensaje.Visible=true;litMensaje.Text="No pudimos iniciar sesión. Revisá tus datos o intentá de nuevo más tarde.";
  var correo=txtCorreo.Text.Trim().ToLowerInvariant();var clave=txtClave.Text;
  if(correo.Length==0 || correo.Length>160 || clave.Length==0 || clave.Length>128)return;
  try {
   var table=CatalogData.Query("SELECT id,password_hash,password_salt,iteraciones,bloqueado_hasta FROM dbo.usuarios WHERE correo=@correo",new SqlParameter("@correo",SqlDbType.NVarChar,160){Value=correo});
   if(table.Rows.Count==0) {CatalogData.Hash(clave,new byte[16],150000);return;}
   var row=table.Rows[0];if(row["bloqueado_hasta"]!=DBNull.Value && (DateTime)row["bloqueado_hasta"]>DateTime.UtcNow)return;
   var valid=CatalogData.Equal((byte[])row["password_hash"],CatalogData.Hash(clave,(byte[])row["password_salt"],(int)row["iteraciones"]));
   using(var c=CatalogData.Connection()) using(var cmd=new SqlCommand(valid?"UPDATE dbo.usuarios SET intentos_fallidos=0,bloqueado_hasta=NULL WHERE id=@id":@"UPDATE dbo.usuarios SET intentos_fallidos=CASE WHEN bloqueado_hasta<=SYSUTCDATETIME() THEN 1 ELSE intentos_fallidos+1 END,
    bloqueado_hasta=CASE WHEN bloqueado_hasta<=SYSUTCDATETIME() THEN NULL WHEN intentos_fallidos>=4 THEN DATEADD(MINUTE,5,SYSUTCDATETIME()) ELSE NULL END WHERE id=@id",c)) {
    cmd.Parameters.Add("@id",SqlDbType.Int).Value=row["id"];c.Open();cmd.ExecuteNonQuery();
   }
   if(!valid)return;
   Session.Clear();Session.Abandon();FormsAuthentication.SetAuthCookie(correo,false);
   Response.Redirect("~/Default",false);Context.ApplicationInstance.CompleteRequest();
  } catch(SqlException ex){Trace.Warn("SQL","Iniciar sesión",ex);litMensaje.Text="No se pudo conectar con el servicio. Intentá de nuevo en unos momentos.";}
 }
}}