/**/

$(function () {

	MantenerSesion();

	setInterval(MantenerSesion, 1000 * 60 * 1.5);

	ObtenerUsuarios();
	ObtenerSucursal();
	ObtenerDivision();
	ObtenerKPIs();

	/* No borrar es un ejemplo pra el futuro :)
	// request permission on page load
	document.addEventListener('DOMContentLoaded', function () {
		if (!Notification) {
			alert('Desktop notifications not available in your browser. Try Chromium.'); 
			return;
		}
	
		if (Notification.permission !== "granted")
			Notification.requestPermission();
	});
	
	function notifyMe() {
		if (Notification.permission !== "granted")
			Notification.requestPermission();
		else {
			var notification = new Notification('Notification title', {
				icon: 'http://cdn.sstatic.net/stackexchange/img/logos/so/so-icon.png',
				body: "Hey there! You've been notified!",
			});
	
			notification.onclick = function () {
			window.open("http://stackoverflow.com/a/13328397/1269037");      
			};
	
		}
	}
	*/
});

function ObtenerKPIs() {
	MostrarBloqueo();
	var KPIS = new Object();
	KPIS.IdSucursal = parseInt($("#cmbSucursal").val());
	KPIS.IdDivision = parseInt($("#cmbDivision").val());
	KPIS.IdUsuario = parseInt($("#cmbAgente").val());
	var Request = JSON.stringify(KPIS);
	CargarTabla(Request);
	$.ajax({
		url: "Indicadores.aspx/ObtenerIndicadores",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			OcultarBloqueo();
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				var TotalProspectos = json.Modelo.TotalProspectos;
				var Presentacion = json.Modelo.Presentacion;
				var Preventa = json.Modelo.Preventa;
				var SinAtender = TotalProspectos - Presentacion - Preventa;
				var Planeado = json.Modelo.Planeado;
				var Meta = json.Modelo.Meta;
				var Ventas = json.Modelo.Ventas;
				KpiPresentacion(TotalProspectos, Presentacion, SinAtender);
				KpiPreventa(TotalProspectos, Preventa, SinAtender);
				KpiMeta(Meta, Planeado);
				KpiOportunidad(Meta, Ventas);
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function KpiPresentacion(TotalProspectos, Presentacion, SinAtender) {
	s1 = [Presentacion];

	var p = TotalProspectos / 4;

	plot4 = $.jqplot('cnvPresentacion', [s1], {
		title: "Presentación",
		seriesDefaults: {
			renderer: $.jqplot.MeterGaugeRenderer,
			rendererOptions: {
				min: 0,
				max: TotalProspectos,
				label: Presentacion + ' Clientes de ' + TotalProspectos + ", sin atender " + SinAtender,
				labelPosition: 'bottom',
				intervals: [TotalProspectos - (p * 3), TotalProspectos - (p * 2), TotalProspectos - (p * 1), TotalProspectos],
				intervalColors: ['#f03e40', '#fca045', '#f4ef5f', '#15a85f']
			}
		}
	});
}

function KpiPreventa(TotalProspectos, Preventa, SinAtender) {
	s1 = [Preventa];

	var p = TotalProspectos / 4;

	plot4 = $.jqplot('cnvPreventa', [s1], {
		title: "Preventa",
		seriesDefaults: {
			renderer: $.jqplot.MeterGaugeRenderer,
			rendererOptions: {
				min: 0,
				max: TotalProspectos,
				label: Preventa + ' Clientes de ' + TotalProspectos + ", sin atender " + SinAtender,
				labelPosition: 'bottom',
				intervals: [TotalProspectos - (p * 3), TotalProspectos - (p * 2), TotalProspectos - (p * 1), TotalProspectos],
				intervalColors: ['#f03e40', '#fca045', '#f4ef5f', '#15a85f']
			}
		}
	});
}

function KpiMeta(Meta, Planeado) {
	s1 = [Planeado];

	var p = Meta / 3;

	plot4 = $.jqplot('cnvMeta', [s1], {
		title:"Planeado / Meta",
		seriesDefaults: {
			renderer: $.jqplot.MeterGaugeRenderer,
			rendererOptions: {
				min: 0,
				max: Meta*2,
				label: formato.moneda(Planeado, '$') + ' de \n' + formato.moneda(Meta, '$'),
				labelPosition: 'bottom',
				intervals: [Meta - (p * 2), Meta - (p * 1), Meta, Meta * 2],
				intervalColors: ['#f03e40', '#fca045', '#f4ef5f', '#15a85f']
			}
		}
	});
}

function KpiOportunidad(Meta, Ventas) {
	s1 = [Ventas];

	var p = Meta / 4;

	plot4 = $.jqplot('cnvPlaneacion', [s1], {
		title: "Facturado / Meta",
		seriesDefaults: {
			renderer: $.jqplot.MeterGaugeRenderer,
			rendererOptions: {
				min: 0,
				max: Meta,
				label: formato.moneda(Ventas, '$') + ' de \n' + formato.moneda(Meta, '$'),
				labelPosition: 'bottom',
				intervals: [Meta - (p * 3), Meta - (p * 2), Meta - (p * 1), Meta],
				intervalColors: ['#f03e40', '#fca045', '#f4ef5f', '#15a85f']
			}
		}
	});
}

function CargarTabla(Request) {
	$("#divTabla").obtenerVista({
		url: "Indicadores.aspx/ObtenerTabla",
		parametros: Request,
		nombreTemplate: "tmplTablaIndicadores.html"
	});
}

function ObtenerUsuarios() {
	MostrarBloqueo();
	$.ajax({
		url: "Indicadores.aspx/ObtenerUsuarios",
		type: "post",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var Usuarios = json.Modelo.Usuarios;
				for (x in Usuarios) {
					$("#cmbAgente").append($("<option value='" + Usuarios[x].Valor + "'>" + Usuarios[x].Nombre + "</option>"));
				}
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
			OcultarBloqueo();
		}
	});
}

function ObtenerSucursal() {
	MostrarBloqueo();
	$.ajax({
		url: "Indicadores.aspx/ObtenerSucursal",
		type: "post",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var Usuarios = json.Modelo.Usuarios;
				for (x in Usuarios) {
					$("#cmbSucursal").append($("<option value='" + Usuarios[x].Valor + "'>" + Usuarios[x].Nombre + "</option>"));
				}
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
			OcultarBloqueo();
		}
	});
}

function ObtenerDivision() {
	MostrarBloqueo();
	$.ajax({
		url: "Indicadores.aspx/ObtenerDivision",
		type: "post",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var Usuarios = json.Modelo.Usuarios;
				for (x in Usuarios) {
					$("#cmbDivision").append($("<option value='" + Usuarios[x].Valor + "'>" + Usuarios[x].Nombre + "</option>"));
				}
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
			OcultarBloqueo();
		}
	});
}
