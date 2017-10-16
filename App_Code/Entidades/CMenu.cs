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


public partial class CMenu
{
    //Metodos Especiales
    public string ObtenerJsonArbol(CConexion pConexion)
    {
        CSelect ObtenMenu = new CSelect();
        ObtenMenu.StoredProcedure.CommandText = "sp_Menu_Consulta";
        ObtenMenu.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenMenu.Llena<CMenu>(typeof(CMenu), pConexion);

        JObject oRespuesta = new JObject();
        oRespuesta.Add(new JProperty("Error", 0));
        JObject Modelo = new JObject();
        Modelo.Add(new JProperty("id", 0));
        JArray JItem = new JArray();
        foreach (CMenu OMenu in ObtenMenu.ListaRegistros)
        {
            JObject JMenu = new JObject();
            JMenu.Add(new JProperty("id", OMenu.IdMenu.ToString()));
            JMenu.Add(new JProperty("text", OMenu.Menu));
            JMenu.Add(new JProperty("im0", "html.png"));
            JMenu.Add(new JProperty("im1", "html.png"));
            JMenu.Add(new JProperty("im2", "html.png"));
            JItem.Add(JMenu);
        }
        JObject oMenu = new JObject();
        oMenu.Add(new JProperty("id", "Menu"));
        oMenu.Add(new JProperty("text", "Menu"));
        oMenu.Add(new JProperty("open", "1"));
        oMenu.Add(new JProperty("select", "1"));
        oMenu.Add(new JProperty("item", JItem));

        JItem = new JArray();
        JItem.Add(oMenu);
        Modelo.Add(new JProperty("item", JItem));

        oRespuesta.Add(new JProperty("Modelo", Modelo));
        return oRespuesta.ToString();
    }

    public List<object> ObtenerMenuPrincipal(int pIdPerfil, string pProyecto, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        string strConsulta = "";
        int opcion = 0;
        CUsuario Usuario = new CUsuario();

        if (Usuario.TienePermisos(new string[] { "controlTotal" }, pIdPerfil, pConexion) == "")
        {
            strConsulta = strConsulta + "sp_Menu_Consulta";
            opcion = 2;
        }
        else
        {
            strConsulta = strConsulta + "sp_Perfil_Consulta";
            opcion = 4;
        }

        ObtenObjeto.StoredProcedure.CommandText = strConsulta;
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", opcion);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pProyectoSistema", pProyecto);
        if (Usuario.TienePermisos(new string[] { "controlTotal" }, pIdPerfil, pConexion) != "")
        {
            ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", pIdPerfil);
        }
        ObtenObjeto.Llena<CMenu>(typeof(CMenu), pConexion);
        return ObtenObjeto.ListaRegistros;
    }

    public void OrdenarMenu(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_Menu_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Editar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdMenu", idMenu);
        Editar.Update(pConexion);
    }

    public List<object> LlenaObjetos_OrdenadoPorMenu(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_Menu_Consultar_OrdenadoPorMenu";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Llena<CMenu>(typeof(CMenu), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos_OrdenadoPorOrden(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_Menu_Consultar_OrdenadoPorOrden";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Llena<CMenu>(typeof(CMenu), pConexion);
        return Obten.ListaRegistros;
    }
}