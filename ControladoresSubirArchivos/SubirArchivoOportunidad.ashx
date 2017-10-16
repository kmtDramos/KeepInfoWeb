<%@ WebHandler Language="C#" Class="SubirArchivoOportunidad" %>
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.IO;

/// <summary>
/// Descripción breve de $codebehindclassname$
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class SubirArchivoOportunidad : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {

        context.Response.ContentType = "text/plain";
        context.Response.Expires = -1;

        int IdOportunidad = Convert.ToInt32(HttpContext.Current.Request["pIdOportunidad"]);
        String filename = HttpContext.Current.Request.Headers["X-File-Name"];
        string ruta = HttpContext.Current.Server.MapPath("~") + "\\Archivos\\ArchivosOportunidad";
        Stream inputStream = HttpContext.Current.Request.InputStream;
        filename = filename.Replace("%20", "");
        string[] aFileName = filename.Split('.');
        filename = filename.Replace("." + aFileName.GetValue(aFileName.Length - 1), "");
        filename = "archivo_oportunidad_"+ IdOportunidad +"." + aFileName.GetValue(aFileName.Length - 1);
        FileStream fileStream = new FileStream(ruta + "\\" + filename, FileMode.OpenOrCreate);

        byte[] bytesInStream = new byte[inputStream.Length];
        inputStream.Read(bytesInStream, 0, (int)bytesInStream.Length);
        fileStream.Write(bytesInStream, 0, bytesInStream.Length);
        fileStream.Close();
        context.Response.Write("{success:true, name:\"" + filename + "\", path:\"" + ruta + "/" + filename + "\"}"); fileStream.Close();

        CConexion ConexionBaseDatos = new CConexion();
        ConexionBaseDatos.ConectarBaseDatosSqlServer();
        
        COportunidad Oportunidad = new COportunidad();
        Oportunidad.LlenaObjeto(IdOportunidad, ConexionBaseDatos);
        Oportunidad.Archivo = filename;
        Oportunidad.Editar(ConexionBaseDatos);

        CArchivoOportunidad ArchivoOportunidad = new CArchivoOportunidad();
        ArchivoOportunidad.IdOportunidad = IdOportunidad;
        ArchivoOportunidad.ArchivoOportunidad = filename;
        ArchivoOportunidad.FechaCreacion = DateTime.Now;
        ArchivoOportunidad.Agregar(ConexionBaseDatos);

        ConexionBaseDatos.CerrarBaseDatosSqlServer();

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}