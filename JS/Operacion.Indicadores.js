/**/

$(function () {

	var VentanaIndicadores = $('<div id="divIndicadores" class="oculto"></div>');

	$("body").append(VentanaIndicadores);

	$(VentanaIndicadores).click(function () {
		if ($(VentanaIndicadores).hasClass("oculto"))
			$(VentanaIndicadores).toggleClass("visible oculto", 200)
	});

	var Indicadores = new Object();
	Indicadores.IdUsuario = 0;
	var Request = JSON.stringify(Indicadores);
	InitInidicadores(Request, 0);

});

function InitInidicadores(Request, IdUsuario) {
	$("#divIndicadores").obtenerVista({
		nombreTemplate: "tmplInidicadores.html",
		parametros: Request,
		url: "Indicador.aspx/ObtenerIndicadores",
		despuesDeCompilar: function (Respuesta) {
			var Modelo = Respuesta.modelo;
			if (Modelo.EsVendedor || true) {
				$("#btnCerrarIndicadores").click(function () {
					$("#divIndicadores").toggleClass("visible oculto", 200)
				});

				var porcentajeCliente = Modelo.ClientesAtendidos / Modelo.TotalClientes * 100;
				porcentajeCliente = (isNaN(porcentajeCliente)) ? 100 : porcentajeCliente;
				porcentajeCliente = (porcentajeCliente > 100) ? 100 : porcentajeCliente;
				var porcentajeVenta = Modelo.Venta / Modelo.Meta * 100;
				porcentajeVenta = (isNaN(porcentajeVenta)) ? 100 : porcentajeVenta;
				porcentajeVenta = (porcentajeVenta > 100) ? 100 : porcentajeVenta;
				var porcentajeOportunidad = Modelo.OportunidadesConSeguimiento / Modelo.TotalOportunidades * 100;
				porcentajeOportunidad = (isNaN(porcentajeOportunidad)) ? 100 : porcentajeOportunidad;
				porcentajeOportunidad = (porcentajeOportunidad > 100) ? 100 : porcentajeOportunidad;
				var porcentajeGlobal = (porcentajeCliente + porcentajeVenta + porcentajeOportunidad) / 3;

				$("#lvlIndicadoresClientesAtendidos").text(Modelo.ClientesAtendidos);
				$("#lvlIndicadoresTotalClientes").text(Modelo.TotalClientes);
				$("#lvlIndicadoresClientesPorcentaje").text(porcentajeCliente.toFixed(2));
				$("#lvlIndicadoresVenta").text(formato.moneda(Modelo.Venta,'$'));
				$("#lvlIndicadoresMeta").text(formato.moneda(Modelo.Meta, '$'));
				$("#lvlIndicadoresVentaPorcentaje").text(porcentajeVenta.toFixed(2));
				$("#lvlIndicadoresOportunidadesConSeguimiento").text(Modelo.OportunidadesConSeguimiento);
				$("#lvlIndicadoresTotalOportunidades").text(Modelo.TotalOportunidades);
				$("#lvlIndicadoresOportunidadesPorcentaje").text(porcentajeOportunidad.toFixed(2));
				$("#lvlIndicadoresGlobalPorcentaje").text(porcentajeGlobal.toFixed(2));

				$("#imgIndicadorClientes").attr("src", CaraIndicador(porcentajeCliente));
				$("#imgIndicadorVenta").attr("src", CaraIndicador(porcentajeVenta));
				$("#imgIndicadorOportunidades").attr("src", CaraIndicador(porcentajeOportunidad));
				$("#imgIndicadorGlobal").attr("src", CaraIndicador(porcentajeGlobal));

				$("#pbCliente").progressbar({ value: porcentajeCliente }).find(".ui-progressbar-value").css({ "background": ColorIndicador(porcentajeCliente) });
				$("#pbMeta").progressbar({ value: porcentajeVenta }).find(".ui-progressbar-value").css({ "background": ColorIndicador(porcentajeVenta) });
				$("#pbOportunidad").progressbar({ value: porcentajeOportunidad }).find(".ui-progressbar-value").css({ "background": ColorIndicador(porcentajeOportunidad) });

				$("#cmbUsuarioInidicadores").change(function(){

					var Indicadores = new Object();
					Indicadores.IdUsuario = parseInt($(this).val());
					var Request = JSON.stringify(Indicadores);
					InitInidicadores(Request, Indicadores.IdUsuario);
				}).val(IdUsuario);

			} else {
				$("#divIndicadores").hide();
			}
		}
	});
}

function CaraIndicador(Porcentaje) {
	var img = "";
	if (Porcentaje >= 80) {
		img = "../Images/face_happy.png";
	} else if (Porcentaje >= 60) {
		img = "../Images/face_agree.png";
	} else if (Porcentaje >= 40) {
		img = "../Images/face_disagree.png";
	} else if (Porcentaje > 0){
		img = "../Images/face_sad.png";
	} else {
		img = "../Images/face_angry.png";
	}
	return img;
}

function ColorIndicador(Porcentaje) {
	var rgb = "";
	if (Porcentaje >= 90) {
		rgb = "#5cd94b";
	} else if (Porcentaje >= 60) {
		rgb = "#f8ec31";
	} else if (Porcentaje >= 40) {
		rgb = "#f5ba48";
	} else {
		rgb = "#d53737";
	}
	return rgb;
}