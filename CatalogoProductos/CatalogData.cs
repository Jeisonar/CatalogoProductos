using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
namespace CatalogoProductos {
 public class CatalogPage : Page {
  protected override void OnInit(EventArgs e) {
   if(Session["NexoCsrf"]==null) Session["NexoCsrf"]=Guid.NewGuid().ToString("N");
   ViewStateUserKey=(string)Session["NexoCsrf"];
   Response.Cache.SetCacheability(HttpCacheability.NoCache);Response.Cache.SetNoStore();
   base.OnInit(e);
  }
 }
 public static class CatalogData {
  public static SqlConnection Connection() { return new SqlConnection(ConfigurationManager.ConnectionStrings["CatalogoDB"].ConnectionString); }
  public static DataTable Query(string sql,params SqlParameter[] parameters) {
   using(var c=Connection()) using(var cmd=new SqlCommand(sql,c)) using(var a=new SqlDataAdapter(cmd)) {
    cmd.Parameters.AddRange(parameters);var table=new DataTable();a.Fill(table);return table;
   }
  }
  public static byte[] Hash(string password,byte[] salt,int iterations) {
   using(var derive=new Rfc2898DeriveBytes(password,salt,iterations,HashAlgorithmName.SHA256)) return derive.GetBytes(32);
  }
  public static bool Equal(byte[] a,byte[] b) { int diff=a.Length^b.Length;for(int i=0;i<a.Length && i<b.Length;i++) diff |= a[i]^b[i];return diff==0; }
 }
}