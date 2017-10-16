using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CPaginaPermiso
{
    //Metodos Especiales
    public List<object> PermisosAsignados(int pIdPagina, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_PaginaPermiso_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 3);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdPagina", pIdPagina);
        ObtenObjeto.Llena<CPaginaPermiso>(typeof(CPaginaPermiso), pConexion);
        return ObtenObjeto.ListaRegistros;
    }

    public List<object> PermisosExistentes(string pIdOpciones, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "spr_PermisosExistentesPagina_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdOpcion", pIdOpciones);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdPagina", IdPagina);
        ObtenObjeto.Llena<CPaginaPermiso>(typeof(CPaginaPermiso), pConexion);
        return ObtenObjeto.ListaRegistros;
    }

    public List<object> PermisosPagina(string pPagina, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_Pagina_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 3);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pPagina", pPagina);
        ObtenObjeto.Llena<CPaginaPermiso>(typeof(CPaginaPermiso), pConexion);
        return ObtenObjeto.ListaRegistros;
    }

    public void Activar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_PaginaPermiso_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdPaginaPermiso", IdPaginaPermiso);
        Editar.Update(pConexion);
    }

    public void Desactivar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_PaginaPermiso_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 3);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdPaginaPermiso", IdPaginaPermiso);
        Editar.Update(pConexion);
    }

    public void DesactivarTodos(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_PaginaPermiso_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 4);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdPagina", IdPagina);
        Editar.Update(pConexion);
    }
}
