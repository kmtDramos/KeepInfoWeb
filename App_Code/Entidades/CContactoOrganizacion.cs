using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;


public partial class CContactoOrganizacion
{
    //Constructores

    //Metodos Especiales
    public void EliminarTelefonoCorreo(int pIdContactoOrganizacion, CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_ContactoOrganizacion_EliminarTelefonoCorreo";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", pIdContactoOrganizacion);
        Eliminar.Delete(pConexion);
    }


    public static JArray ObtenerJsonContactoOrganizacion(int pIdOrganizacion, CConexion pConexion)
    {
        CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
        JArray JContactoOrganizaciones = new JArray();

        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("IdOrganizacion", pIdOrganizacion);
        ParametrosTI.Add("Baja", 0);

        foreach (CContactoOrganizacion oContactoOrganizacion in ContactoOrganizacion.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JContactoOrganizacion = new JObject();
            JContactoOrganizacion.Add("IdContactoOrganizacion", oContactoOrganizacion.IdContactoOrganizacion);
            JContactoOrganizacion.Add("Nombre", oContactoOrganizacion.Nombre);
            JContactoOrganizacion.Add("Notas", oContactoOrganizacion.Notas);
            if (oContactoOrganizacion.IdContactoOrganizacion == pIdOrganizacion)
            {
                JContactoOrganizacion.Add(new JProperty("Selected", 1));
            }
            else
            {
                JContactoOrganizacion.Add(new JProperty("Selected", 0));
            }

            JContactoOrganizaciones.Add(JContactoOrganizacion);
        }
        return JContactoOrganizaciones;
    }

    public static JArray ObtenerJsonContactoOrganizacionFiltroIdCliente(int pIdCliente, CConexion pConexion)
    {
        CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
        JArray JContactoOrganizaciones = new JArray();

        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("IdCliente", pIdCliente);
        ParametrosTI.Add("Baja", false);

        foreach (CContactoOrganizacion oContactoOrganizacion in ContactoOrganizacion.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JContactoOrganizacion = new JObject();
            JContactoOrganizacion.Add("Valor", oContactoOrganizacion.IdContactoOrganizacion);
            JContactoOrganizacion.Add("Descripcion", oContactoOrganizacion.Nombre);
            JContactoOrganizaciones.Add(JContactoOrganizacion);
        }
        return JContactoOrganizaciones;
    }

    public static JArray ObtenerJsonContactoOrganizacion(int pIdOrganizacion, int pIdContactoOrganizacion, CConexion pConexion)
    {
        CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
        JArray JContactoOrganizaciones = new JArray();

        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("IdOrganizacion", pIdOrganizacion);
        ParametrosTI.Add("Baja", 0);

        foreach (CContactoOrganizacion oContactoOrganizacion in ContactoOrganizacion.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JContactoOrganizacion = new JObject();
            JContactoOrganizacion.Add("IdContactoOrganizacion", oContactoOrganizacion.IdContactoOrganizacion);
            JContactoOrganizacion.Add("Nombre", oContactoOrganizacion.Nombre);
            JContactoOrganizacion.Add("Notas", oContactoOrganizacion.Notas);
            if (oContactoOrganizacion.IdContactoOrganizacion == pIdContactoOrganizacion)
            {
                JContactoOrganizacion.Add(new JProperty("Selected", 1));
            }
            else
            {
                JContactoOrganizacion.Add(new JProperty("Selected", 0));
            }

            JContactoOrganizaciones.Add(JContactoOrganizacion);
        }
        return JContactoOrganizaciones;
    }

}