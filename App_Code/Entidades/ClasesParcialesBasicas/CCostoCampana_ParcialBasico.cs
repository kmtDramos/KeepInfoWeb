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

public partial class CCostoCampana
{
    //Propiedades Privadas
    private int idCostoCampana;
    private string costoCampana;
    private DateTime fechaAlta;
    private decimal monto;
    private int idCampana;
    private int idUsuario;
    private bool baja;

    //Propiedades
    public int IdCostoCampana
    {
        get { return idCostoCampana; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idCostoCampana = value;
        }
    }

    public string CostoCampana
    {
        get { return costoCampana; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            costoCampana = value;
        }
    }

    public DateTime FechaAlta
    {
        get { return fechaAlta; }
        set { fechaAlta = value; }
    }

    public decimal Monto
    {
        get { return monto; }
        set
        {
            if (value <= 0)
            {
                return;
            }
            monto = value;
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

    public int IdUsuario
    {
        get { return idUsuario; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUsuario = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CCostoCampana()
    {
        idCostoCampana = 0;
        costoCampana = "";
        fechaAlta = new DateTime(1, 1, 1);
        monto = 0;
        idCampana = 0;
        idUsuario = 0;
        baja = false;
    }

    public CCostoCampana(int pIdCostoCampana)
    {
        idCostoCampana = pIdCostoCampana;
        costoCampana = "";
        fechaAlta = new DateTime(1, 1, 1);
        monto = 0;
        idCampana = 0;
        idUsuario = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CostoCampana_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CCostoCampana>(typeof(CCostoCampana), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CostoCampana_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CCostoCampana>(typeof(CCostoCampana), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CostoCampana_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdCostoCampana", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CCostoCampana>(typeof(CCostoCampana), pConexion);
        foreach (CCostoCampana O in Obten.ListaRegistros)
        {
            idCostoCampana = O.IdCostoCampana;
            costoCampana = O.CostoCampana;
            fechaAlta = O.FechaAlta;
            monto = O.Monto;
            idCampana = O.IdCampana;
            idUsuario = O.IdUsuario;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CostoCampana_ConsultarFiltros";
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
        Obten.Llena<CCostoCampana>(typeof(CCostoCampana), pConexion);
        foreach (CCostoCampana O in Obten.ListaRegistros)
        {
            idCostoCampana = O.IdCostoCampana;
            costoCampana = O.CostoCampana;
            fechaAlta = O.FechaAlta;
            monto = O.Monto;
            idCampana = O.IdCampana;
            idUsuario = O.IdUsuario;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_CostoCampana_ConsultarFiltros";
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
        Obten.Llena<CCostoCampana>(typeof(CCostoCampana), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_CostoCampana_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCostoCampana", 0);
        Agregar.StoredProcedure.Parameters["@pIdCostoCampana"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCostoCampana", costoCampana);
        if (fechaAlta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCampana", idCampana);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idCostoCampana = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCostoCampana"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_CostoCampana_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCostoCampana", idCostoCampana);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCostoCampana", costoCampana);
        if (fechaAlta.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCampana", idCampana);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_CostoCampana_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCostoCampana", idCostoCampana);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}