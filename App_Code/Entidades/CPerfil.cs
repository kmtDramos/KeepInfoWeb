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


public partial class CPerfil
{
    //Metodos Especiales
    public string ObtenerJsonArbol(CConexion pConexion)
    {
        CSelect ObtenPerfil = new CSelect();
        ObtenPerfil.StoredProcedure.CommandText = "sp_Perfil_Consulta";
        ObtenPerfil.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenPerfil.Llena<CPerfil>(typeof(CPerfil), pConexion);

        JObject oRespuesta = new JObject();
        oRespuesta.Add(new JProperty("Error", 0));
        JObject Modelo = new JObject();
        Modelo.Add(new JProperty("id", 0));
        JArray JItem = new JArray();
        foreach (CPerfil OPerfil in ObtenPerfil.ListaRegistros)
        {
            JObject JPerfil = new JObject();
            JPerfil.Add(new JProperty("id", OPerfil.IdPerfil.ToString()));
            JPerfil.Add(new JProperty("text", OPerfil.Perfil));
            JPerfil.Add(new JProperty("im0", "html.png"));
            JPerfil.Add(new JProperty("im1", "html.png"));
            JPerfil.Add(new JProperty("im2", "html.png"));
            JItem.Add(JPerfil);
        }
        JObject oPerfil = new JObject();
        oPerfil.Add(new JProperty("id", "Perfil"));
        oPerfil.Add(new JProperty("text", "Perfil"));
        oPerfil.Add(new JProperty("open", "1"));
        oPerfil.Add(new JProperty("select", "1"));
        oPerfil.Add(new JProperty("item", JItem));

        JItem = new JArray();
        JItem.Add(oPerfil);
        Modelo.Add(new JProperty("item", JItem));

        oRespuesta.Add(new JProperty("Modelo", Modelo));
        return oRespuesta.ToString();
    }

    public string PaginaInicio(CConexion pConexion)
    {
        string Pagina = "";
        CMenu Menu = new CMenu();
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_Perfil_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 5);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", IdPerfil);
        ObtenObjeto.Llena<CPagina>(typeof(CPagina), pConexion);
        foreach (CPagina OPagina in ObtenObjeto.ListaRegistros)
        {
            Pagina = OPagina.Pagina;
            Menu.IdMenu = OPagina.IdMenu;
        }
        if (Menu.IdMenu != 0)
        {
            Menu.LlenaObjeto(Menu.IdMenu, pConexion);
        }
        return "../Paginas/" + Pagina;
    }
}
