using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CPerfilPermiso
{
    //Metodos Especiales
    public List<object> PermisosAsignados(int pIdPerfil, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_PerfilPermiso_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 3);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", pIdPerfil);
        ObtenObjeto.Llena<CPerfilPermiso>(typeof(CPerfilPermiso), pConexion);
        return ObtenObjeto.ListaRegistros;
    }

    public List<object> PermisosExistentes(string pIdOpciones, int pIdPerfil, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "spr_PermisosExistentesPerfil_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", pIdPerfil);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdOpcion", pIdOpciones);
        ObtenObjeto.Llena<CPerfilPermiso>(typeof(CPerfilPermiso), pConexion);
        return ObtenObjeto.ListaRegistros;
    }

    public void Activar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_PerfilPermiso_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdPerfilPermiso", IdPerfilPermiso);
        Editar.Update(pConexion);
    }

    public void Desactivar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_PerfilPermiso_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 3);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdPerfilPermiso", IdPerfilPermiso);
        Editar.Update(pConexion);
    }

    public void DesactivarTodos(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_PerfilPermiso_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 4);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", IdPerfil);
        Editar.Update(pConexion);
    }
}
