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

public partial class CCaracteristicaProducto
{
    //Propiedades Privadas
    private int idCaracteristicaProducto;
    private string valor;
    private int idCaracteristica;
    private int idUnidadCaracteristica;
    private int idProducto;
    private bool baja;

    //Propiedades
    public int IdCaracteristicaProducto
    {
        get { return idCaracteristicaProducto; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idCaracteristicaProducto = value;
        }
    }

    public string Valor
    {
        get { return valor; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            valor = value;
        }
    }

    public int IdCaracteristica
    {
        get { return idCaracteristica; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idCaracteristica = value;
        }
    }

    public int IdUnidadCaracteristica
    {
        get { return idUnidadCaracteristica; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUnidadCaracteristica = value;
        }
    }

    public int IdProducto
    {
        get { return idProducto; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idProducto = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CCaracteristicaProducto()
    {
        idCaracteristicaProducto = 0;
        valor = "";
        idCaracteristica = 0;
        idUnidadCaracteristica = 0;
        idProducto = 0;
        baja = false;
    }

    public CCaracteristicaProducto(int pIdCaracteristicaProducto)
    {
        idCaracteristicaProducto = pIdCaracteristicaProducto;
        valor = "";
        idCaracteristica = 0;
        idUnidadCaracteristica = 0;
        idProducto = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CaracteristicaProducto_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CCaracteristicaProducto>(typeof(CCaracteristicaProducto), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CaracteristicaProducto_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CCaracteristicaProducto>(typeof(CCaracteristicaProducto), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CaracteristicaProducto_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdCaracteristicaProducto", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CCaracteristicaProducto>(typeof(CCaracteristicaProducto), pConexion);
        foreach (CCaracteristicaProducto O in Obten.ListaRegistros)
        {
            idCaracteristicaProducto = O.IdCaracteristicaProducto;
            valor = O.Valor;
            idCaracteristica = O.IdCaracteristica;
            idUnidadCaracteristica = O.IdUnidadCaracteristica;
            idProducto = O.IdProducto;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CaracteristicaProducto_ConsultarFiltros";
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
        Obten.Llena<CCaracteristicaProducto>(typeof(CCaracteristicaProducto), pConexion);
        foreach (CCaracteristicaProducto O in Obten.ListaRegistros)
        {
            idCaracteristicaProducto = O.IdCaracteristicaProducto;
            valor = O.Valor;
            idCaracteristica = O.IdCaracteristica;
            idUnidadCaracteristica = O.IdUnidadCaracteristica;
            idProducto = O.IdProducto;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CaracteristicaProducto_ConsultarFiltros";
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
        Obten.Llena<CCaracteristicaProducto>(typeof(CCaracteristicaProducto), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_CaracteristicaProducto_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCaracteristicaProducto", 0);
        Agregar.StoredProcedure.Parameters["@pIdCaracteristicaProducto"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pValor", valor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCaracteristica", idCaracteristica);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCaracteristica", idUnidadCaracteristica);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idCaracteristicaProducto = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCaracteristicaProducto"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_CaracteristicaProducto_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCaracteristicaProducto", idCaracteristicaProducto);
        Editar.StoredProcedure.Parameters.AddWithValue("@pValor", valor);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCaracteristica", idCaracteristica);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCaracteristica", idUnidadCaracteristica);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_CaracteristicaProducto_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCaracteristicaProducto", idCaracteristicaProducto);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}