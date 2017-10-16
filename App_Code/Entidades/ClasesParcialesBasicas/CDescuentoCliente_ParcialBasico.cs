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

public partial class CDescuentoCliente
{
    //Propiedades Privadas
    private int idDescuentoCliente;
    private string descripcion;
    private decimal descuentoCliente;
    private int idCliente;
    private int idUsuarioAlta;
    private DateTime fechaAlta;
    private bool baja;

    //Propiedades
    public int IdDescuentoCliente
    {
        get { return idDescuentoCliente; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idDescuentoCliente = value;
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

    public decimal DescuentoCliente
    {
        get { return descuentoCliente; }
        set
        {
            if (value < 0)
            {
                return;
            }
            descuentoCliente = value;
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

    public int IdUsuarioAlta
    {
        get { return idUsuarioAlta; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUsuarioAlta = value;
        }
    }

    public DateTime FechaAlta
    {
        get { return fechaAlta; }
        set { fechaAlta = value; }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CDescuentoCliente()
    {
        idDescuentoCliente = 0;
        descripcion = "";
        descuentoCliente = 0;
        idCliente = 0;
        idUsuarioAlta = 0;
        fechaAlta = new DateTime(1, 1, 1);
        baja = false;
    }

    public CDescuentoCliente(int pIdDescuentoCliente)
    {
        idDescuentoCliente = pIdDescuentoCliente;
        descripcion = "";
        descuentoCliente = 0;
        idCliente = 0;
        idUsuarioAlta = 0;
        fechaAlta = new DateTime(1, 1, 1);
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DescuentoCliente_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CDescuentoCliente>(typeof(CDescuentoCliente), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DescuentoCliente_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CDescuentoCliente>(typeof(CDescuentoCliente), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DescuentoCliente_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoCliente", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CDescuentoCliente>(typeof(CDescuentoCliente), pConexion);
        foreach (CDescuentoCliente O in Obten.ListaRegistros)
        {
            idDescuentoCliente = O.IdDescuentoCliente;
            descripcion = O.Descripcion;
            descuentoCliente = O.DescuentoCliente;
            idCliente = O.IdCliente;
            idUsuarioAlta = O.IdUsuarioAlta;
            fechaAlta = O.FechaAlta;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DescuentoCliente_ConsultarFiltros";
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
        Obten.Llena<CDescuentoCliente>(typeof(CDescuentoCliente), pConexion);
        foreach (CDescuentoCliente O in Obten.ListaRegistros)
        {
            idDescuentoCliente = O.IdDescuentoCliente;
            descripcion = O.Descripcion;
            descuentoCliente = O.DescuentoCliente;
            idCliente = O.IdCliente;
            idUsuarioAlta = O.IdUsuarioAlta;
            fechaAlta = O.FechaAlta;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_DescuentoCliente_ConsultarFiltros";
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
        Obten.Llena<CDescuentoCliente>(typeof(CDescuentoCliente), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_DescuentoCliente_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoCliente", 0);
        Agregar.StoredProcedure.Parameters["@pIdDescuentoCliente"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuentoCliente", descuentoCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        if (fechaAlta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idDescuentoCliente = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDescuentoCliente"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_DescuentoCliente_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoCliente", idDescuentoCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDescuentoCliente", descuentoCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        if (fechaAlta.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_DescuentoCliente_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoCliente", idDescuentoCliente);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}