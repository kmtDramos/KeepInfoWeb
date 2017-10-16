using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.IO;
using System.IO.IsolatedStorage;

    /// <summary>
    /// Descripción breve de $codebehindclassname$
    /// </summary>
[WebService(Namespace = "http://www.tsk.com.mx/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class SubirarchivoXML : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.Expires = -1;

        String filename = HttpContext.Current.Request.Headers["X-File-Name"];
        string ruta = HttpContext.Current.Server.MapPath("~") + "\\Archivos\\ArchivosAddendas";
        Stream inputStream = HttpContext.Current.Request.InputStream;
        filename = filename.Replace("%20", "");
        string[] aFileName = filename.Split('.');
        filename = filename.Replace("." + aFileName.GetValue(aFileName.Length - 1), "");
        //filename = filename + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "." + aFileName.GetValue(aFileName.Length - 1);
        filename = filename + "." + aFileName.GetValue(aFileName.Length - 1);
        FileStream fileStream = new FileStream(ruta + "\\" + filename, FileMode.OpenOrCreate);

        byte[] bytesInStream = new byte[inputStream.Length];
        inputStream.Read(bytesInStream, 0, (int)bytesInStream.Length);
        fileStream.Write(bytesInStream, 0, bytesInStream.Length);
        fileStream.Close();
        context.Response.Write("{success:true, name:\"" + filename + "\", path:\"" + ruta + "/" + filename + "\"}");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}