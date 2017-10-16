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

public partial class CProducto
{
	//Propiedades Privadas
	private int idProducto;
	private string producto;
	private string clave;
	private string numeroParte;
	private string descripcion;
	private decimal costo;
	private decimal margenUtilidad;
	private decimal precio;
	private string modelo;
	private int idUnidadCompraVenta;
	private int idTipoVenta;
	private int idTipoMoneda;
	private int idMarca;
	private int idCategoria;
	private int idUsuarioAlta;
	private DateTime fechaAlta;
	private int idUsuarioModifico;
	private DateTime fechaModificacion;
	private string codigoBarra;
	private string imagen;
	private decimal valorMedida;
	private int idTipoIVA;
	private int idSubCategoria;
	private int idGrupo;
	private bool baja;
	
	//Propiedades
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
	
	public string Producto
	{
		get { return producto; }
		set
		{
			producto = value;
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
	
	public string NumeroParte
	{
		get { return numeroParte; }
		set
		{
			numeroParte = value;
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
	
	public decimal Costo
	{
		get { return costo; }
		set
		{
			if (value < 0)
			{
				return;
			}
			costo = value;
		}
	}
	
	public decimal MargenUtilidad
	{
		get { return margenUtilidad; }
		set
		{
			if (value < 0)
			{
				return;
			}
			margenUtilidad = value;
		}
	}
	
	public decimal Precio
	{
		get { return precio; }
		set
		{
			if (value < 0)
			{
				return;
			}
			precio = value;
		}
	}
	
	public string Modelo
	{
		get { return modelo; }
		set
		{
			modelo = value;
		}
	}
	
	public int IdUnidadCompraVenta
	{
		get { return idUnidadCompraVenta; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUnidadCompraVenta = value;
		}
	}
	
	public int IdTipoVenta
	{
		get { return idTipoVenta; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoVenta = value;
		}
	}
	
	public int IdTipoMoneda
	{
		get { return idTipoMoneda; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoMoneda = value;
		}
	}
	
	public int IdMarca
	{
		get { return idMarca; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idMarca = value;
		}
	}
	
	public int IdCategoria
	{
		get { return idCategoria; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idCategoria = value;
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
	
	public int IdUsuarioModifico
	{
		get { return idUsuarioModifico; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUsuarioModifico = value;
		}
	}
	
	public DateTime FechaModificacion
	{
		get { return fechaModificacion; }
		set { fechaModificacion = value; }
	}
	
	public string CodigoBarra
	{
		get { return codigoBarra; }
		set
		{
			codigoBarra = value;
		}
	}
	
	public string Imagen
	{
		get { return imagen; }
		set
		{
			imagen = value;
		}
	}
	
	public decimal ValorMedida
	{
		get { return valorMedida; }
		set
		{
			if (value < 0)
			{
				return;
			}
			valorMedida = value;
		}
	}
	
	public int IdTipoIVA
	{
		get { return idTipoIVA; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoIVA = value;
		}
	}
	
	public int IdSubCategoria
	{
		get { return idSubCategoria; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSubCategoria = value;
		}
	}
	
	public int IdGrupo
	{
		get { return idGrupo; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idGrupo = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CProducto()
	{
		idProducto = 0;
		producto = "";
		clave = "";
		numeroParte = "";
		descripcion = "";
		costo = 0;
		margenUtilidad = 0;
		precio = 0;
		modelo = "";
		idUnidadCompraVenta = 0;
		idTipoVenta = 0;
		idTipoMoneda = 0;
		idMarca = 0;
		idCategoria = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		idUsuarioModifico = 0;
		fechaModificacion = new DateTime(1, 1, 1);
		codigoBarra = "";
		imagen = "";
		valorMedida = 0;
		idTipoIVA = 0;
		idSubCategoria = 0;
		idGrupo = 0;
		baja = false;
	}
	
	public CProducto(int pIdProducto)
	{
		idProducto = pIdProducto;
		producto = "";
		clave = "";
		numeroParte = "";
		descripcion = "";
		costo = 0;
		margenUtilidad = 0;
		precio = 0;
		modelo = "";
		idUnidadCompraVenta = 0;
		idTipoVenta = 0;
		idTipoMoneda = 0;
		idMarca = 0;
		idCategoria = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		idUsuarioModifico = 0;
		fechaModificacion = new DateTime(1, 1, 1);
		codigoBarra = "";
		imagen = "";
		valorMedida = 0;
		idTipoIVA = 0;
		idSubCategoria = 0;
		idGrupo = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Producto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CProducto>(typeof(CProducto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Producto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CProducto>(typeof(CProducto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Producto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdProducto", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CProducto>(typeof(CProducto), pConexion);
		foreach (CProducto O in Obten.ListaRegistros)
		{
			idProducto = O.IdProducto;
			producto = O.Producto;
			clave = O.Clave;
			numeroParte = O.NumeroParte;
			descripcion = O.Descripcion;
			costo = O.Costo;
			margenUtilidad = O.MargenUtilidad;
			precio = O.Precio;
			modelo = O.Modelo;
			idUnidadCompraVenta = O.IdUnidadCompraVenta;
			idTipoVenta = O.IdTipoVenta;
			idTipoMoneda = O.IdTipoMoneda;
			idMarca = O.IdMarca;
			idCategoria = O.IdCategoria;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
			fechaModificacion = O.FechaModificacion;
			codigoBarra = O.CodigoBarra;
			imagen = O.Imagen;
			valorMedida = O.ValorMedida;
			idTipoIVA = O.IdTipoIVA;
			idSubCategoria = O.IdSubCategoria;
			idGrupo = O.IdGrupo;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Producto_ConsultarFiltros";
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
		Obten.Llena<CProducto>(typeof(CProducto), pConexion);
		foreach (CProducto O in Obten.ListaRegistros)
		{
			idProducto = O.IdProducto;
			producto = O.Producto;
			clave = O.Clave;
			numeroParte = O.NumeroParte;
			descripcion = O.Descripcion;
			costo = O.Costo;
			margenUtilidad = O.MargenUtilidad;
			precio = O.Precio;
			modelo = O.Modelo;
			idUnidadCompraVenta = O.IdUnidadCompraVenta;
			idTipoVenta = O.IdTipoVenta;
			idTipoMoneda = O.IdTipoMoneda;
			idMarca = O.IdMarca;
			idCategoria = O.IdCategoria;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
			fechaModificacion = O.FechaModificacion;
			codigoBarra = O.CodigoBarra;
			imagen = O.Imagen;
			valorMedida = O.ValorMedida;
			idTipoIVA = O.IdTipoIVA;
			idSubCategoria = O.IdSubCategoria;
			idGrupo = O.IdGrupo;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Producto_ConsultarFiltros";
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
		Obten.Llena<CProducto>(typeof(CProducto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Producto_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", 0);
		Agregar.StoredProcedure.Parameters["@pIdProducto"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProducto", producto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroParte", numeroParte);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMargenUtilidad", margenUtilidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPrecio", precio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pModelo", modelo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoVenta", idTipoVenta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMarca", idMarca);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCategoria", idCategoria);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		if(fechaModificacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCodigoBarra", codigoBarra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pImagen", imagen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pValorMedida", valorMedida);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSubCategoria", idSubCategoria);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdGrupo", idGrupo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idProducto= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdProducto"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Producto_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pProducto", producto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroParte", numeroParte);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMargenUtilidad", margenUtilidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPrecio", precio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pModelo", modelo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoVenta", idTipoVenta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMarca", idMarca);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCategoria", idCategoria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		if(fechaModificacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pCodigoBarra", codigoBarra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pImagen", imagen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pValorMedida", valorMedida);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSubCategoria", idSubCategoria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdGrupo", idGrupo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Producto_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}