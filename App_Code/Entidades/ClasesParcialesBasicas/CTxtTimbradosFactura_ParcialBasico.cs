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

public partial class CTxtTimbradosFactura
{
	//Propiedades Privadas
	private int idTxtTimbradosFactura;
	private string noCertificadoSAT;
	private string fechaTimbrado;
	private string uuid;
	private string noCertificado;
	private string selloSAT;
	private string sello;
	private string fecha;
	private string folio;
	private string serie;
	private string totalConLetra;
	private string leyendaImpresion;
	private string cadenaOriginal;
	private string mensajeError;
	private string fechaCancelacion;
	private string refid;
	
	//Propiedades
	public int IdTxtTimbradosFactura
	{
		get { return idTxtTimbradosFactura; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTxtTimbradosFactura = value;
		}
	}
	
	public string NoCertificadoSAT
	{
		get { return noCertificadoSAT; }
		set
		{
			noCertificadoSAT = value;
		}
	}
	
	public string FechaTimbrado
	{
		get { return fechaTimbrado; }
		set
		{
			fechaTimbrado = value;
		}
	}
	
	public string Uuid
	{
		get { return uuid; }
		set
		{
			uuid = value;
		}
	}
	
	public string NoCertificado
	{
		get { return noCertificado; }
		set
		{
			noCertificado = value;
		}
	}
	
	public string SelloSAT
	{
		get { return selloSAT; }
		set
		{
			selloSAT = value;
		}
	}
	
	public string Sello
	{
		get { return sello; }
		set
		{
			sello = value;
		}
	}
	
	public string Fecha
	{
		get { return fecha; }
		set
		{
			fecha = value;
		}
	}
	
	public string Folio
	{
		get { return folio; }
		set
		{
			folio = value;
		}
	}
	
	public string Serie
	{
		get { return serie; }
		set
		{
			serie = value;
		}
	}
	
	public string TotalConLetra
	{
		get { return totalConLetra; }
		set
		{
			totalConLetra = value;
		}
	}
	
	public string LeyendaImpresion
	{
		get { return leyendaImpresion; }
		set
		{
			leyendaImpresion = value;
		}
	}
	
	public string CadenaOriginal
	{
		get { return cadenaOriginal; }
		set
		{
			cadenaOriginal = value;
		}
	}
	
	public string MensajeError
	{
		get { return mensajeError; }
		set
		{
			mensajeError = value;
		}
	}
	
	public string FechaCancelacion
	{
		get { return fechaCancelacion; }
		set
		{
			fechaCancelacion = value;
		}
	}
	
	public string Refid
	{
		get { return refid; }
		set
		{
			refid = value;
		}
	}
	
	//Constructores
	public CTxtTimbradosFactura()
	{
		idTxtTimbradosFactura = 0;
		noCertificadoSAT = "";
		fechaTimbrado = "";
		uuid = "";
		noCertificado = "";
		selloSAT = "";
		sello = "";
		fecha = "";
		folio = "";
		serie = "";
		totalConLetra = "";
		leyendaImpresion = "";
		cadenaOriginal = "";
		mensajeError = "";
		fechaCancelacion = "";
		refid = "";
	}
	
	public CTxtTimbradosFactura(int pIdTxtTimbradosFactura)
	{
		idTxtTimbradosFactura = pIdTxtTimbradosFactura;
		noCertificadoSAT = "";
		fechaTimbrado = "";
		uuid = "";
		noCertificado = "";
		selloSAT = "";
		sello = "";
		fecha = "";
		folio = "";
		serie = "";
		totalConLetra = "";
		leyendaImpresion = "";
		cadenaOriginal = "";
		mensajeError = "";
		fechaCancelacion = "";
		refid = "";
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TxtTimbradosFactura_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CTxtTimbradosFactura>(typeof(CTxtTimbradosFactura), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TxtTimbradosFactura_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CTxtTimbradosFactura>(typeof(CTxtTimbradosFactura), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TxtTimbradosFactura_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdTxtTimbradosFactura", pIdentificador);
		Obten.Llena<CTxtTimbradosFactura>(typeof(CTxtTimbradosFactura), pConexion);
		foreach (CTxtTimbradosFactura O in Obten.ListaRegistros)
		{
			idTxtTimbradosFactura = O.IdTxtTimbradosFactura;
			noCertificadoSAT = O.NoCertificadoSAT;
			fechaTimbrado = O.FechaTimbrado;
			uuid = O.Uuid;
			noCertificado = O.NoCertificado;
			selloSAT = O.SelloSAT;
			sello = O.Sello;
			fecha = O.Fecha;
			folio = O.Folio;
			serie = O.Serie;
			totalConLetra = O.TotalConLetra;
			leyendaImpresion = O.LeyendaImpresion;
			cadenaOriginal = O.CadenaOriginal;
			mensajeError = O.MensajeError;
			fechaCancelacion = O.FechaCancelacion;
			refid = O.Refid;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TxtTimbradosFactura_ConsultarFiltros";
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
		Obten.Llena<CTxtTimbradosFactura>(typeof(CTxtTimbradosFactura), pConexion);
		foreach (CTxtTimbradosFactura O in Obten.ListaRegistros)
		{
			idTxtTimbradosFactura = O.IdTxtTimbradosFactura;
			noCertificadoSAT = O.NoCertificadoSAT;
			fechaTimbrado = O.FechaTimbrado;
			uuid = O.Uuid;
			noCertificado = O.NoCertificado;
			selloSAT = O.SelloSAT;
			sello = O.Sello;
			fecha = O.Fecha;
			folio = O.Folio;
			serie = O.Serie;
			totalConLetra = O.TotalConLetra;
			leyendaImpresion = O.LeyendaImpresion;
			cadenaOriginal = O.CadenaOriginal;
			mensajeError = O.MensajeError;
			fechaCancelacion = O.FechaCancelacion;
			refid = O.Refid;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TxtTimbradosFactura_ConsultarFiltros";
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
		Obten.Llena<CTxtTimbradosFactura>(typeof(CTxtTimbradosFactura), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_TxtTimbradosFactura_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTxtTimbradosFactura", 0);
		Agregar.StoredProcedure.Parameters["@pIdTxtTimbradosFactura"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNoCertificadoSAT", noCertificadoSAT);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaTimbrado", fechaTimbrado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pUuid", uuid);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNoCertificado", noCertificado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSelloSAT", selloSAT);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSello", sello);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSerie", serie);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotalConLetra", totalConLetra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pLeyendaImpresion", leyendaImpresion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCadenaOriginal", cadenaOriginal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMensajeError", mensajeError);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRefid", refid);
		Agregar.Insert(pConexion);
		idTxtTimbradosFactura= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdTxtTimbradosFactura"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_TxtTimbradosFactura_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTxtTimbradosFactura", idTxtTimbradosFactura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNoCertificadoSAT", noCertificadoSAT);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFechaTimbrado", fechaTimbrado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pUuid", uuid);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNoCertificado", noCertificado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSelloSAT", selloSAT);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSello", sello);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSerie", serie);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotalConLetra", totalConLetra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pLeyendaImpresion", leyendaImpresion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCadenaOriginal", cadenaOriginal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMensajeError", mensajeError);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pRefid", refid);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_TxtTimbradosFactura_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdTxtTimbradosFactura", idTxtTimbradosFactura);
		Eliminar.Delete(pConexion);
	}
}