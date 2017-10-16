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

public partial class CConfiguracion
{
    //Propiedades Privadas
    private int idConfiguracion;
    private string descripcion;
    private string valorTexto;
    private int valorLogico;
    private bool baja;

    //Propiedades
    public int IdConfiguracion
    {
        get { return idConfiguracion; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idConfiguracion = value;
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

    public string ValorTexto
    {
        get { return valorTexto; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            valorTexto = value;
        }
    }

    public int ValorLogico
    {
        get { return valorLogico; }
        set
        {
            if (value < 0)
            {
                return;
            }
            valorLogico = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CConfiguracion()
    {
        idConfiguracion = 0;
        descripcion = "";
        valorTexto = "";
        valorLogico = 0;
        baja = false;
    }

    public CConfiguracion(int pIdConfiguracion)
    {
        idConfiguracion = pIdConfiguracion;
        descripcion = "";
        valorTexto = "";
        valorLogico = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Configuracion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CConfiguracion>(typeof(CConfiguracion), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Configuracion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CConfiguracion>(typeof(CConfiguracion), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Configuracion_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdConfiguracion", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CConfiguracion>(typeof(CConfiguracion), pConexion);
        foreach (CConfiguracion O in Obten.ListaRegistros)
        {
            idConfiguracion = O.IdConfiguracion;
            descripcion = O.Descripcion;
            valorTexto = O.ValorTexto;
            valorLogico = O.ValorLogico;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Configuracion_ConsultarFiltros";
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
        Obten.Llena<CConfiguracion>(typeof(CConfiguracion), pConexion);
        foreach (CConfiguracion O in Obten.ListaRegistros)
        {
            idConfiguracion = O.IdConfiguracion;
            descripcion = O.Descripcion;
            valorTexto = O.ValorTexto;
            valorLogico = O.ValorLogico;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Configuracion_ConsultarFiltros";
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
        Obten.Llena<CConfiguracion>(typeof(CConfiguracion), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_Configuracion_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdConfiguracion", 0);
        Agregar.StoredProcedure.Parameters["@pIdConfiguracion"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pValorTexto", valorTexto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pValorLogico", valorLogico);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idConfiguracion = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdConfiguracion"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_Configuracion_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdConfiguracion", idConfiguracion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pValorTexto", valorTexto);
        Editar.StoredProcedure.Parameters.AddWithValue("@pValorLogico", valorLogico);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_Configuracion_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdConfiguracion", idConfiguracion);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}