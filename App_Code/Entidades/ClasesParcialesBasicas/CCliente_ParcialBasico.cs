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

public partial class CCliente
{
    //Propiedades Privadas
    private int idCliente;
    private DateTime fechaAlta;
    private DateTime fechaModificacion;
    private string limiteDeCredito;
    private string correo;
    private int idUsuarioAlta;
    private int idUsuarioModifico;
    private int idOrganizacion;
    private int idTipoCliente;
    private int idFormaContacto;
    private int idCondicionPago;
    private decimal iVAActual;
    private int idTipoGarantia;
    private int idUsuarioAgente;
    private string cuentaContable;
    private string cuentaContableDolares;
    private int idCampana;
    private bool esCliente;
    private bool baja;

    //Propiedades
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

    public DateTime FechaAlta
    {
        get { return fechaAlta; }
        set { fechaAlta = value; }
    }

    public DateTime FechaModificacion
    {
        get { return fechaModificacion; }
        set { fechaModificacion = value; }
    }

    public string LimiteDeCredito
    {
        get { return limiteDeCredito; }
        set
        {
            limiteDeCredito = value;
        }
    }

    public string Correo
    {
        get { return correo; }
        set
        {
            correo = value;
        }
    }

    public int IdUsuarioAlta
    {
        get { return idUsuarioAlta; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUsuarioAlta = value;
        }
    }

    public int IdUsuarioModifico
    {
        get { return idUsuarioModifico; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUsuarioModifico = value;
        }
    }

    public int IdOrganizacion
    {
        get { return idOrganizacion; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idOrganizacion = value;
        }
    }

    public int IdTipoCliente
    {
        get { return idTipoCliente; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoCliente = value;
        }
    }

    public int IdFormaContacto
    {
        get { return idFormaContacto; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idFormaContacto = value;
        }
    }

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

    public decimal IVAActual
    {
        get { return iVAActual; }
        set
        {
            if (value < 0)
            {
                return;
            }
            iVAActual = value;
        }
    }

    public int IdTipoGarantia
    {
        get { return idTipoGarantia; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoGarantia = value;
        }
    }

    public int IdUsuarioAgente
    {
        get { return idUsuarioAgente; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUsuarioAgente = value;
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

    public string CuentaContableDolares
    {
        get { return cuentaContableDolares; }
        set
        {
            cuentaContableDolares = value;
        }
    }

    public int IdCampana
    {
        get { return idCampana; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idCampana = value;
        }
    }

    public bool EsCliente
    {
        get { return esCliente; }
        set { esCliente = value; }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CCliente()
    {
        idCliente = 0;
        fechaAlta = new DateTime(1, 1, 1);
        fechaModificacion = new DateTime(1, 1, 1);
        limiteDeCredito = "";
        correo = "";
        idUsuarioAlta = 0;
        idUsuarioModifico = 0;
        idOrganizacion = 0;
        idTipoCliente = 0;
        idFormaContacto = 0;
        idCondicionPago = 0;
        iVAActual = 0;
        idTipoGarantia = 0;
        idUsuarioAgente = 0;
        cuentaContable = "";
        cuentaContableDolares = "";
        idCampana = 0;
        esCliente = false;
        baja = false;
    }

    public CCliente(int pIdCliente)
    {
        idCliente = pIdCliente;
        fechaAlta = new DateTime(1, 1, 1);
        fechaModificacion = new DateTime(1, 1, 1);
        limiteDeCredito = "";
        correo = "";
        idUsuarioAlta = 0;
        idUsuarioModifico = 0;
        idOrganizacion = 0;
        idTipoCliente = 0;
        idFormaContacto = 0;
        idCondicionPago = 0;
        iVAActual = 0;
        idTipoGarantia = 0;
        idUsuarioAgente = 0;
        cuentaContable = "";
        cuentaContableDolares = "";
        idCampana = 0;
        esCliente = false;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Cliente_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CCliente>(typeof(CCliente), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Cliente_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CCliente>(typeof(CCliente), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Cliente_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdCliente", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CCliente>(typeof(CCliente), pConexion);
        foreach (CCliente O in Obten.ListaRegistros)
        {
            idCliente = O.IdCliente;
            fechaAlta = O.FechaAlta;
            fechaModificacion = O.FechaModificacion;
            limiteDeCredito = O.LimiteDeCredito;
            correo = O.Correo;
            idUsuarioAlta = O.IdUsuarioAlta;
            idUsuarioModifico = O.IdUsuarioModifico;
            idOrganizacion = O.IdOrganizacion;
            idTipoCliente = O.IdTipoCliente;
            idFormaContacto = O.IdFormaContacto;
            idCondicionPago = O.IdCondicionPago;
            iVAActual = O.IVAActual;
            idTipoGarantia = O.IdTipoGarantia;
            idUsuarioAgente = O.IdUsuarioAgente;
            cuentaContable = O.CuentaContable;
            cuentaContableDolares = O.CuentaContableDolares;
            idCampana = O.IdCampana;
            esCliente = O.EsCliente;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Cliente_ConsultarFiltros";
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
        Obten.Llena<CCliente>(typeof(CCliente), pConexion);
        foreach (CCliente O in Obten.ListaRegistros)
        {
            idCliente = O.IdCliente;
            fechaAlta = O.FechaAlta;
            fechaModificacion = O.FechaModificacion;
            limiteDeCredito = O.LimiteDeCredito;
            correo = O.Correo;
            idUsuarioAlta = O.IdUsuarioAlta;
            idUsuarioModifico = O.IdUsuarioModifico;
            idOrganizacion = O.IdOrganizacion;
            idTipoCliente = O.IdTipoCliente;
            idFormaContacto = O.IdFormaContacto;
            idCondicionPago = O.IdCondicionPago;
            iVAActual = O.IVAActual;
            idTipoGarantia = O.IdTipoGarantia;
            idUsuarioAgente = O.IdUsuarioAgente;
            cuentaContable = O.CuentaContable;
            cuentaContableDolares = O.CuentaContableDolares;
            idCampana = O.IdCampana;
            esCliente = O.EsCliente;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Cliente_ConsultarFiltros";
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
        Obten.Llena<CCliente>(typeof(CCliente), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_Cliente_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", 0);
        Agregar.StoredProcedure.Parameters["@pIdCliente"].Direction = ParameterDirection.Output;
        if (fechaAlta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        if (fechaModificacion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pLimiteDeCredito", limiteDeCredito);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCliente", idTipoCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFormaContacto", idFormaContacto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", idCondicionPago);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIVAActual", iVAActual);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoGarantia", idTipoGarantia);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaContableDolares", cuentaContableDolares);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCampana", idCampana);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pEsCliente", esCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idCliente = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCliente"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_Cliente_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        if (fechaAlta.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        if (fechaModificacion.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pLimiteDeCredito", limiteDeCredito);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCliente", idTipoCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdFormaContacto", idFormaContacto);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", idCondicionPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIVAActual", iVAActual);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoGarantia", idTipoGarantia);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaContableDolares", cuentaContableDolares);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCampana", idCampana);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEsCliente", esCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_Cliente_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}