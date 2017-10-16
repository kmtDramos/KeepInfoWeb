using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;


public partial class COportunidad
{
    //Constructores
    public SqlCommand StoredProcedure = new SqlCommand();

    //Metodos Especiales
    public static JArray ObtenerOportunidadUsuario(int pIdUsuario, CConexion Conexion)
    {
        JArray JOportunidades = new JArray();
        COportunidad Oportunidad = new COportunidad();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();

        Parametros.Add("Baja", 0);
        Parametros.Add("IdUsuarioCreacion", pIdUsuario);

        foreach (COportunidad oOportunidad in Oportunidad.LlenaObjetosFiltros(Parametros, Conexion))
        {
            JObject JOportunidad = new JObject();
            CNivelInteresOportunidad NivelInteresOportunidad = new CNivelInteresOportunidad();
            NivelInteresOportunidad.LlenaObjeto(oOportunidad.IdNivelInteresOportunidad, Conexion);
            JOportunidad.Add("Valor", oOportunidad.IdOportunidad);
            JOportunidad.Add("Descripcion", oOportunidad.Oportunidad + " (" + NivelInteresOportunidad.NivelInteresOportunidad + ")");

            JOportunidades.Add(JOportunidad);
        }

        return JOportunidades;
    }

    public static JArray ObtenerOportunidadCotizacion(int IdOportunidad, int pIdUsuario, CConexion Conexion)
    {
        JArray JOportunidades = new JArray();
        COportunidad Oportunidad = new COportunidad();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();

        if (pIdUsuario == Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]))
        {
            Parametros.Add("Baja", 0);
            Parametros.Add("IdUsuarioCreacion", pIdUsuario);

            foreach (COportunidad oOportunidad in Oportunidad.LlenaObjetosFiltros(Parametros, Conexion))
            {
                JObject JOportunidad = new JObject();
                JOportunidad.Add("Valor", oOportunidad.IdOportunidad);
                JOportunidad.Add("Descripcion", oOportunidad.Oportunidad);
                if (oOportunidad.IdOportunidad == IdOportunidad)
                {
                    JOportunidad.Add("Selected", 1);
                }
                else
                {
                    JOportunidad.Add("Selected", 0);
                }
                JOportunidades.Add(JOportunidad);
            }
        }
        else
        {
            JObject JOportunidad = new JObject();
            Oportunidad.LlenaObjeto(IdOportunidad, Conexion);
            JOportunidad.Add("Valor", IdOportunidad);
            JOportunidad.Add("Descripcion", Oportunidad.Oportunidad);
            JOportunidad.Add("Selected", 1);
            JOportunidades.Add(JOportunidad);
        }

        return JOportunidades;
    }

    public static JArray ObtenerOportunidadProyecto(int pIdCliente, int pIdUsuario, CConexion Conexion)
    {
        JArray JOportunidades = new JArray();
        COportunidad Oportunidad = new COportunidad();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        //Parametros.Add("IdUsuarioCreacion", pIdUsuario);
        Parametros.Add("IdCliente", pIdCliente);
		//Parametros.Add("EsProyecto", 1);
        Parametros.Add("Cerrado", 0);

        foreach (COportunidad oOportunidad in Oportunidad.LlenaObjetosFiltros(Parametros, Conexion))
        {
            CUsuario UsuarioCreador = new CUsuario();
            UsuarioCreador.LlenaObjeto(oOportunidad.IdUsuarioCreacion, Conexion);
            JObject JOportunidad = new JObject();
            JOportunidad.Add("Valor", oOportunidad.IdOportunidad);
            JOportunidad.Add("Descripcion", oOportunidad.Oportunidad + " (" + UsuarioCreador.Nombre + " " + UsuarioCreador.ApellidoPaterno + " " + UsuarioCreador.ApellidoMaterno + ")");
            JOportunidades.Add(JOportunidad);
        }

        return JOportunidades;
    }

    public static JArray ObtenerOportunidadProyecto(int pIdCliente, int pIdUsuario, int pIdOportunidad, CConexion Conexion)
    {
        JArray JOportunidades = new JArray();
        COportunidad Oportunidad = new COportunidad();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        //Parametros.Add("IdUsuarioCreacion", pIdUsuario);
        Parametros.Add("IdCliente", pIdCliente);

        foreach (COportunidad oOportunidad in Oportunidad.LlenaObjetosFiltros(Parametros, Conexion))
        {
            JObject JOportunidad = new JObject();
            CUsuario UsuarioCreador = new CUsuario();
            UsuarioCreador.LlenaObjeto(oOportunidad.IdUsuarioCreacion, Conexion);
            JOportunidad.Add("Valor", oOportunidad.IdOportunidad);
            JOportunidad.Add("Descripcion", oOportunidad.Oportunidad + " (" + UsuarioCreador.Nombre + UsuarioCreador.ApellidoPaterno + UsuarioCreador.ApellidoMaterno + ")");
            if (oOportunidad.IdOportunidad == pIdOportunidad)
            {
                JOportunidad.Add("Selected", 1);
            }
            else
            {
                JOportunidad.Add("Selected", 0);
            }
            JOportunidades.Add(JOportunidad);
        }

        return JOportunidades;
    }

    public static void ActualizarTotalesOportunidad(int pIdOportunidad, CConexion pConexion)
    {
        if (pIdOportunidad != 0 && pIdOportunidad != null)
        {
            CConsultaAccion Editar = new CConsultaAccion();
            Editar.StoredProcedure.CommandText = "sp_Oportunidad_ActualizarTotales";
            Editar.StoredProcedure.CommandType = CommandType.StoredProcedure;
            Editar.StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
            Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", pIdOportunidad);
            Editar.Update(pConexion);
        }
    }

    public string ObtenerJsonOportunidad(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }

    public static JArray ObtenerJsonOportunidadClasificacion(int Clasificacion, CConexion Conexion)
    {
        JArray JClasificaciones = new JArray();
        for (int i = 0; i <= 1; i++)
        {
            JObject JClasificacion = new JObject();
            JClasificacion.Add("Valor", i);
            JClasificacion.Add("Descripcion", (i==0)?"GA":"Cliente");
            if (Clasificacion == i)
            {
                JClasificacion.Add("Selected", 1);
            }
            else
            {
                JClasificacion.Add("Selected", 0);
            }
            JClasificaciones.Add(JClasificacion);
        }
        return JClasificaciones;
    }
}