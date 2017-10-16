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
using Newtonsoft.Json.Linq;


public partial class COpcion
{
    //Metodos Especiales
    public string ObtenerJsonArbol(CConexion pConexion)
    {
        CSelect ObtenOpcion = new CSelect();
        ObtenOpcion.StoredProcedure.CommandText = "sp_Opcion_Consulta";
        ObtenOpcion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenOpcion.Llena<COpcion>(typeof(COpcion), pConexion);

        JObject oRespuesta = new JObject();
        oRespuesta.Add(new JProperty("Error", 0));
        JObject Modelo = new JObject();
        Modelo.Add(new JProperty("id", 0));
        JArray JItem = new JArray();
        foreach (COpcion OOpcion in ObtenOpcion.ListaRegistros)
        {
            JObject JOpcion = new JObject();
            JOpcion.Add(new JProperty("id", OOpcion.IdOpcion.ToString()));
            JOpcion.Add(new JProperty("text", OOpcion.Opcion));
            JOpcion.Add(new JProperty("im0", "html.png"));
            JOpcion.Add(new JProperty("im1", "html.png"));
            JOpcion.Add(new JProperty("im2", "html.png"));
            JItem.Add(JOpcion);
        }

        JObject oOpcion = new JObject();
        oOpcion.Add(new JProperty("id", "Opcion"));
        oOpcion.Add(new JProperty("text", "Opcion"));
        oOpcion.Add(new JProperty("open", "1"));
        oOpcion.Add(new JProperty("select", "1"));
        oOpcion.Add(new JProperty("item", JItem));

        JItem = new JArray();
        JItem.Add(oOpcion);
        Modelo.Add(new JProperty("item", JItem));

        oRespuesta.Add(new JProperty("Modelo", Modelo));
        return oRespuesta.ToString();
    }

    public string ObtenerJsonArbolPermisos(CConexion pConexion)
    {
        JObject oRespuesta = new JObject();
        oRespuesta.Add(new JProperty("Error", 0));
        JObject Modelo = new JObject();
        Modelo.Add(new JProperty("id", 0));
        JArray JItems = new JArray();

        //Perfil
        CSelect ObtenPerfil = new CSelect();
        ObtenPerfil.StoredProcedure.CommandText = "sp_Perfil_Consulta";
        ObtenPerfil.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenPerfil.Llena<CPerfil>(typeof(CPerfil), pConexion);
        JArray JAPerfiles = new JArray();
        foreach (CPerfil OPerfil in ObtenPerfil.ListaRegistros)
        {
            JObject JPerfil = new JObject();
            JPerfil.Add(new JProperty("id", "Perfil|" + OPerfil.IdPerfil.ToString()));
            JPerfil.Add(new JProperty("text", OPerfil.Perfil));
            JPerfil.Add(new JProperty("im0", "html.png"));
            JPerfil.Add(new JProperty("im1", "html.png"));
            JPerfil.Add(new JProperty("im2", "html.png"));
            JAPerfiles.Add(JPerfil);
        }
        JObject JIPerfil = new JObject();
        JIPerfil.Add("id", "Perfiles");
        JIPerfil.Add("text", "Perfiles");
        JIPerfil.Add("item", JAPerfiles);
        JItems.Add(JIPerfil);

        //Pagina
        CSelect ObtenPagina = new CSelect();
        ObtenPagina.StoredProcedure.CommandText = "sp_Pagina_Consulta";
        ObtenPagina.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenPagina.Llena<CPagina>(typeof(CPagina), pConexion);
        JArray JAPaginas = new JArray();
        foreach (CPagina OPagina in ObtenPagina.ListaRegistros)
        {
            JObject JPagina = new JObject();
            JPagina.Add(new JProperty("id", "Pagina|" + OPagina.IdPagina.ToString()));
            JPagina.Add(new JProperty("text", OPagina.Pagina));
            JPagina.Add(new JProperty("im0", "html.png"));
            JPagina.Add(new JProperty("im1", "html.png"));
            JPagina.Add(new JProperty("im2", "html.png"));
            JAPaginas.Add(JPagina);
        }
        JObject JIPagina = new JObject();
        JIPagina.Add("id", "Paginas");
        JIPagina.Add("text", "Paginas");
        JIPagina.Add("item", JAPaginas);
        JItems.Add(JIPagina);

        //Permisos
        JObject oPermisos = new JObject();
        oPermisos.Add(new JProperty("id", "Permisos"));
        oPermisos.Add(new JProperty("text", "Permisos"));
        oPermisos.Add(new JProperty("open", "1"));
        oPermisos.Add(new JProperty("select", "1"));
        oPermisos.Add(new JProperty("item", JItems));
        Modelo.Add(new JProperty("item", new JArray(oPermisos)));

        oRespuesta.Add(new JProperty("Modelo", Modelo));
        return oRespuesta.ToString();
    }

    public string XMLArbolPermisos(CConexion pConexion)
    {
        string XMLPermiso = "<?xml version='1.0' encoding='iso-8859-1'?>";
        XMLPermiso = XMLPermiso + "<tree id='0'>";
        XMLPermiso = XMLPermiso + "<item text='Permisos' id='Permisos' open='1' select='1'>";

        CSelect ObtenPerfil = new CSelect();
        ObtenPerfil.StoredProcedure.CommandText = "sp_Perfil_Consulta";
        ObtenPerfil.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenPerfil.Llena<CPerfil>(typeof(CPerfil), pConexion);

        string XMLPerfil = "";
        XMLPerfil = XMLPerfil + "<item text='Perfiles' id='Perfiles'>";
        foreach (CPerfil OPerfil in ObtenPerfil.ListaRegistros)
        {
            XMLPerfil = XMLPerfil + "<item text='" + OPerfil.Perfil + "' id='Perfil|" + OPerfil.IdPerfil + "' im0='llave.png' im1='llave.png' im2='llave.png'></item>";
            XMLPerfil = XMLPerfil + "";
        }
        XMLPerfil = XMLPerfil + "</item>";

        CSelect ObtenPagina = new CSelect();
        ObtenPagina.StoredProcedure.CommandText = "sp_Pagina_Consulta";
        ObtenPagina.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenPagina.Llena<CPagina>(typeof(CPagina), pConexion);

        string XMLPagina = "";
        XMLPagina = XMLPagina + "<item text='Paginas' id='Paginas'>";
        foreach (CPagina OPagina in ObtenPagina.ListaRegistros)
        {
            XMLPagina = XMLPagina + "<item text='" + OPagina.Pagina + "' id='Pagina|" + OPagina.IdPagina + "' im0='html.png' im1='html.png' im2='html.png'></item>";
            XMLPagina = XMLPagina + "";
        }
        XMLPagina = XMLPagina + "</item>";

        XMLPermiso = XMLPermiso + XMLPerfil + XMLPagina + "</item></tree>";

        return XMLPermiso;
    }

    public List<object> PerfilPermisosDisponibles(int pIdPerfil, CConexion pConexion)
    {
        CSelect ObtenPermisos = new CSelect();
        ObtenPermisos.StoredProcedure.CommandText = "sp_PerfilPermiso_Consulta";
        ObtenPermisos.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        ObtenPermisos.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", pIdPerfil);
        ObtenPermisos.Llena<COpcion>(typeof(COpcion), pConexion);
        return ObtenPermisos.ListaRegistros;
    }

    public List<object> PaginaPermisosDisponibles(int pIdPagina, CConexion pConexion)
    {
        CSelect ObtenPermisos = new CSelect();
        ObtenPermisos.StoredProcedure.CommandText = "sp_PaginaPermiso_Consulta";
        ObtenPermisos.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        ObtenPermisos.StoredProcedure.Parameters.AddWithValue("@pIdPagina", pIdPagina);
        ObtenPermisos.Llena<COpcion>(typeof(COpcion), pConexion);
        return ObtenPermisos.ListaRegistros;
    }

    public static bool ExisteOpcion(int pIdOpcion, string pOpcion, CConexion pConexion)
    {
        bool existe = false;
        CSelectEspecifico ObtenValidacion = new CSelectEspecifico();
        ObtenValidacion.StoredProcedure.CommandText = "sp_Opcion_Consulta";
        ObtenValidacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 3);
        ObtenValidacion.StoredProcedure.Parameters.AddWithValue("@pIdOpcion", pIdOpcion);
        ObtenValidacion.StoredProcedure.Parameters.AddWithValue("@pOpcion", pOpcion);
        ObtenValidacion.Llena(pConexion);
        if (ObtenValidacion.Registros.Read())
        {
            existe = true;
        }
        ObtenValidacion.CerrarConsulta();
        return existe;
    }

    public static bool ExisteComando(int pIdOpcion, string pComando, CConexion pConexion)
    {
        bool existe = false;
        CSelectEspecifico ObtenValidacion = new CSelectEspecifico();
        ObtenValidacion.StoredProcedure.CommandText = "sp_Opcion_Consulta";
        ObtenValidacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 4);
        ObtenValidacion.StoredProcedure.Parameters.AddWithValue("@pIdOpcion", pIdOpcion);
        ObtenValidacion.StoredProcedure.Parameters.AddWithValue("@pComando", pComando);
        ObtenValidacion.Llena(pConexion);
        if (ObtenValidacion.Registros.Read())
        {
            existe = true;
        }
        ObtenValidacion.CerrarConsulta();
        return existe;
    }

    public static List<object> ObtienePermisosOpciones(int pIdPerfil, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_PerfilPermiso_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 5);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", pIdPerfil);
        ObtenObjeto.Llena<COpcion>(typeof(COpcion), pConexion);
        return ObtenObjeto.ListaRegistros;
    }
}