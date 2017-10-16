using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;

public partial class Paginas_Agenda : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JArray Eventos = new JArray();
        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion) {
            if (Error == 0) {
                string FechaInicio = Convert.ToString(HttpContext.Current.Request["start"]);
                string FechaFin = Convert.ToString(HttpContext.Current.Request["end"]);
                int IdUsuario = Convert.ToInt32(HttpContext.Current.Request["IdUsuario"]);
                IdUsuario = (IdUsuario == -1) ? UsuarioSesion.IdUsuario : IdUsuario;

                CSelectEspecifico Select = new CSelectEspecifico();
                Select.StoredProcedure.CommandText = "sp_Actividad_ObtenerActividades";
                Select.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;
                Select.StoredProcedure.Parameters.Add("FechaInicio", SqlDbType.VarChar, 10).Value = FechaInicio;
                Select.StoredProcedure.Parameters.Add("FechaFin", SqlDbType.VarChar,10).Value = FechaFin;

                Select.Llena(pConexion);

                while(Select.Registros.Read()) {
                    JObject Evento = new JObject();

                    int IdActividad = Convert.ToInt32(Select.Registros["IdActividad"]);
                    string Actividad = Convert.ToString(Select.Registros["Actividad"]);
                    string Cliente = Convert.ToString(Select.Registros["Cliente"]);
                    string Fecha = Convert.ToString(Select.Registros["Fecha"]);
                    string Fin = Convert.ToString(Select.Registros["Fin"]);
                    DateTime FechaCompleta = Convert.ToDateTime(Select.Registros["FechaCompleta"]);
                    DateTime FinActividad = Convert.ToDateTime(Select.Registros["FinActividad"]);
                    string Oportunidad = Convert.ToString(Select.Registros["Oportunidad"]);
                    string TipoActividad = Convert.ToString(Select.Registros["TipoActividad"]);
                    string Color = Convert.ToString(Select.Registros["Color"]);

                    string fechaInicio = FechaCompleta.ToShortDateString() + ' ' + FechaCompleta.ToShortTimeString();
                    string fechaFin = FinActividad.ToShortDateString() + ' ' + FinActividad.ToShortTimeString();

                    Actividad = fechaInicio +" - "+ fechaFin +"<br/>"+
                        "<strong>" + Cliente + "</strong>" + "<br/>"+
                        "<i>" + Actividad + "</i><br/>" +
                        Oportunidad;

                    Evento.Add("id", IdActividad);
                    Evento.Add("title", TipoActividad +" - "+ Cliente);
                    Evento.Add("dialogTitle", TipoActividad + " - " + FechaCompleta.ToShortDateString() +' '+ FechaCompleta.ToShortTimeString());
                    Evento.Add("description", Actividad);
                    Evento.Add("start", Fecha);
                    Evento.Add("end", Fin);
                    Evento.Add("color", Color);
                    Eventos.Add(Evento);
                }

                Select.CerrarConsulta();
                
            }
        });

        HttpContext.Current.Response.Write(Eventos.ToString());
    }
}
