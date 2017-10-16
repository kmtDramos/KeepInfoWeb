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

public partial class CAtributoAddenda
{
    //Propiedades Privadas
    private int idAtributoAddenda;
    private int orden;
    private string nodoABuscar;
    private string formato;
    private string atributoReferencia;
    private bool esAddenda;
    private string atributoAddenda;
    private string valor;
    private int idEstructuraAddenda;
    private bool esConstante;
    private bool esVisible;
    private string tipoDato;
    private bool baja;

    //Propiedades
    public int IdAtributoAddenda
    {
        get { return idAtributoAddenda; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idAtributoAddenda = value;
        }
    }

    public int Orden
    {
        get { return orden; }
        set
        {
            if (value < 0)
            {
                return;
            }
            orden = value;
        }
    }

    public string NodoABuscar
    {
        get { return nodoABuscar; }
        set
        {
            nodoABuscar = value;
        }
    }

    public string Formato
    {
        get { return formato; }
        set
        {
            formato = value;
        }
    }

    public string AtributoReferencia
    {
        get { return atributoReferencia; }
        set
        {
            atributoReferencia = value;
        }
    }

    public bool EsAddenda
    {
        get { return esAddenda; }
        set { esAddenda = value; }
    }

    public string AtributoAddenda
    {
        get { return atributoAddenda; }
        set
        {
            atributoAddenda = value;
        }
    }

    public string Valor
    {
        get { return valor; }
        set
        {
            valor = value;
        }
    }

    public int IdEstructuraAddenda
    {
        get { return idEstructuraAddenda; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idEstructuraAddenda = value;
        }
    }

    public bool EsConstante
    {
        get { return esConstante; }
        set { esConstante = value; }
    }

    public bool EsVisible
    {
        get { return esVisible; }
        set { esVisible = value; }
    }

    public string TipoDato
    {
        get { return tipoDato; }
        set
        {
            tipoDato = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CAtributoAddenda()
    {
        idAtributoAddenda = 0;
        orden = 0;
        nodoABuscar = "";
        formato = "";
        atributoReferencia = "";
        esAddenda = false;
        atributoAddenda = "";
        valor = "";
        idEstructuraAddenda = 0;
        esConstante = false;
        esVisible = false;
        tipoDato = "";
        baja = false;
    }

    public CAtributoAddenda(int pIdAtributoAddenda)
    {
        idAtributoAddenda = pIdAtributoAddenda;
        orden = 0;
        nodoABuscar = "";
        formato = "";
        atributoReferencia = "";
        esAddenda = false;
        atributoAddenda = "";
        valor = "";
        idEstructuraAddenda = 0;
        esConstante = false;
        esVisible = false;
        tipoDato = "";
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AtributoAddenda_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAtributoAddenda>(typeof(CAtributoAddenda), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AtributoAddenda_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CAtributoAddenda>(typeof(CAtributoAddenda), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AtributoAddenda_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdAtributoAddenda", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAtributoAddenda>(typeof(CAtributoAddenda), pConexion);
        foreach (CAtributoAddenda O in Obten.ListaRegistros)
        {
            idAtributoAddenda = O.IdAtributoAddenda;
            orden = O.Orden;
            nodoABuscar = O.NodoABuscar;
            formato = O.Formato;
            atributoReferencia = O.AtributoReferencia;
            esAddenda = O.EsAddenda;
            atributoAddenda = O.AtributoAddenda;
            valor = O.Valor;
            idEstructuraAddenda = O.IdEstructuraAddenda;
            esConstante = O.EsConstante;
            esVisible = O.EsVisible;
            tipoDato = O.TipoDato;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AtributoAddenda_ConsultarFiltros";
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
        Obten.Llena<CAtributoAddenda>(typeof(CAtributoAddenda), pConexion);
        foreach (CAtributoAddenda O in Obten.ListaRegistros)
        {
            idAtributoAddenda = O.IdAtributoAddenda;
            orden = O.Orden;
            nodoABuscar = O.NodoABuscar;
            formato = O.Formato;
            atributoReferencia = O.AtributoReferencia;
            esAddenda = O.EsAddenda;
            atributoAddenda = O.AtributoAddenda;
            valor = O.Valor;
            idEstructuraAddenda = O.IdEstructuraAddenda;
            esConstante = O.EsConstante;
            esVisible = O.EsVisible;
            tipoDato = O.TipoDato;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AtributoAddenda_ConsultarFiltros";
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
        Obten.Llena<CAtributoAddenda>(typeof(CAtributoAddenda), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_AtributoAddenda_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAtributoAddenda", 0);
        Agregar.StoredProcedure.Parameters["@pIdAtributoAddenda"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNodoABuscar", nodoABuscar);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pFormato", formato);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pAtributoReferencia", atributoReferencia);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pEsAddenda", esAddenda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pAtributoAddenda", atributoAddenda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pValor", valor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstructuraAddenda", idEstructuraAddenda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pEsConstante", esConstante);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pEsVisible", esVisible);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoDato", tipoDato);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idAtributoAddenda = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdAtributoAddenda"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_AtributoAddenda_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdAtributoAddenda", idAtributoAddenda);
        Editar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNodoABuscar", nodoABuscar);
        Editar.StoredProcedure.Parameters.AddWithValue("@pFormato", formato);
        Editar.StoredProcedure.Parameters.AddWithValue("@pAtributoReferencia", atributoReferencia);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEsAddenda", esAddenda);
        Editar.StoredProcedure.Parameters.AddWithValue("@pAtributoAddenda", atributoAddenda);
        Editar.StoredProcedure.Parameters.AddWithValue("@pValor", valor);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstructuraAddenda", idEstructuraAddenda);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEsConstante", esConstante);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEsVisible", esVisible);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoDato", tipoDato);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_AtributoAddenda_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdAtributoAddenda", idAtributoAddenda);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}
