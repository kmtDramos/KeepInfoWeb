using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CProyectoSistema
{
    //Constructores

    //Metodos Especiales
    public string XMLArbolProyecto(CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_Proyecto_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.Llena<CProyectoSistema>(typeof(CProyectoSistema), pConexion);

        string XMLProyecto = "<?xml version='1.0' encoding='iso-8859-1'?>";
        XMLProyecto = XMLProyecto + "<tree id='0'>";
        XMLProyecto = XMLProyecto + "<item text='Proyecto' id='Proyecto' open='1' select='1'>";
        foreach (CProyectoSistema OProyecto in ObtenObjeto.ListaRegistros)
        {
            XMLProyecto = XMLProyecto + "<item text='" + OProyecto.ProyectoSistema + "' id='" + OProyecto.IdProyectoSistema + "' im0='html.png' im1='html.png' im2='html.png'></item>";
            XMLProyecto = XMLProyecto + "";
        }
        XMLProyecto = XMLProyecto + "</item>";
        XMLProyecto = XMLProyecto + "</tree>";

        return XMLProyecto;
    }
}
