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

public partial class CContactosDeOrganizacion
{
    //Propiedades Privadas
    private int idContactosDeOrganizacion;
    private string nombre;
    private string apellidoPaterno;
    private string apellidoMaterno;
    private string puesto;
    private string celular;
    private string fax;
    private DateTime cumpleanos;
    private string nota;
    private int idCliente;
    private int idProveedor;
    private bool baja;

    //Propiedades
    public int IdContactosDeOrganizacion
    {
        get { return idContactosDeOrganizacion; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idContactosDeOrganizacion = value;
        }
    }

    public string Nombre
    {
        get { return nombre; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            nombre = value;
        }
    }

    public string ApellidoPaterno
    {
        get { return apellidoPaterno; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            apellidoPaterno = value;
        }
    }

    public string ApellidoMaterno
    {
        get { return apellidoMaterno; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            apellidoMaterno = value;
        }
    }

    public string Puesto
    {
        get { return puesto; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            puesto = value;
        }
    }

    public string Celular
    {
        get { return celular; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            celular = value;
        }
    }

    public string Fax
    {
        get { return fax; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            fax = value;
        }
    }

    public DateTime Cumpleanos
    {
        get { return cumpleanos; }
        set { cumpleanos = value; }
    }

    public string Nota
    {
        get { return nota; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            nota = value;
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

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CContactosDeOrganizacion()
    {
        idContactosDeOrganizacion = 0;
        nombre = "";
        apellidoPaterno = "";
        apellidoMaterno = "";
        puesto = "";
        celular = "";
        fax = "";
        cumpleanos = new DateTime(1, 1, 1);
        nota = "";
        idCliente = 0;
        idProveedor = 0;
        baja = false;
    }

    public CContactosDeOrganizacion(int pIdContactosDeOrganizacion)
    {
        idContactosDeOrganizacion = pIdContactosDeOrganizacion;
        nombre = "";
        apellidoPaterno = "";
        apellidoMaterno = "";
        puesto = "";
        celular = "";
        fax = "";
        cumpleanos = new DateTime(1, 1, 1);
        nota = "";
        idCliente = 0;
        idProveedor = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ContactosDeOrganizacion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CContactosDeOrganizacion>(typeof(CContactosDeOrganizacion), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ContactosDeOrganizacion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CContactosDeOrganizacion>(typeof(CContactosDeOrganizacion), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ContactosDeOrganizacion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdContactosDeOrganizacion", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CContactosDeOrganizacion>(typeof(CContactosDeOrganizacion), pConexion);
        foreach (CContactosDeOrganizacion O in Obten.ListaRegistros)
        {
            idContactosDeOrganizacion = O.IdContactosDeOrganizacion;
            nombre = O.Nombre;
            apellidoPaterno = O.ApellidoPaterno;
            apellidoMaterno = O.ApellidoMaterno;
            puesto = O.Puesto;
            celular = O.Celular;
            fax = O.Fax;
            cumpleanos = O.Cumpleanos;
            nota = O.Nota;
            idCliente = O.IdCliente;
            idProveedor = O.IdProveedor;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ContactosDeOrganizacion_ConsultarFiltros";
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
        Obten.Llena<CContactosDeOrganizacion>(typeof(CContactosDeOrganizacion), pConexion);
        foreach (CContactosDeOrganizacion O in Obten.ListaRegistros)
        {
            idContactosDeOrganizacion = O.IdContactosDeOrganizacion;
            nombre = O.Nombre;
            apellidoPaterno = O.ApellidoPaterno;
            apellidoMaterno = O.ApellidoMaterno;
            puesto = O.Puesto;
            celular = O.Celular;
            fax = O.Fax;
            cumpleanos = O.Cumpleanos;
            nota = O.Nota;
            idCliente = O.IdCliente;
            idProveedor = O.IdProveedor;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ContactosDeOrganizacion_ConsultarFiltros";
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
        Obten.Llena<CContactosDeOrganizacion>(typeof(CContactosDeOrganizacion), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_ContactosDeOrganizacion_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdContactosDeOrganizacion", 0);
        Agregar.StoredProcedure.Parameters["@pIdContactosDeOrganizacion"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNombre", nombre);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pApellidoPaterno", apellidoPaterno);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pApellidoMaterno", apellidoMaterno);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pPuesto", puesto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCelular", celular);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pFax", fax);
        if (cumpleanos.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pCumpleanos", cumpleanos);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idContactosDeOrganizacion = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdContactosDeOrganizacion"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_ContactosDeOrganizacion_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdContactosDeOrganizacion", idContactosDeOrganizacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNombre", nombre);
        Editar.StoredProcedure.Parameters.AddWithValue("@pApellidoPaterno", apellidoPaterno);
        Editar.StoredProcedure.Parameters.AddWithValue("@pApellidoMaterno", apellidoMaterno);
        Editar.StoredProcedure.Parameters.AddWithValue("@pPuesto", puesto);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCelular", celular);
        Editar.StoredProcedure.Parameters.AddWithValue("@pFax", fax);
        if (cumpleanos.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pCumpleanos", cumpleanos);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_ContactosDeOrganizacion_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdContactosDeOrganizacion", idContactosDeOrganizacion);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}