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

public partial class CAlmacen
{
    //Propiedades Privadas
    private int idAlmacen;
    private string almacen;
    private string calle;
    private string numeroExterior;
    private string colonia;
    private string codigoPostal;
    private string telefono;
    private string correo;
    private int idEmpresa;
    private string numeroInterior;
    private int idPais;
    private int idEstado;
    private int idMunicipio;
    private int idTipoAlmacen;
    private bool disponibleVenta;
    private bool baja;

    //Propiedades
    public int IdAlmacen
    {
        get { return idAlmacen; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idAlmacen = value;
        }
    }

    public string Almacen
    {
        get { return almacen; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            almacen = value;
        }
    }

    public string Calle
    {
        get { return calle; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            calle = value;
        }
    }

    public string NumeroExterior
    {
        get { return numeroExterior; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            numeroExterior = value;
        }
    }

    public string Colonia
    {
        get { return colonia; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            colonia = value;
        }
    }

    public string CodigoPostal
    {
        get { return codigoPostal; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            codigoPostal = value;
        }
    }

    public string Telefono
    {
        get { return telefono; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            telefono = value;
        }
    }

    public string Correo
    {
        get { return correo; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            correo = value;
        }
    }

    public int IdEmpresa
    {
        get { return idEmpresa; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idEmpresa = value;
        }
    }

    public string NumeroInterior
    {
        get { return numeroInterior; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            numeroInterior = value;
        }
    }

    public int IdPais
    {
        get { return idPais; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idPais = value;
        }
    }

    public int IdEstado
    {
        get { return idEstado; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idEstado = value;
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

    public int IdTipoAlmacen
    {
        get { return idTipoAlmacen; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoAlmacen = value;
        }
    }

    public bool DisponibleVenta
    {
        get { return disponibleVenta; }
        set { disponibleVenta = value; }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CAlmacen()
    {
        idAlmacen = 0;
        almacen = "";
        calle = "";
        numeroExterior = "";
        colonia = "";
        codigoPostal = "";
        telefono = "";
        correo = "";
        idEmpresa = 0;
        numeroInterior = "";
        idPais = 0;
        idEstado = 0;
        idMunicipio = 0;
        idTipoAlmacen = 0;
        disponibleVenta = false;
        baja = false;
    }

    public CAlmacen(int pIdAlmacen)
    {
        idAlmacen = pIdAlmacen;
        almacen = "";
        calle = "";
        numeroExterior = "";
        colonia = "";
        codigoPostal = "";
        telefono = "";
        correo = "";
        idEmpresa = 0;
        numeroInterior = "";
        idPais = 0;
        idEstado = 0;
        idMunicipio = 0;
        idTipoAlmacen = 0;
        disponibleVenta = false;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Almacen_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAlmacen>(typeof(CAlmacen), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Almacen_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CAlmacen>(typeof(CAlmacen), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Almacen_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAlmacen>(typeof(CAlmacen), pConexion);
        foreach (CAlmacen O in Obten.ListaRegistros)
        {
            idAlmacen = O.IdAlmacen;
            almacen = O.Almacen;
            calle = O.Calle;
            numeroExterior = O.NumeroExterior;
            colonia = O.Colonia;
            codigoPostal = O.CodigoPostal;
            telefono = O.Telefono;
            correo = O.Correo;
            idEmpresa = O.IdEmpresa;
            numeroInterior = O.NumeroInterior;
            idPais = O.IdPais;
            idEstado = O.IdEstado;
            idMunicipio = O.IdMunicipio;
            idTipoAlmacen = O.IdTipoAlmacen;
            disponibleVenta = O.DisponibleVenta;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Almacen_ConsultarFiltros";
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
        Obten.Llena<CAlmacen>(typeof(CAlmacen), pConexion);
        foreach (CAlmacen O in Obten.ListaRegistros)
        {
            idAlmacen = O.IdAlmacen;
            almacen = O.Almacen;
            calle = O.Calle;
            numeroExterior = O.NumeroExterior;
            colonia = O.Colonia;
            codigoPostal = O.CodigoPostal;
            telefono = O.Telefono;
            correo = O.Correo;
            idEmpresa = O.IdEmpresa;
            numeroInterior = O.NumeroInterior;
            idPais = O.IdPais;
            idEstado = O.IdEstado;
            idMunicipio = O.IdMunicipio;
            idTipoAlmacen = O.IdTipoAlmacen;
            disponibleVenta = O.DisponibleVenta;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Almacen_ConsultarFiltros";
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
        Obten.Llena<CAlmacen>(typeof(CAlmacen), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetosAlmacenAsignadoUsuario(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Almacen_ConsultarAlmacenAsignadoUsuario";
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
        Obten.Llena<CAlmacen>(typeof(CAlmacen), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_Almacen_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", 0);
        Agregar.StoredProcedure.Parameters["@pIdAlmacen"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pAlmacen", almacen);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCalle", calle);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroExterior", numeroExterior);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pColonia", colonia);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostal", codigoPostal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTelefono", telefono);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", idEmpresa);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroInterior", numeroInterior);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPais", idPais);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstado", idEstado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipio", idMunicipio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoAlmacen", idTipoAlmacen);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDisponibleVenta", disponibleVenta);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idAlmacen = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdAlmacen"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_Almacen_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
        Editar.StoredProcedure.Parameters.AddWithValue("@pAlmacen", almacen);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCalle", calle);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroExterior", numeroExterior);
        Editar.StoredProcedure.Parameters.AddWithValue("@pColonia", colonia);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostal", codigoPostal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTelefono", telefono);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", idEmpresa);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroInterior", numeroInterior);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdPais", idPais);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstado", idEstado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipio", idMunicipio);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoAlmacen", idTipoAlmacen);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDisponibleVenta", disponibleVenta);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_Almacen_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}