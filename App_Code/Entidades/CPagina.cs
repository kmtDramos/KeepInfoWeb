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


public partial class CPagina
{
    //Metodos Especiales
    public void FiltrarPagina(string pPagina, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_Pagina_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 6);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pPagina", pPagina);
        ObtenObjeto.Llena<CPagina>(typeof(CPagina), pConexion);
        foreach (CPagina OPagina in ObtenObjeto.ListaRegistros)
        {
            IdPagina = OPagina.IdPagina;
            Pagina = OPagina.Pagina;
            Titulo = OPagina.Titulo;
            NombreMenu = OPagina.NombreMenu;
            IdMenu = OPagina.IdMenu;
        }
    }

    public string ObtenerJsonArbol(CConexion pConexion)
    {
        CSelect ObtenPagina = new CSelect();
        ObtenPagina.StoredProcedure.CommandText = "sp_Pagina_Consulta";
        ObtenPagina.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenPagina.Llena<CPagina>(typeof(CPagina), pConexion);

        JObject oRespuesta = new JObject();
        oRespuesta.Add(new JProperty("Error", 0));
        JObject Modelo = new JObject();
        Modelo.Add(new JProperty("id", 0));
        JArray JItem = new JArray();
        foreach (CPagina OPagina in ObtenPagina.ListaRegistros)
        {
            JObject JPagina = new JObject();
            JPagina.Add(new JProperty("id", OPagina.IdPagina.ToString()));
            JPagina.Add(new JProperty("text", OPagina.Pagina));
            JPagina.Add(new JProperty("im0", "html.png"));
            JPagina.Add(new JProperty("im1", "html.png"));
            JPagina.Add(new JProperty("im2", "html.png"));
            JItem.Add(JPagina);
        }
        JObject oPagina = new JObject();
        oPagina.Add(new JProperty("id", "Pagina"));
        oPagina.Add(new JProperty("text", "Páginas"));
        oPagina.Add(new JProperty("open", "1"));
        oPagina.Add(new JProperty("select", "1"));
        oPagina.Add(new JProperty("item", JItem));

        JItem = new JArray();
        JItem.Add(oPagina);
        Modelo.Add(new JProperty("item", JItem));

        oRespuesta.Add(new JProperty("Modelo", Modelo));
        return oRespuesta.ToString();
    }

    public List<object> ObtenerMenuSecundario(int pIdMenu, int pIdPerfil, string pProyecto, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        string strConsulta = "";
        int opcion = 0;
        CUsuario Usuario = new CUsuario();

        if (Usuario.TienePermisos(new string[] { "controlTotal" }, pIdPerfil, pConexion) == "")
        {
            strConsulta = strConsulta + "sp_Pagina_Consulta";
            opcion = 2;
        }
        else
        {
            strConsulta = strConsulta + "spr_ObtenerMenuSecundario_Consulta";
            opcion = 1;
        }

        ObtenObjeto.StoredProcedure.CommandText = strConsulta;
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", opcion);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pProyectoSistema", pProyecto);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdMenu", pIdMenu);
        if (Usuario.TienePermisos(new string[] { "controlTotal" }, pIdPerfil, pConexion) != "")
        {
            ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", pIdPerfil);
        }
        ObtenObjeto.Llena<CPagina>(typeof(CPagina), pConexion);
        return ObtenObjeto.ListaRegistros;
    }

    public List<object> LlenaObjetos(int pIdMenu, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_Pagina_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 5);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdMenu", pIdMenu);
        ObtenObjeto.Llena<CPagina>(typeof(CPagina), pConexion);
        return ObtenObjeto.ListaRegistros;
    }

    public void OrdenarSubmenu(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_Pagina_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Editar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdPagina", idPagina);
        Editar.Update(pConexion);
    }
}
