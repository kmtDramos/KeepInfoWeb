using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;

public partial class CClaseAtributo
{
    //Propiedades Privadas
    private int idClaseAtributo;
    private string atributo;
    private string tipoAtributo;
    private string longitud;
    private string decimales;
    private string llavePrimaria;
    private string identidad;
    private int idClaseGenerador;

    //Propiedades
    public int IdClaseAtributo
    {
        get { return idClaseAtributo; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idClaseAtributo = value;
        }
    }

    public string Atributo
    {
        get { return atributo; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            atributo = value;
        }
    }

    public string TipoAtributo
    {
        get { return tipoAtributo; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            tipoAtributo = value;
        }
    }

    public string Longitud
    {
        get { return longitud; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            longitud = value;
        }
    }

    public string Decimales
    {
        get { return decimales; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            decimales = value;
        }
    }

    public string LlavePrimaria
    {
        get { return llavePrimaria; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            llavePrimaria = value;
        }
    }

    public string Identidad
    {
        get { return identidad; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            identidad = value;
        }
    }

    public int IdClaseGenerador
    {
        get { return idClaseGenerador; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idClaseGenerador = value;
        }
    }

    //Constructores
    public CClaseAtributo()
    {
        IdClaseAtributo = 0;
        atributo = "";
        tipoAtributo = "";
        longitud = "";
        decimales = "";
        llavePrimaria = "";
        identidad = "";
        idClaseGenerador = 1;
    }

    public CClaseAtributo(int pIdClaseAtributo)
    {
        idClaseAtributo = pIdClaseAtributo;
        atributo = "";
        tipoAtributo = "";
        longitud = "";
        decimales = "";
        llavePrimaria = "";
        identidad = "";
        idClaseGenerador = 1;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ClaseAtributo_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Llena<CClaseAtributo>(typeof(CClaseAtributo), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ClaseAtributo_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CClaseAtributo>(typeof(CClaseAtributo), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ClaseAtributo_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdClaseAtributo", pIdentificador);
        Obten.Llena<CClaseAtributo>(typeof(CClaseAtributo), pConexion);
        foreach (CClaseAtributo O in Obten.ListaRegistros)
        {
            idClaseAtributo = O.IdClaseAtributo;
            atributo = O.Atributo;
            tipoAtributo = O.TipoAtributo;
            longitud = O.Longitud;
            decimales = O.Decimales;
            llavePrimaria = O.LlavePrimaria;
            identidad = O.Identidad;
            idClaseGenerador = O.IdClaseGenerador;
        }
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_ClaseAtributo_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pAtributo", atributo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoAtributo", tipoAtributo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pLongitud", longitud);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDecimales", decimales);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pLlavePrimaria", llavePrimaria);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdentidad", identidad);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdClaseGenerador", idClaseGenerador);
        Agregar.Insert(pConexion);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_ClaseAtributo_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdClaseAtributo", idClaseAtributo);
        Editar.StoredProcedure.Parameters.AddWithValue("@pAtributo", atributo);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoAtributo", tipoAtributo);
        Editar.StoredProcedure.Parameters.AddWithValue("@pLongitud", longitud);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDecimales", decimales);
        Editar.StoredProcedure.Parameters.AddWithValue("@pLlavePrimaria", llavePrimaria);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdentidad", identidad);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdClaseGenerador", idClaseGenerador);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_ClaseAtributo_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdClaseAtributo", idClaseAtributo);
        Eliminar.Delete(pConexion);
    }
}