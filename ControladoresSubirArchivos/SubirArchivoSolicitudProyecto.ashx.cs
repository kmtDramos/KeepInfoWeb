using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.IO;

namespace arquitecturaNet.ControladoresSubirArchivos
{
    /// <summary>
    /// Descripción breve de $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SubirArchivoSolicitudProyecto : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

            int IdSolicitudProyecto = Convert.ToInt32(HttpContext.Current.Request["pIdSolicitudProyecto"]);
            String filename = HttpContext.Current.Request.Headers["X-File-Name"];
            string ruta = HttpContext.Current.Server.MapPath("~") + "\\Archivos\\ArchivosSolicitudProyecto";
            Stream inputStream = HttpContext.Current.Request.InputStream;
            filename = filename.Replace("%20", "");
            string[] aFileName = filename.Split('.');
            filename = filename.Replace("." + aFileName.GetValue(aFileName.Length - 1), "");
            filename = "archivo_solicitudProyecto_" + IdSolicitudProyecto + "." + aFileName.GetValue(aFileName.Length - 1);
            FileStream fileStream = new FileStream(ruta + "\\" + filename, FileMode.OpenOrCreate);

            byte[] bytesInStream = new byte[inputStream.Length];
            inputStream.Read(bytesInStream, 0, (int)bytesInStream.Length);
            fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            fileStream.Close();
            context.Response.Write("{success:true, name:\"" + filename + "\", path:\"" + ruta + "/" + filename + "\"}"); fileStream.Close();

            CConexion ConexionBaseDatos = new CConexion();
            ConexionBaseDatos.ConectarBaseDatosSqlServer();

            CSolicitudProyecto solicitudProyecto = new CSolicitudProyecto();
            solicitudProyecto.LlenaObjeto(IdSolicitudProyecto, ConexionBaseDatos);
            solicitudProyecto.Archivo = filename;
            solicitudProyecto.Editar(ConexionBaseDatos);

            CArchivoSolicitudProyecto archivoSolicitudProyecto = new CArchivoSolicitudProyecto();
            archivoSolicitudProyecto.IdArchivoSolicitudProyecto = IdSolicitudProyecto;
            archivoSolicitudProyecto.ArchivoSolicitudProyecto = filename;
            archivoSolicitudProyecto.FechaCreacion = DateTime.Now;
            archivoSolicitudProyecto.IdUsuarioCracion = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            archivoSolicitudProyecto.Agregar(ConexionBaseDatos);

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
}
