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

public partial class CAsientoContableDetalle
{
    //Propiedades Privadas
    private int idAsientoContableDetalle;
    private int idAsientoContable;
    private string cuentaContable;
    private int idCliente;
    private int idProveedor;
    private int idCuentaBancaria;
    private int idIVA;
    private int idSucursal;
    private int idDivision;
    private int idTipoCompra;
    private int idCuentaContable;
    private decimal cargo;
    private decimal abono;
    private string descripcionCuentaContable;
    private bool baja;

    //Propiedades
    public int IdAsientoContableDetalle
    {
        get { return idAsientoContableDetalle; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idAsientoContableDetalle = value;
        }
    }

    public int IdAsientoContable
    {
        get { return idAsientoContable; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idAsientoContable = value;
        }
    }

    public string CuentaContable
    {
        get { return cuentaContable; }
        set
        {
            cuentaContable = value;
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

    public int IdProveedor
    {
        get { return idProveedor; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idProveedor = value;
        }
    }

    public int IdCuentaBancaria
    {
        get { return idCuentaBancaria; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idCuentaBancaria = value;
        }
    }

    public int IdIVA
    {
        get { return idIVA; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idIVA = value;
        }
    }

    public int IdSucursal
    {
        get { return idSucursal; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idSucursal = value;
        }
    }

    public int IdDivision
    {
        get { return idDivision; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idDivision = value;
        }
    }

    public int IdTipoCompra
    {
        get { return idTipoCompra; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoCompra = value;
        }
    }

    public int IdCuentaContable
    {
        get { return idCuentaContable; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idCuentaContable = value;
        }
    }

    public decimal Cargo
    {
        get { return cargo; }
        set
        {
            if (value < 0)
            {
                return;
            }
            cargo = value;
        }
    }

    public decimal Abono
    {
        get { return abono; }
        set
        {
            if (value < 0)
            {
                return;
            }
            abono = value;
        }
    }

    public string DescripcionCuentaContable
    {
        get { return descripcionCuentaContable; }
        set
        {
            descripcionCuentaContable = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CAsientoContableDetalle()
    {
        idAsientoContableDetalle = 0;
        idAsientoContable = 0;
        cuentaContable = "";
        idCliente = 0;
        idProveedor = 0;
        idCuentaBancaria = 0;
        idIVA = 0;
        idSucursal = 0;
        idDivision = 0;
        idTipoCompra = 0;
        idCuentaContable = 0;
        cargo = 0;
        abono = 0;
        descripcionCuentaContable = "";
        baja = false;
    }

    public CAsientoContableDetalle(int pIdAsientoContableDetalle)
    {
        idAsientoContableDetalle = pIdAsientoContableDetalle;
        idAsientoContable = 0;
        cuentaContable = "";
        idCliente = 0;
        idProveedor = 0;
        idCuentaBancaria = 0;
        idIVA = 0;
        idSucursal = 0;
        idDivision = 0;
        idTipoCompra = 0;
        idCuentaContable = 0;
        cargo = 0;
        abono = 0;
        descripcionCuentaContable = "";
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsientoContableDetalle_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAsientoContableDetalle>(typeof(CAsientoContableDetalle), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsientoContableDetalle_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CAsientoContableDetalle>(typeof(CAsientoContableDetalle), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsientoContableDetalle_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdAsientoContableDetalle", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAsientoContableDetalle>(typeof(CAsientoContableDetalle), pConexion);
        foreach (CAsientoContableDetalle O in Obten.ListaRegistros)
        {
            idAsientoContableDetalle = O.IdAsientoContableDetalle;
            idAsientoContable = O.IdAsientoContable;
            cuentaContable = O.CuentaContable;
            idCliente = O.IdCliente;
            idProveedor = O.IdProveedor;
            idCuentaBancaria = O.IdCuentaBancaria;
            idIVA = O.IdIVA;
            idSucursal = O.IdSucursal;
            idDivision = O.IdDivision;
            idTipoCompra = O.IdTipoCompra;
            idCuentaContable = O.IdCuentaContable;
            cargo = O.Cargo;
            abono = O.Abono;
            descripcionCuentaContable = O.DescripcionCuentaContable;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsientoContableDetalle_ConsultarFiltros";
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
        Obten.Llena<CAsientoContableDetalle>(typeof(CAsientoContableDetalle), pConexion);
        foreach (CAsientoContableDetalle O in Obten.ListaRegistros)
        {
            idAsientoContableDetalle = O.IdAsientoContableDetalle;
            idAsientoContable = O.IdAsientoContable;
            cuentaContable = O.CuentaContable;
            idCliente = O.IdCliente;
            idProveedor = O.IdProveedor;
            idCuentaBancaria = O.IdCuentaBancaria;
            idIVA = O.IdIVA;
            idSucursal = O.IdSucursal;
            idDivision = O.IdDivision;
            idTipoCompra = O.IdTipoCompra;
            idCuentaContable = O.IdCuentaContable;
            cargo = O.Cargo;
            abono = O.Abono;
            descripcionCuentaContable = O.DescripcionCuentaContable;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsientoContableDetalle_ConsultarFiltros";
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
        Obten.Llena<CAsientoContableDetalle>(typeof(CAsientoContableDetalle), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_AsientoContableDetalle_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAsientoContableDetalle", 0);
        Agregar.StoredProcedure.Parameters["@pIdAsientoContableDetalle"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAsientoContable", idAsientoContable);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdIVA", idIVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", idTipoCompra);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaContable", idCuentaContable);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCargo", cargo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pAbono", abono);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcionCuentaContable", descripcionCuentaContable);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idAsientoContableDetalle = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdAsientoContableDetalle"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_AsientoContableDetalle_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdAsientoContableDetalle", idAsientoContableDetalle);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdAsientoContable", idAsientoContable);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdIVA", idIVA);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", idTipoCompra);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaContable", idCuentaContable);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCargo", cargo);
        Editar.StoredProcedure.Parameters.AddWithValue("@pAbono", abono);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcionCuentaContable", descripcionCuentaContable);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_AsientoContableDetalle_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdAsientoContableDetalle", idAsientoContableDetalle);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}
