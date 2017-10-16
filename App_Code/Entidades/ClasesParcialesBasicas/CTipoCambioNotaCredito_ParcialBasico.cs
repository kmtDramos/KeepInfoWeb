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

public partial class CTipoCambioNotaCredito
{
    //Propiedades Privadas
    private int idTipoCambioNotaCredito;
    private int idTipoMonedaOrigen;
    private int idTipoMonedaDestino;
    private decimal tipoCambio;
    private DateTime fecha;
    private int idNotaCredito;

    //Propiedades
    public int IdTipoCambioNotaCredito
    {
        get { return idTipoCambioNotaCredito; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoCambioNotaCredito = value;
        }
    }

    public int IdTipoMonedaOrigen
    {
        get { return idTipoMonedaOrigen; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoMonedaOrigen = value;
        }
    }

    public int IdTipoMonedaDestino
    {
        get { return idTipoMonedaDestino; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoMonedaDestino = value;
        }
    }

    public decimal TipoCambio
    {
        get { return tipoCambio; }
        set
        {
            if (value < 0)
            {
                return;
            }
            tipoCambio = value;
        }
    }

    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public int IdNotaCredito
    {
        get { return idNotaCredito; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idNotaCredito = value;
        }
    }

    //Constructores
    public CTipoCambioNotaCredito()
    {
        idTipoCambioNotaCredito = 0;
        idTipoMonedaOrigen = 0;
        idTipoMonedaDestino = 0;
        tipoCambio = 0;
        fecha = new DateTime(1, 1, 1);
        idNotaCredito = 0;
    }

    public CTipoCambioNotaCredito(int pIdTipoCambioNotaCredito)
    {
        idTipoCambioNotaCredito = pIdTipoCambioNotaCredito;
        idTipoMonedaOrigen = 0;
        idTipoMonedaDestino = 0;
        tipoCambio = 0;
        fecha = new DateTime(1, 1, 1);
        idNotaCredito = 0;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_TipoCambioNotaCredito_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Llena<CTipoCambioNotaCredito>(typeof(CTipoCambioNotaCredito), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_TipoCambioNotaCredito_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CTipoCambioNotaCredito>(typeof(CTipoCambioNotaCredito), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_TipoCambioNotaCredito_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioNotaCredito", pIdentificador);
        Obten.Llena<CTipoCambioNotaCredito>(typeof(CTipoCambioNotaCredito), pConexion);
        foreach (CTipoCambioNotaCredito O in Obten.ListaRegistros)
        {
            idTipoCambioNotaCredito = O.IdTipoCambioNotaCredito;
            idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
            idTipoMonedaDestino = O.IdTipoMonedaDestino;
            tipoCambio = O.TipoCambio;
            fecha = O.Fecha;
            idNotaCredito = O.IdNotaCredito;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_TipoCambioNotaCredito_ConsultarFiltros";
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
        Obten.Llena<CTipoCambioNotaCredito>(typeof(CTipoCambioNotaCredito), pConexion);
        foreach (CTipoCambioNotaCredito O in Obten.ListaRegistros)
        {
            idTipoCambioNotaCredito = O.IdTipoCambioNotaCredito;
            idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
            idTipoMonedaDestino = O.IdTipoMonedaDestino;
            tipoCambio = O.TipoCambio;
            fecha = O.Fecha;
            idNotaCredito = O.IdNotaCredito;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_TipoCambioNotaCredito_ConsultarFiltros";
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
        Obten.Llena<CTipoCambioNotaCredito>(typeof(CTipoCambioNotaCredito), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_TipoCambioNotaCredito_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioNotaCredito", 0);
        Agregar.StoredProcedure.Parameters["@pIdTipoCambioNotaCredito"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        if (fecha.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", idNotaCredito);
        Agregar.Insert(pConexion);
        idTipoCambioNotaCredito = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdTipoCambioNotaCredito"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_TipoCambioNotaCredito_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioNotaCredito", idTipoCambioNotaCredito);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        if (fecha.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", idNotaCredito);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_TipoCambioNotaCredito_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioNotaCredito", idTipoCambioNotaCredito);
        Eliminar.Delete(pConexion);
    }
}