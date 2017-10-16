using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;

public partial class CClaseGenerador
{
    //Propiedades Privadas
    private int idClaseGenerador;
    private string clase;
    private bool bloqueo;
    private bool manejaBaja;
    private string abreviatura;

    //Propiedades
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

    public string Clase
    {
        get { return clase; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            clase = value;
        }
    }

    public bool Bloqueo
    {
        get { return bloqueo; }
        set { bloqueo = value; }
    }

    public bool ManejaBaja
    {
        get { return manejaBaja; }
        set { manejaBaja = value; }
    }

    public string Abreviatura
    {
        get { return abreviatura; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            abreviatura = value;
        }
    }

    //Constructores
    public CClaseGenerador()
    {
        IdClaseGenerador = 0;
        clase = "";
        bloqueo = false;
        manejaBaja = false;
        abreviatura = "";
    }

    public CClaseGenerador(int pIdClaseGenerador)
    {
        idClaseGenerador = pIdClaseGenerador;
        clase = "";
        bloqueo = false;
        manejaBaja = false;
        abreviatura = "";
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ClaseGenerador_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Llena<CClaseGenerador>(typeof(CClaseGenerador), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ClaseGenerador_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CClaseGenerador>(typeof(CClaseGenerador), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ClaseGenerador_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdClaseGenerador", pIdentificador);
        Obten.Llena<CClaseGenerador>(typeof(CClaseGenerador), pConexion);
        foreach (CClaseGenerador O in Obten.ListaRegistros)
        {
            idClaseGenerador = O.IdClaseGenerador;
            clase = O.Clase;
            bloqueo = O.Bloqueo;
            manejaBaja = O.ManejaBaja;
            abreviatura = O.Abreviatura;
        }
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_ClaseGenerador_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdClaseGenerador", 0);
        Agregar.StoredProcedure.Parameters["@pIdClaseGenerador"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pClase", clase);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBloqueo", bloqueo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pManejaBaja", manejaBaja);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pAbreviatura", abreviatura);
        Agregar.Insert(pConexion);
        idClaseGenerador = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdClaseGenerador"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_ClaseGenerador_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdClaseGenerador", idClaseGenerador);
        Editar.StoredProcedure.Parameters.AddWithValue("@pClase", clase);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBloqueo", bloqueo);
        Editar.StoredProcedure.Parameters.AddWithValue("@pManejaBaja", manejaBaja);
        Editar.StoredProcedure.Parameters.AddWithValue("@pAbreviatura", abreviatura);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_ClaseGenerador_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdClaseGenerador", idClaseGenerador);
        Eliminar.Delete(pConexion);
    }
}