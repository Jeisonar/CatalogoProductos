using System;
using System.Web.Security;
using System.Web.UI;
namespace CatalogoProductos { public partial class SiteMaster : MasterPage {
 public string NavClass(string page) {
  var current=System.IO.Path.GetFileNameWithoutExtension(Request.AppRelativeCurrentExecutionFilePath);
  return string.Equals(current,page,StringComparison.OrdinalIgnoreCase)?"nav-link active":"nav-link";
 }
 protected void Page_Load(object sender,EventArgs e) { }
 protected void Salir_Click(object sender,EventArgs e) { FormsAuthentication.SignOut();Session.Clear();Session.Abandon();Response.Redirect("~/Login",false);Context.ApplicationInstance.CompleteRequest(); }
}}