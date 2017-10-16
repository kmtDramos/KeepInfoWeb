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

public partial class CExistenciaVendida
{
    //Propiedades Privadas
    private int idExistenciaVendida;
    private DateTime fecha;
    private int cantidad;
    private int idFacturaDetalleCliente;
    private int idDetalleFacturaProveedor;

    //Propiedades
    public int IdExistenciaVendida
    {
        get { return idExistenciaVendida; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idExistenciaVendida = value;
        }
    }

    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public int Cantidad
    {
        get { return cantidad; }
        set
        {
            if (value < 0)
            {
                return;
            }
            cantidad = value;
        }
    }

    public int IdFacturaDetalleCliente
    {
        get { return idFacturaDetalleCliente; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idFacturaDetalleCliente = value;
        }
    }

    public int IdDetalleFacturaProveedor
    {
        get { return idDetalleFacturaProveedor; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idDetalleFacturaProveedor = value;
        }
    }

    //Constructores
    public CExistenciaVendida()
    {
        idExistenciaVendida = 0;
        fecha = new DateTime(1, 1, 1);
        cantidad = 0;
        idFacturaDetalleCliente = 0;
        idDetalleFacturaProveedor = 0;
    }

    public CExistenciaVendida(int pIdExistenciaVendida)
    {
        idExistenciaVendida = pIdExistenciaVendida;
        fecha = new DateTime(1, 1, 1);
        cantidad = 0;
        idFacturaDetalleCliente = 0;
        idDetalleFacturaProveedor = 0;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ExistenciaVendida_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Llena<CExistenciaVendida>(typeof(CExistenciaVendida), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ExistenciaVendida_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CExistenciaVendida>(typeof(CExistenciaVendida), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ExistenciaVendida_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaVendida", pIdentificador);
        Obten.Llena<CExistenciaVendida>(typeof(CExistenciaVendida), pConexion);
        foreach (CExistenciaVendida O in Obten.ListaRegistros)
        {
            idExistenciaVendida = O.IdExistenciaVendida;
            fecha = O.Fecha;
            cantidad = O.Cantidad;
            idFacturaDetalleCliente = O.IdFacturaDetalleCliente;
            idDetalleFacturaProveedor = O.IdDetalleFacturaProveedor;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ExistenciaVendida_ConsultarFiltros";
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
        Obten.Llena<CExistenciaVendida>(typeof(CExistenciaVendida), pConexion);
        foreach (CExistenciaVendida O in Obten.ListaRegistros)
        {
            idExistenciaVendida = O.IdExistenciaVendida;
            fecha = O.Fecha;
            cantidad = O.Cantidad;
            idFacturaDetalleCliente = O.IdFacturaDetalleCliente;
            idDetalleFacturaProveedor = O.IdDetalleFacturaProveedor;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ExistenciaVendida_ConsultarFiltros";
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
        Obten.Llena<CExistenciaVendida>(typeof(CExistenciaVendida), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_ExistenciaVendida_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaVendida", 0);
        Agregar.StoredProcedure.Parameters["@pIdExistenciaVendida"].Direction = ParameterDirection.Output;
        if (fecha.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaDetalleCliente", idFacturaDetalleCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", idDetalleFacturaProveedor);
        Agregar.Insert(pConexion);
        idExistenciaVendida = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdExistenciaVendida"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_ExistenciaVendida_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaVendida", idExistenciaVendida);
        if (fecha.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaDetalleCliente", idFacturaDetalleCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", idDetalleFacturaProveedor);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_ExistenciaVendida_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaVendida", idExistenciaVendida);
        Eliminar.Delete(pConexion);
    }
}