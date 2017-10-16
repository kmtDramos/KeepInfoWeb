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

public partial class CDepositosEncabezadoFactura
{
	//Propiedades Privadas
	private int idDepositosEncabezadoFactura;
	private DateTime fechaPago;
	private decimal monto;
	private string nota;
	private int idEncabezadoFactura;
	private int idDepositos;
	private int idUsuario;
	private bool baja;
	
	//Propiedades
	public int IdDepositosEncabezadoFactura
	{
		get { return idDepositosEncabezadoFactura; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idDepositosEncabezadoFactura = value;
		}
	}
	
	public DateTime FechaPago
	{
		get { return fechaPago; }
		set { fechaPago = value; }
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
	
	public string Nota
	{
		get { return nota; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			nota = value;
		}
	}
	
	public int IdEncabezadoFactura
	{
		get { return idEncabezadoFactura; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEncabezadoFactura = value;
		}
	}
	
	public int IdDepositos
	{
		get { return idDepositos; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idDepositos = value;
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
	public CDepositosEncabezadoFactura()
	{
		idDepositosEncabezadoFactura = 0;
		fechaPago = new DateTime(1, 1, 1);
		monto = 0;
		nota = "";
		idEncabezadoFactura = 0;
		idDepositos = 0;
		idUsuario = 0;
		baja = false;
	}
	
	public CDepositosEncabezadoFactura(int pIdDepositosEncabezadoFactura)
	{
		idDepositosEncabezadoFactura = pIdDepositosEncabezadoFactura;
		fechaPago = new DateTime(1, 1, 1);
		monto = 0;
		nota = "";
		idEncabezadoFactura = 0;
		idDepositos = 0;
		idUsuario = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DepositosEncabezadoFactura_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDepositosEncabezadoFactura>(typeof(CDepositosEncabezadoFactura), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DepositosEncabezadoFactura_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CDepositosEncabezadoFactura>(typeof(CDepositosEncabezadoFactura), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DepositosEncabezadoFactura_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdDepositosEncabezadoFactura", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDepositosEncabezadoFactura>(typeof(CDepositosEncabezadoFactura), pConexion);
		foreach (CDepositosEncabezadoFactura O in Obten.ListaRegistros)
		{
			idDepositosEncabezadoFactura = O.IdDepositosEncabezadoFactura;
			fechaPago = O.FechaPago;
			monto = O.Monto;
			nota = O.Nota;
			idEncabezadoFactura = O.IdEncabezadoFactura;
			idDepositos = O.IdDepositos;
			idUsuario = O.IdUsuario;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DepositosEncabezadoFactura_ConsultarFiltros";
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
		Obten.Llena<CDepositosEncabezadoFactura>(typeof(CDepositosEncabezadoFactura), pConexion);
		foreach (CDepositosEncabezadoFactura O in Obten.ListaRegistros)
		{
			idDepositosEncabezadoFactura = O.IdDepositosEncabezadoFactura;
			fechaPago = O.FechaPago;
			monto = O.Monto;
			nota = O.Nota;
			idEncabezadoFactura = O.IdEncabezadoFactura;
			idDepositos = O.IdDepositos;
			idUsuario = O.IdUsuario;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DepositosEncabezadoFactura_ConsultarFiltros";
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
		Obten.Llena<CDepositosEncabezadoFactura>(typeof(CDepositosEncabezadoFactura), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_DepositosEncabezadoFactura_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDepositosEncabezadoFactura", 0);
		Agregar.StoredProcedure.Parameters["@pIdDepositosEncabezadoFactura"].Direction = ParameterDirection.Output;
		if(fechaPago.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFactura", idEncabezadoFactura);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDepositos", idDepositos);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idDepositosEncabezadoFactura= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDepositosEncabezadoFactura"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_DepositosEncabezadoFactura_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDepositosEncabezadoFactura", idDepositosEncabezadoFactura);
		if(fechaPago.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFactura", idEncabezadoFactura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDepositos", idDepositos);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_DepositosEncabezadoFactura_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDepositosEncabezadoFactura", idDepositosEncabezadoFactura);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}