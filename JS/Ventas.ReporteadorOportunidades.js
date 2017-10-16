/**/
var totales;
var detalle;
$(function () {

	MantenerSesion();

	setInterval(MantenerSesion, 1000 * 60 * 1.5);

	detalle = $('#tblDetalleOportunidad').DataTable({ "oLanguage": { "sUrl": "../JS/Spanish.json" } });

	totales = $('#tblTotales').DataTable({ "oLanguage": { "sUrl": "../JS/Spanish.json" } });

	$("#btnActualizarReporteador").click(ObtenerTotalesReporteador);

	$("#txtFechaInicial").datepicker();

	$("#txtFechaFinal").datepicker();

});

function ObtenerTotalesReporteador ()
{
	var Oportunidades = new Object();
	Oportunidades.Cliente = $("#txtCliente").val();
	Oportunidades.Agente = $("#txtAgente").val();
	Oportunidades.IdSucursal = parseInt($("#cmbSucursal").val());
	Oportunidades.IdDivision = parseInt($("#cmbDivision").val());
	Oportunidades.FechaInicial = $("#txtFechaInicial").val();
	Oportunidades.FechaFinal = $("#txtFechaFinal").val();

	var Request = JSON.stringify(Oportunidades);

	$.ajax({
		url: "ReporteadorOportunidades.aspx/ObtenerTotalesReporteador",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				var Modelo = json.Modelo;
				var Totales = Modelo.Totales;
				var Detalle = Modelo.Detalle;
				detalle.clear();
				totales.clear();
				for (x in Totales) {
					totales.row.add(Totales[x]).draw(false);
				}
				for (x in Detalle) {
					detalle.row.add(Detalle[x]).draw(false);
				}
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function AutocompletarCliente()
{
	$("#txtCliente").autocomplete({
		source: function (request, response) {
			var Cliente = new Object();
			Cliente.RazonSocial = request.term
			$.ajax({
				url: "",
				dataType: "json",
				data: JSON.stringify(Cliente),
				success: function (Respuesta) {
					var json = JSON.parse(Respuesta.d);

				}
			});
		}
	});
}