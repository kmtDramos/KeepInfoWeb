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

public partial class CCorreoContactoOrganizacion
{
    //Propiedades Privadas
    private int idCorreoContactoOrganizacion;
    private string correo;
    private int idContactoOrganizacion;
    private bool baja;

    //Propiedades
    public int IdCorreoContactoOrganizacion
    {
        get { return idCorreoContactoOrganizacion; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idCorreoContactoOrganizacion = value;
        }
    }

    public string Correo
    {
        get { return correo; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            correo = value;
        }
    }

    public int IdContactoOrganizacion
    {
        get { return idContactoOrganizacion; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idContactoOrganizacion = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CCorreoContactoOrganizacion()
    {
        idCorreoContactoOrganizacion = 0;
        correo = "";
        idContactoOrganizacion = 0;
        baja = false;
    }

    public CCorreoContactoOrganizacion(int pIdCorreoContactoOrganizacion)
    {
        idCorreoContactoOrganizacion = pIdCorreoContactoOrganizacion;
        correo = "";
        idContactoOrganizacion = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CorreoContactoOrganizacion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CCorreoContactoOrganizacion>(typeof(CCorreoContactoOrganizacion), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CorreoContactoOrganizacion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CCorreoContactoOrganizacion>(typeof(CCorreoContactoOrganizacion), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CorreoContactoOrganizacion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdCorreoContactoOrganizacion", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CCorreoContactoOrganizacion>(typeof(CCorreoContactoOrganizacion), pConexion);
        foreach (CCorreoContactoOrganizacion O in Obten.ListaRegistros)
        {
            idCorreoContactoOrganizacion = O.IdCorreoContactoOrganizacion;
            correo = O.Correo;
            idContactoOrganizacion = O.IdContactoOrganizacion;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CorreoContactoOrganizacion_ConsultarFiltros";
        foreach (KeyValuePair<string, object> parametro in pParametros)
        {
            if (parametro.Key == "Opcion")
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@" + parametro.Key, parametro.Value);
            }
            else
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@p" + parametro.Key, parametro.Value);
            }
        }
        Obten.Llena<CCorreoContactoOrganizacion>(typeof(CCorreoContactoOrganizacion), pConexion);
        foreach (CCorreoContactoOrganizacion O in Obten.ListaRegistros)
        {
            idCorreoContactoOrganizacion = O.IdCorreoContactoOrganizacion;
            correo = O.Correo;
            idContactoOrganizacion = O.IdContactoOrganizacion;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CorreoContactoOrganizacion_ConsultarFiltros";
        foreach (KeyValuePair<string, object> parametro in pParametros)
        {
            if (parametro.Key == "Opcion")
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@" + parametro.Key, parametro.Value);
            }
            else
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@p" + parametro.Key, parametro.Value);
            }
        }
        Obten.Llena<CCorreoContactoOrganizacion>(typeof(CCorreoContactoOrganizacion), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_CorreoContactoOrganizacion_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCorreoContactoOrganizacion", 0);
        Agregar.StoredProcedure.Parameters["@pIdCorreoContactoOrganizacion"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idCorreoContactoOrganizacion = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCorreoContactoOrganizacion"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_CorreoContactoOrganizacion_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCorreoContactoOrganizacion", idCorreoContactoOrganizacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_CorreoContactoOrganizacion_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCorreoContactoOrganizacion", idCorreoContactoOrganizacion);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}