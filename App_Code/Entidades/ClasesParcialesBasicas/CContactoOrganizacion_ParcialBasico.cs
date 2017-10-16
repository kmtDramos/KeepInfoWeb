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

public partial class CContactoOrganizacion
{
    //Propiedades Privadas
    private int idContactoOrganizacion;
    private string nombre;
    private string puesto;
    private DateTime cumpleanio;
    private string notas;
    private int idCliente;
    private int idProveedor;
    private int idOrganizacion;
    private bool baja;

    //Propiedades
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

    public DateTime Cumpleanio
    {
        get { return cumpleanio; }
        set { cumpleanio = value; }
    }

    public string Notas
    {
        get { return notas; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            notas = value;
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

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CContactoOrganizacion()
    {
        idContactoOrganizacion = 0;
        nombre = "";
        puesto = "";
        cumpleanio = new DateTime(1, 1, 1);
        notas = "";
        idCliente = 0;
        idProveedor = 0;
        idOrganizacion = 0;
        baja = false;
    }

    public CContactoOrganizacion(int pIdContactoOrganizacion)
    {
        idContactoOrganizacion = pIdContactoOrganizacion;
        nombre = "";
        puesto = "";
        cumpleanio = new DateTime(1, 1, 1);
        notas = "";
        idCliente = 0;
        idProveedor = 0;
        idOrganizacion = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ContactoOrganizacion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CContactoOrganizacion>(typeof(CContactoOrganizacion), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ContactoOrganizacion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CContactoOrganizacion>(typeof(CContactoOrganizacion), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ContactoOrganizacion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CContactoOrganizacion>(typeof(CContactoOrganizacion), pConexion);
        foreach (CContactoOrganizacion O in Obten.ListaRegistros)
        {
            idContactoOrganizacion = O.IdContactoOrganizacion;
            nombre = O.Nombre;
            puesto = O.Puesto;
            cumpleanio = O.Cumpleanio;
            notas = O.Notas;
            idCliente = O.IdCliente;
            idProveedor = O.IdProveedor;
            idOrganizacion = O.IdOrganizacion;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ContactoOrganizacion_ConsultarFiltros";
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
        Obten.Llena<CContactoOrganizacion>(typeof(CContactoOrganizacion), pConexion);
        foreach (CContactoOrganizacion O in Obten.ListaRegistros)
        {
            idContactoOrganizacion = O.IdContactoOrganizacion;
            nombre = O.Nombre;
            puesto = O.Puesto;
            cumpleanio = O.Cumpleanio;
            notas = O.Notas;
            idCliente = O.IdCliente;
            idProveedor = O.IdProveedor;
            idOrganizacion = O.IdOrganizacion;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ContactoOrganizacion_ConsultarFiltros";
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
        Obten.Llena<CContactoOrganizacion>(typeof(CContactoOrganizacion), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_ContactoOrganizacion_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", 0);
        Agregar.StoredProcedure.Parameters["@pIdContactoOrganizacion"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNombre", nombre);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pPuesto", puesto);
        if (cumpleanio.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pCumpleanio", cumpleanio);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNotas", notas);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idContactoOrganizacion = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdContactoOrganizacion"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_ContactoOrganizacion_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNombre", nombre);
        Editar.StoredProcedure.Parameters.AddWithValue("@pPuesto", puesto);
        if (cumpleanio.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pCumpleanio", cumpleanio);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pNotas", notas);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_ContactoOrganizacion_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}