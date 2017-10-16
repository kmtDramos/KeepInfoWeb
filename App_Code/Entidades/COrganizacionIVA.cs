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


public partial class COrganizacionIVA
{
    //Constructores

    //Metodos Especiales
    public void AgregarOrganizacionIVA(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_OrganizacionIVA_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacionIVA", 0);
        Agregar.StoredProcedure.Parameters["@pIdOrganizacionIVA"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pEsPrincipal", esPrincipal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idOrganizacionIVA = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdOrganizacionIVA"].Value);
    }

    public static JObject ObtenerIVA(JObject pModelo, int pIdOrganizacionIVA, CConexion pConexion)
    {
        COrganizacionIVA OrganizacionIVA = new COrganizacionIVA();
        OrganizacionIVA.LlenaObjeto(pIdOrganizacionIVA, pConexion);
        pModelo.Add("IdOrganizacionIVA", OrganizacionIVA.IdOrganizacionIVA);
        pModelo.Add("IVA", OrganizacionIVA.IVA);
        pModelo.Add("EsPrincipal", OrganizacionIVA.EsPrincipal);
        return pModelo;
    }

    public void EditarOrganizacionIVA(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_OrganizacionIVA_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacionIVA", idOrganizacionIVA);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEsPrincipal", esPrincipal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }
}
