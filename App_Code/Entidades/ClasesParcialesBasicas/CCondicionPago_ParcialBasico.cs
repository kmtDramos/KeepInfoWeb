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

public partial class CCondicionPago
{
    //Propiedades Privadas
    private int idCondicionPago;
    private string condicionPago;
    private int numeroDias;
    private bool baja;

    //Propiedades
    public int IdCondicionPago
    {
        get { return idCondicionPago; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idCondicionPago = value;
        }
    }

    public string CondicionPago
    {
        get { return condicionPago; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            condicionPago = value;
        }
    }

    public int NumeroDias
    {
        get { return numeroDias; }
        set
        {
            if (value < 0)
            {
                return;
            }
            numeroDias = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CCondicionPago()
    {
        idCondicionPago = 0;
        condicionPago = "";
        numeroDias = 0;
        baja = false;
    }

    public CCondicionPago(int pIdCondicionPago)
    {
        idCondicionPago = pIdCondicionPago;
        condicionPago = "";
        numeroDias = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CondicionPago_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CCondicionPago>(typeof(CCondicionPago), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CondicionPago_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CCondicionPago>(typeof(CCondicionPago), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CondicionPago_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CCondicionPago>(typeof(CCondicionPago), pConexion);
        foreach (CCondicionPago O in Obten.ListaRegistros)
        {
            idCondicionPago = O.IdCondicionPago;
            condicionPago = O.CondicionPago;
            numeroDias = O.NumeroDias;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CondicionPago_ConsultarFiltros";
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
        Obten.Llena<CCondicionPago>(typeof(CCondicionPago), pConexion);
        foreach (CCondicionPago O in Obten.ListaRegistros)
        {
            idCondicionPago = O.IdCondicionPago;
            condicionPago = O.CondicionPago;
            numeroDias = O.NumeroDias;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CondicionPago_ConsultarFiltros";
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
        Obten.Llena<CCondicionPago>(typeof(CCondicionPago), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_CondicionPago_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", 0);
        Agregar.StoredProcedure.Parameters["@pIdCondicionPago"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCondicionPago", condicionPago);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroDias", numeroDias);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idCondicionPago = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCondicionPago"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_CondicionPago_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", idCondicionPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCondicionPago", condicionPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroDias", numeroDias);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_CondicionPago_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", idCondicionPago);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}