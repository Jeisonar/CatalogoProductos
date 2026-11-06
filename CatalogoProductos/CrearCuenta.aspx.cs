using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Security.Cryptography;
namespace CatalogoProductos { public partial class CrearCuenta : CatalogPage {
 protected void Page_Load(object sender,EventArgs e){}
 protected void Crear_Click(object sender,EventArgs e) {
  pnlMensaje.Visible=true;pnlMensaje.CssClass="notice error";
  var nombre=txtNombre.Text.Trim();var correo=txtCorreo.Text.Trim().ToLowerInvariant();var clave=txtClave.Text;
  litMensaje.Text="Ingresá tu nombre, un correo válido y una contraseña de 10 a 128 caracteres. Las contraseñas deben coincidir.";
  if(nombre.Length==0 || nombre.Length>80 || correo.Length>160 || clave.Length<10 || clave.Length>128 || clave!=txtConfirmar.Text)return;
  try{if(new MailAddress(correo).Address!=correo)return;}catch(FormatException){return;}catch(ArgumentException){return;}
  var salt=new byte[16];using(var random=RandomNumberGenerator.Create())random.GetBytes(salt);var hash=CatalogData.Hash(clave,salt,150000);
  try {
   using(var c=CatalogData.Connection()) using(var cmd=new SqlCommand("INSERT INTO dbo.usuarios(nombre,correo,password_hash,password_salt,iteraciones) VALUES(@nombre,@correo,@hash,@salt,150000)",c)) {
    cmd.Parameters.Add("@nombre",SqlDbType.NVarChar,80).Value=nombre;cmd.Parameters.Add("@correo",SqlDbType.NVarChar,160).Value=correo;
    cmd.Parameters.Add("@hash",SqlDbType.VarBinary,32).Value=hash;cmd.Parameters.Add("@salt",SqlDbType.VarBinary,16).Value=salt;c.Open();cmd.ExecuteNonQuery();
   }
   pnlMensaje.CssClass="notice success";litMensaje.Text="Tu cuenta está lista. Ya podés iniciar sesión.";btnCrear.Enabled=false;
  } catch(SqlException ex){Trace.Warn("SQL","Crear cuenta",ex);litMensaje.Text=(ex.Number==2601 || ex.Number==2627)?"No se pudo crear la cuenta con ese correo. Si ya tenés cuenta, iniciá sesión.":"No se pudo crear la cuenta. Intentá de nuevo en unos momentos.";}
 }
}}