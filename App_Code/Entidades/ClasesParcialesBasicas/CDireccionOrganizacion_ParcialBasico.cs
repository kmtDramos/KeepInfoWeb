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

public partial class CDireccionOrganizacion
{
    //Propiedades Privadas
    private int idDireccionOrganizacion;
    private string calle;
    private string numeroExterior;
    private string numeroInterior;
    private string colonia;
    private string codigoPostal;
    private string conmutadorTelefono;
    private string referencia;
    private int idTipoDireccion;
    private int idMunicipio;
    private int idOrganizacion;
    private int idLocalidad;
    private string descripcion;
    private bool baja;

    //Propiedades
    public int IdDireccionOrganizacion
    {
        get { return idDireccionOrganizacion; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idDireccionOrganizacion = value;
        }
    }

    public string Calle
    {
        get { return calle; }
        set
        {
            calle = value;
        }
    }

    public string NumeroExterior
    {
        get { return numeroExterior; }
        set
        {
            numeroExterior = value;
        }
    }

    public string NumeroInterior
    {
        get { return numeroInterior; }
        set
        {
            numeroInterior = value;
        }
    }

    public string Colonia
    {
        get { return colonia; }
        set
        {
            colonia = value;
        }
    }

    public string CodigoPostal
    {
        get { return codigoPostal; }
        set
        {
            codigoPostal = value;
        }
    }

    public string ConmutadorTelefono
    {
        get { return conmutadorTelefono; }
        set
        {
            conmutadorTelefono = value;
        }
    }

    public string Referencia
    {
        get { return referencia; }
        set
        {
            referencia = value;
        }
    }

    public int IdTipoDireccion
    {
        get { return idTipoDireccion; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoDireccion = value;
        }
    }

    public int IdMunicipio
    {
        get { return idMunicipio; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idMunicipio = value;
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

    public int IdLocalidad
    {
        get { return idLocalidad; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idLocalidad = value;
        }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            descripcion = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CDireccionOrganizacion()
    {
        idDireccionOrganizacion = 0;
        calle = "";
        numeroExterior = "";
        numeroInterior = "";
        colonia = "";
        codigoPostal = "";
        conmutadorTelefono = "";
        referencia = "";
        idTipoDireccion = 0;
        idMunicipio = 0;
        idOrganizacion = 0;
        idLocalidad = 0;
        descripcion = "";
        baja = false;
    }

    public CDireccionOrganizacion(int pIdDireccionOrganizacion)
    {
        idDireccionOrganizacion = pIdDireccionOrganizacion;
        calle = "";
        numeroExterior = "";
        numeroInterior = "";
        colonia = "";
        codigoPostal = "";
        conmutadorTelefono = "";
        referencia = "";
        idTipoDireccion = 0;
        idMunicipio = 0;
        idOrganizacion = 0;
        idLocalidad = 0;
        descripcion = "";
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DireccionOrganizacion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CDireccionOrganizacion>(typeof(CDireccionOrganizacion), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DireccionOrganizacion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CDireccionOrganizacion>(typeof(CDireccionOrganizacion), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DireccionOrganizacion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdDireccionOrganizacion", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CDireccionOrganizacion>(typeof(CDireccionOrganizacion), pConexion);
        foreach (CDireccionOrganizacion O in Obten.ListaRegistros)
        {
            idDireccionOrganizacion = O.IdDireccionOrganizacion;
            calle = O.Calle;
            numeroExterior = O.NumeroExterior;
            numeroInterior = O.NumeroInterior;
            colonia = O.Colonia;
            codigoPostal = O.CodigoPostal;
            conmutadorTelefono = O.ConmutadorTelefono;
            referencia = O.Referencia;
            idTipoDireccion = O.IdTipoDireccion;
            idMunicipio = O.IdMunicipio;
            idOrganizacion = O.IdOrganizacion;
            idLocalidad = O.IdLocalidad;
            descripcion = O.Descripcion;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DireccionOrganizacion_ConsultarFiltros";
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
        Obten.Llena<CDireccionOrganizacion>(typeof(CDireccionOrganizacion), pConexion);
        foreach (CDireccionOrganizacion O in Obten.ListaRegistros)
        {
            idDireccionOrganizacion = O.IdDireccionOrganizacion;
            calle = O.Calle;
            numeroExterior = O.NumeroExterior;
            numeroInterior = O.NumeroInterior;
            colonia = O.Colonia;
            codigoPostal = O.CodigoPostal;
            conmutadorTelefono = O.ConmutadorTelefono;
            referencia = O.Referencia;
            idTipoDireccion = O.IdTipoDireccion;
            idMunicipio = O.IdMunicipio;
            idOrganizacion = O.IdOrganizacion;
            idLocalidad = O.IdLocalidad;
            descripcion = O.Descripcion;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DireccionOrganizacion_ConsultarFiltros";
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
        Obten.Llena<CDireccionOrganizacion>(typeof(CDireccionOrganizacion), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_DireccionOrganizacion_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDireccionOrganizacion", 0);
        Agregar.StoredProcedure.Parameters["@pIdDireccionOrganizacion"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCalle", calle);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroExterior", numeroExterior);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroInterior", numeroInterior);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pColonia", colonia);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostal", codigoPostal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pConmutadorTelefono", conmutadorTelefono);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoDireccion", idTipoDireccion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipio", idMunicipio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidad", idLocalidad);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idDireccionOrganizacion = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDireccionOrganizacion"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_DireccionOrganizacion_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdDireccionOrganizacion", idDireccionOrganizacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCalle", calle);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroExterior", numeroExterior);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroInterior", numeroInterior);
        Editar.StoredProcedure.Parameters.AddWithValue("@pColonia", colonia);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostal", codigoPostal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pConmutadorTelefono", conmutadorTelefono);
        Editar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoDireccion", idTipoDireccion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipio", idMunicipio);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidad", idLocalidad);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_DireccionOrganizacion_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDireccionOrganizacion", idDireccionOrganizacion);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}