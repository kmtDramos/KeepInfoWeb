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

public partial class CEstatusFacturaEncabezado
{
    //Propiedades Privadas
    private int idEstatusFacturaEncabezado;
    private string estatusFacturaEncabezado;
    private bool baja;

    //Propiedades
    public int IdEstatusFacturaEncabezado
    {
        get { return idEstatusFacturaEncabezado; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idEstatusFacturaEncabezado = value;
        }
    }

    public string EstatusFacturaEncabezado
    {
        get { return estatusFacturaEncabezado; }
        set
        {
            estatusFacturaEncabezado = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CEstatusFacturaEncabezado()
    {
        idEstatusFacturaEncabezado = 0;
        estatusFacturaEncabezado = "";
        baja = false;
    }

    public CEstatusFacturaEncabezado(int pIdEstatusFacturaEncabezado)
    {
        idEstatusFacturaEncabezado = pIdEstatusFacturaEncabezado;
        estatusFacturaEncabezado = "";
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_EstatusFacturaEncabezado_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CEstatusFacturaEncabezado>(typeof(CEstatusFacturaEncabezado), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_EstatusFacturaEncabezado_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CEstatusFacturaEncabezado>(typeof(CEstatusFacturaEncabezado), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_EstatusFacturaEncabezado_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdEstatusFacturaEncabezado", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CEstatusFacturaEncabezado>(typeof(CEstatusFacturaEncabezado), pConexion);
        foreach (CEstatusFacturaEncabezado O in Obten.ListaRegistros)
        {
            idEstatusFacturaEncabezado = O.IdEstatusFacturaEncabezado;
            estatusFacturaEncabezado = O.EstatusFacturaEncabezado;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_EstatusFacturaEncabezado_ConsultarFiltros";
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
        Obten.Llena<CEstatusFacturaEncabezado>(typeof(CEstatusFacturaEncabezado), pConexion);
        foreach (CEstatusFacturaEncabezado O in Obten.ListaRegistros)
        {
            idEstatusFacturaEncabezado = O.IdEstatusFacturaEncabezado;
            estatusFacturaEncabezado = O.EstatusFacturaEncabezado;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_EstatusFacturaEncabezado_ConsultarFiltros";
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
        Obten.Llena<CEstatusFacturaEncabezado>(typeof(CEstatusFacturaEncabezado), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_EstatusFacturaEncabezado_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusFacturaEncabezado", 0);
        Agregar.StoredProcedure.Parameters["@pIdEstatusFacturaEncabezado"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pEstatusFacturaEncabezado", estatusFacturaEncabezado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idEstatusFacturaEncabezado = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdEstatusFacturaEncabezado"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_EstatusFacturaEncabezado_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusFacturaEncabezado", idEstatusFacturaEncabezado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEstatusFacturaEncabezado", estatusFacturaEncabezado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_EstatusFacturaEncabezado_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusFacturaEncabezado", idEstatusFacturaEncabezado);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}