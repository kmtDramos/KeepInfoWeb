/**/

$(function () {

	MantenerSesion();

	setInterval(MantenerSesion, 1000 * 60 * 1.5);

	$("#divReportes").tabs();

	$("#tblDetalle").DataTable({ "oLanguage": { "sUrl": "../JS/Spanish.json" } });

	$("#tblSucursales").DataTable({ "oLanguage": { "sUrl": "../JS/Spanish.json" } });

	$("#tblFamilias").DataTable({ "oLanguage": { "sUrl": "../JS/Spanish.json" } });

	$("#tblVendedores").DataTable({ "oLanguage": { "sUrl": "../JS/Spanish.json" } });

	$("#txtFechaInicial").datepicker();

	$("#txtFechaFinal").datepicker();

	LlenaSucursales();

});


function LlenaSucursales ()
{
	$.ajax({
		url: "ReporteadorFacturacion.aspx/ObtenerSucursales",
		type: "post",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				var Modelo = json.Modelo;
				var Sucursales = Modelo.Sucursales;
				$("#cmbSucursal").html('').append($('<option value="-1">-Todas-</option>'));
				for (x in Sucursales)
				{
					$("#cmbSucursal").append($('<option value="' + Sucursales[x].Valor + '">' + Sucursales[x].Descripcion + '</option>'));
				}
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}