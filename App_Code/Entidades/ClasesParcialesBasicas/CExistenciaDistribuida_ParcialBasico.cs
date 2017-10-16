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

public partial class CExistenciaDistribuida
{
	//Propiedades Privadas
	private int idExistenciaDistribuida;
	private DateTime fecha;
	private int saldo;
	private int cantidad;
	private int idDetalleFacturaProveedor;
	private int idAlmacen;
	private int idUsuario;
	
	//Propiedades
	public int IdExistenciaDistribuida
	{
		get { return idExistenciaDistribuida; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idExistenciaDistribuida = value;
		}
	}
	
	public DateTime Fecha
	{
		get { return fecha; }
		set { fecha = value; }
	}
	
	public int Saldo
	{
		get { return saldo; }
		set
		{
			if (value < 0)
			{
				return;
			}
			saldo = value;
		}
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
	
	//Constructores
	public CExistenciaDistribuida()
	{
		idExistenciaDistribuida = 0;
		fecha = new DateTime(1, 1, 1);
		saldo = 0;
		cantidad = 0;
		idDetalleFacturaProveedor = 0;
		idAlmacen = 0;
		idUsuario = 0;
	}
	
	public CExistenciaDistribuida(int pIdExistenciaDistribuida)
	{
		idExistenciaDistribuida = pIdExistenciaDistribuida;
		fecha = new DateTime(1, 1, 1);
		saldo = 0;
		cantidad = 0;
		idDetalleFacturaProveedor = 0;
		idAlmacen = 0;
		idUsuario = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciaDistribuida_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CExistenciaDistribuida>(typeof(CExistenciaDistribuida), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciaDistribuida_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CExistenciaDistribuida>(typeof(CExistenciaDistribuida), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciaDistribuida_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaDistribuida", pIdentificador);
		Obten.Llena<CExistenciaDistribuida>(typeof(CExistenciaDistribuida), pConexion);
		foreach (CExistenciaDistribuida O in Obten.ListaRegistros)
		{
			idExistenciaDistribuida = O.IdExistenciaDistribuida;
			fecha = O.Fecha;
			saldo = O.Saldo;
			cantidad = O.Cantidad;
			idDetalleFacturaProveedor = O.IdDetalleFacturaProveedor;
			idAlmacen = O.IdAlmacen;
			idUsuario = O.IdUsuario;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciaDistribuida_ConsultarFiltros";
		foreach (KeyValuePair<string, object> parametro in pParametros)
		{
			if (parametro.Key == "Opcion")
			{
				Obten.StoredProcedure.Parameters.AddWithValue("@"+parametro.Key, parametro.Value);
			}
			else
			{
				Obten.StoredProcedure.Parameters.AddWithValue("@p"+parametro.Key, parametro.Value);
			}
		}
		Obten.Llena<CExistenciaDistribuida>(typeof(CExistenciaDistribuida), pConexion);
		foreach (CExistenciaDistribuida O in Obten.ListaRegistros)
		{
			idExistenciaDistribuida = O.IdExistenciaDistribuida;
			fecha = O.Fecha;
			saldo = O.Saldo;
			cantidad = O.Cantidad;
			idDetalleFacturaProveedor = O.IdDetalleFacturaProveedor;
			idAlmacen = O.IdAlmacen;
			idUsuario = O.IdUsuario;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciaDistribuida_ConsultarFiltros";
		foreach (KeyValuePair<string, object> parametro in pParametros)
		{
			if (parametro.Key == "Opcion")
			{
				Obten.StoredProcedure.Parameters.AddWithValue("@"+parametro.Key, parametro.Value);
			}
			else
			{
				Obten.StoredProcedure.Parameters.AddWithValue("@p"+parametro.Key, parametro.Value);
			}
		}
		Obten.Llena<CExistenciaDistribuida>(typeof(CExistenciaDistribuida), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ExistenciaDistribuida_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaDistribuida", 0);
		Agregar.StoredProcedure.Parameters["@pIdExistenciaDistribuida"].Direction = ParameterDirection.Output;
		if(fecha.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldo", saldo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", idDetalleFacturaProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.Insert(pConexion);
		idExistenciaDistribuida= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdExistenciaDistribuida"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ExistenciaDistribuida_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaDistribuida", idExistenciaDistribuida);
		if(fecha.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pSaldo", saldo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", idDetalleFacturaProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.Update(pConexion);
	}

    public void GenerarTraspaso(CConexion pConexion)
    {
        CConsultaAccion Traspaso = new CConsultaAccion();
        Traspaso.StoredProcedure.CommandText = "sp_Traspaso_Agregar_GenerarTraspaso";
        Traspaso.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", IdUsuario);
        Traspaso.StoredProcedure.Parameters.AddWithValue("@pIdAlmacenDestino", IdAlmacen);
        Traspaso.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
        Traspaso.StoredProcedure.Parameters.AddWithValue("@pDisponibles", saldo);
        Traspaso.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaDistribuida", IdExistenciaDistribuida);
        
        Traspaso.Insert(pConexion);
        
    }
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ExistenciaDistribuida_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaDistribuida", idExistenciaDistribuida);
		Eliminar.Delete(pConexion);
	}
}