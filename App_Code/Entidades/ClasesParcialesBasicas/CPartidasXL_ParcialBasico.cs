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

public partial class CPartidasXL
{
	//Propiedades Privadas
    private int idCotizacion;
    private string clave;
	private string descripcion;
	private string proveedor;
	private decimal costo;
	private decimal manoobra;
	private decimal margen;
	private decimal descuento;
	private string division;
	
	//Propiedades
    public int IdCotizacion
    {
        get { return idCotizacion; }
        set
        {
            idCotizacion = value;
        }
    }

    public string Clave
	{
		get { return clave; }
		set
		{
			clave = value;
		}
	}
	
	public string Descripcion
	{
		get { return descripcion; }
		set
		{
			descripcion = value;
		}
	}
	
	public string Proveedor
	{
		get { return proveedor; }
		set
		{
			proveedor = value;
		}
	}
	
	public decimal Costo
	{
		get { return costo; }
		set
		{
			costo = value;
		}
	}
	
    public decimal ManoObra
	{
		get { return manoobra; }
		set
		{
			manoobra = value;
		}
	}

	public decimal Margen
	{
		get { return margen; }
		set
		{
			margen = value;
		}
	}
    
    public decimal Descuento
	{
		get { return descuento; }
		set
		{
			descuento = value;
		}
	}
	
    public string Division
	{
		get { return division; }
		set
		{
			division = value;
		}
	}
	
	//Constructores
	public CPartidasXL()
	{
        IdCotizacion = 0;
        clave = "";
		descripcion = "";
		proveedor = "";
		costo = 0;
        manoobra = 0;
		margen = 0;
        descuento = 0;
		division = "";
		
	}
	
	public CPartidasXL(int pIdPartidasXL)
	{
        idCotizacion = 0;
        clave = "";
        descripcion = "";
        proveedor = "";
        costo = 0;
        manoobra = 0;
        margen = 0;
        descuento = 0;
        division = "";
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion, int pIdCotizacion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PartidasXL_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@IdCotizacion", pIdCotizacion);
		Obten.Llena<CPartidasXL>(typeof(CPartidasXL), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PartidasXL_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CPartidasXL>(typeof(CPartidasXL), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PartidasXL_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdPartidasXL", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CPartidasXL>(typeof(CPartidasXL), pConexion);
		foreach (CPartidasXL O in Obten.ListaRegistros)
		{
			
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PartidasXL_ConsultarFiltros";
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
		Obten.Llena<CPartidasXL>(typeof(CPartidasXL), pConexion);
		foreach (CPartidasXL O in Obten.ListaRegistros)
		{
			
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PartidasXL_ConsultarFiltros";
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
		Obten.Llena<CPartidasXL>(typeof(CPartidasXL), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{

		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_PartidasXL_Agregar";

        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProveedor", proveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pManoObra", manoobra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMargen", margen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDivision", division);
		Agregar.Insert(pConexion);
		//clave= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdPartidasXL"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
        //CConsultaAccion Editar = new CConsultaAccion();
        //Editar.StoredProcedure.CommandText = "spb_PartidasXL_Editar";
        //Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pIdPartidasXL", clave);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pNota", descripcion);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pMontoLetra", proveedor);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pSubtotal", costo);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pUtilidad", margen);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pFacturado", facturado);
        //if(fechaCreacion.Year != 1)
        //{
        //    Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
        //}
        //if(fechaUltimaModificacion.Year != 1)
        //{
        //    Editar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
        //}
        //if(fechaExpiracion.Year != 1)
        //{
        //    Editar.StoredProcedure.Parameters.AddWithValue("@pFechaExpiracion", fechaExpiracion);
        //}
        //if(fechaCancelacion.Year != 1)
        //{
        //    Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
        //}
        //if(fechaPedido.Year != 1)
        //{
        //    Editar.StoredProcedure.Parameters.AddWithValue("@pFechaPedido", fechaPedido);
        //}
        //Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCancelacion", idUsuarioCancelacion);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion", manoobra);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioPedido", idUsuarioPedido);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusPartidasXL", idEstatusPartidasXL);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pManoObra", manoObra);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pIdDireccionOrganizacion", idDireccionOrganizacion);
        //Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        //Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_PartidasXL_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdPartidasXL", clave);
	
		Eliminar.Delete(pConexion);
	}
}
