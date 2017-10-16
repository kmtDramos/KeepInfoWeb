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

public partial class CDescuentoProducto
{
    //Propiedades Privadas
    private int idDescuentoProducto;
    private int idProducto;
    private string descuentoProducto;
    private decimal descuento;
    private bool baja;

    //Propiedades
    public int IdDescuentoProducto
    {
        get { return idDescuentoProducto; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idDescuentoProducto = value;
        }
    }

    public int IdProducto
    {
        get { return idProducto; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idProducto = value;
        }
    }

    public string DescuentoProducto
    {
        get { return descuentoProducto; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            descuentoProducto = value;
        }
    }

    public decimal Descuento
    {
        get { return descuento; }
        set
        {
            if (value <= 0)
            {
                return;
            }
            descuento = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CDescuentoProducto()
    {
        idDescuentoProducto = 0;
        idProducto = 0;
        descuentoProducto = "";
        descuento = 0;
        baja = false;
    }

    public CDescuentoProducto(int pIdDescuentoProducto)
    {
        idDescuentoProducto = pIdDescuentoProducto;
        idProducto = 0;
        descuentoProducto = "";
        descuento = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DescuentoProducto_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CDescuentoProducto>(typeof(CDescuentoProducto), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DescuentoProducto_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CDescuentoProducto>(typeof(CDescuentoProducto), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DescuentoProducto_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoProducto", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CDescuentoProducto>(typeof(CDescuentoProducto), pConexion);
        foreach (CDescuentoProducto O in Obten.ListaRegistros)
        {
            idDescuentoProducto = O.IdDescuentoProducto;
            idProducto = O.IdProducto;
            descuentoProducto = O.DescuentoProducto;
            descuento = O.Descuento;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DescuentoProducto_ConsultarFiltros";
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
        Obten.Llena<CDescuentoProducto>(typeof(CDescuentoProducto), pConexion);
        foreach (CDescuentoProducto O in Obten.ListaRegistros)
        {
            idDescuentoProducto = O.IdDescuentoProducto;
            idProducto = O.IdProducto;
            descuentoProducto = O.DescuentoProducto;
            descuento = O.Descuento;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DescuentoProducto_ConsultarFiltros";
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
        Obten.Llena<CDescuentoProducto>(typeof(CDescuentoProducto), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_DescuentoProducto_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoProducto", 0);
        Agregar.StoredProcedure.Parameters["@pIdDescuentoProducto"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuentoProducto", descuentoProducto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idDescuentoProducto = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDescuentoProducto"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_DescuentoProducto_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoProducto", idDescuentoProducto);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDescuentoProducto", descuentoProducto);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_DescuentoProducto_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoProducto", idDescuentoProducto);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}