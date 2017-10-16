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

public partial class CCuentaBancariaCliente
{
    //Propiedades Privadas
    private int idCuentaBancariaCliente;
    private string cuentaBancariaCliente;
    private int idCliente;
    private string descripcion;
    private int idBanco;
    private int idTipoMoneda;
    private int idMetodoPago;
    private bool baja;

    //Propiedades
    public int IdCuentaBancariaCliente
    {
        get { return idCuentaBancariaCliente; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idCuentaBancariaCliente = value;
        }
    }

    public string CuentaBancariaCliente
    {
        get { return cuentaBancariaCliente; }
        set
        {
            cuentaBancariaCliente = value;
        }
    }

    public int IdCliente
    {
        get { return idCliente; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idCliente = value;
        }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set
        {
            descripcion = value;
        }
    }

    public int IdBanco
    {
        get { return idBanco; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idBanco = value;
        }
    }

    public int IdTipoMoneda
    {
        get { return idTipoMoneda; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoMoneda = value;
        }
    }

    public int IdMetodoPago
    {
        get { return idMetodoPago; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idMetodoPago = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CCuentaBancariaCliente()
    {
        idCuentaBancariaCliente = 0;
        cuentaBancariaCliente = "";
        idCliente = 0;
        descripcion = "";
        idBanco = 0;
        idTipoMoneda = 0;
        idMetodoPago = 0;
        baja = false;
    }

    public CCuentaBancariaCliente(int pIdCuentaBancariaCliente)
    {
        idCuentaBancariaCliente = pIdCuentaBancariaCliente;
        cuentaBancariaCliente = "";
        idCliente = 0;
        descripcion = "";
        idBanco = 0;
        idTipoMoneda = 0;
        idMetodoPago = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CuentaBancariaCliente_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CCuentaBancariaCliente>(typeof(CCuentaBancariaCliente), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CuentaBancariaCliente_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CCuentaBancariaCliente>(typeof(CCuentaBancariaCliente), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CuentaBancariaCliente_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancariaCliente", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CCuentaBancariaCliente>(typeof(CCuentaBancariaCliente), pConexion);
        foreach (CCuentaBancariaCliente O in Obten.ListaRegistros)
        {
            idCuentaBancariaCliente = O.IdCuentaBancariaCliente;
            cuentaBancariaCliente = O.CuentaBancariaCliente;
            idCliente = O.IdCliente;
            descripcion = O.Descripcion;
            idBanco = O.IdBanco;
            idTipoMoneda = O.IdTipoMoneda;
            idMetodoPago = O.IdMetodoPago;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CuentaBancariaCliente_ConsultarFiltros";
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
        Obten.Llena<CCuentaBancariaCliente>(typeof(CCuentaBancariaCliente), pConexion);
        foreach (CCuentaBancariaCliente O in Obten.ListaRegistros)
        {
            idCuentaBancariaCliente = O.IdCuentaBancariaCliente;
            cuentaBancariaCliente = O.CuentaBancariaCliente;
            idCliente = O.IdCliente;
            descripcion = O.Descripcion;
            idBanco = O.IdBanco;
            idTipoMoneda = O.IdTipoMoneda;
            idMetodoPago = O.IdMetodoPago;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CuentaBancariaCliente_ConsultarFiltros";
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
        Obten.Llena<CCuentaBancariaCliente>(typeof(CCuentaBancariaCliente), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_CuentaBancariaCliente_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancariaCliente", 0);
        Agregar.StoredProcedure.Parameters["@pIdCuentaBancariaCliente"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaBancariaCliente", cuentaBancariaCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdBanco", idBanco);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idCuentaBancariaCliente = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCuentaBancariaCliente"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_CuentaBancariaCliente_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancariaCliente", idCuentaBancariaCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaBancariaCliente", cuentaBancariaCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdBanco", idBanco);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_CuentaBancariaCliente_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancariaCliente", idCuentaBancariaCliente);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}