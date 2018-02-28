using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_ReporteCompras : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{
		GenerarGridVentas(ClientScript, this);
	}

	private static void GenerarGridVentas(ClientScriptManager ClientScript, Page pagina)
	{

		CJQGrid GridVentas = new CJQGrid();
		GridVentas.NombreTabla = "grdVentas";
		GridVentas.CampoIdentificador = "IdOrdenCompraEncabezado";
		GridVentas.ColumnaOrdenacion = "IdOrdenCompraEncabezado";
		GridVentas.TipoOrdenacion = "DESC";
		GridVentas.Metodo = "ObtenerVentas";
		GridVentas.TituloTabla = "Gestión de compras";
		GridVentas.GenerarGridCargaInicial = true;
		GridVentas.GenerarFuncionFiltro = true;
		GridVentas.GenerarFuncionTerminado = true;
		GridVentas.NumeroFila = false;
		GridVentas.Altura = 230;
		GridVentas.Ancho = 1400;
		GridVentas.NumeroRegistros = 10;
		GridVentas.RangoNumeroRegistros = "10,25,40";

		CJQColumn ColIdOrdenCompraEncabezado = new CJQColumn();
		ColIdOrdenCompraEncabezado.Nombre = "IdOrdenCompraEncabezado";
		ColIdOrdenCompraEncabezado.Encabezado = "#";
		ColIdOrdenCompraEncabezado.Buscador = "false";
		ColIdOrdenCompraEncabezado.Oculto = "true";
		GridVentas.Columnas.Add(ColIdOrdenCompraEncabezado);

		CJQColumn ColCliente = new CJQColumn();
		ColCliente.Nombre = "Cliente";
		ColCliente.Encabezado = "Cliente";
		GridVentas.Columnas.Add(ColCliente);

		CJQColumn ColCotizacion = new CJQColumn();
		ColCotizacion.Nombre = "Cotizacion";
		ColCotizacion.Encabezado = "Cotización";
		GridVentas.Columnas.Add(ColCotizacion);

		CJQColumn ColVoBo = new CJQColumn();
		ColVoBo.Nombre = "VoBo";
		ColVoBo.Encabezado = "VoBo Cliente";
		GridVentas.Columnas.Add(ColVoBo);

		CJQColumn ColEntregaCliente = new CJQColumn();
		ColEntregaCliente.Nombre = "EntregaCliente";
		ColEntregaCliente.Encabezado = "Entrega a Cliente";
		GridVentas.Columnas.Add(ColEntregaCliente);

		CJQColumn ColProveedor = new CJQColumn();
		ColProveedor.Nombre = "Proveedor";
		ColProveedor.Encabezado = "Proveedor";
		GridVentas.Columnas.Add(ColProveedor);

		CJQColumn ColPrecio = new CJQColumn();
		ColPrecio.Nombre = "Costo";
		ColPrecio.Encabezado = "Costo";
		GridVentas.Columnas.Add(ColPrecio);

		CJQColumn ColEntrega = new CJQColumn();
		ColEntrega.Nombre = "Entrega";
		ColEntrega.Encabezado = "Fecha entrega";
		GridVentas.Columnas.Add(ColEntrega);

		CJQColumn ColMetaPago = new CJQColumn();
		ColMetaPago.Nombre = "MetaPago";
		ColMetaPago.Encabezado = "Meta";
		GridVentas.Columnas.Add(ColMetaPago);

		CJQColumn ColRealPago = new CJQColumn();
		ColRealPago.Nombre = "RealPago";
		ColRealPago.Encabezado = "Real";
		GridVentas.Columnas.Add(ColRealPago);

		ClientScript.RegisterStartupScript(pagina.GetType(), "grdVentas", GridVentas.GeneraGrid(), true);

	}

}