/**/
var tablaVendedores;
var tablaDivisiones;
var dt;
var vt;
$(function () {

	MantenerSesion();

	setInterval(MantenerSesion, 1000 * 60 * 1.5);

	$("#divReportes").tabs();

	$("#btnActualizarReporteador").click(ObtenerTotalesReporteador);

	$("#txtFechaInicial").datepicker();

	$("#txtFechaFinal").datepicker();

	LlenaSucursales();

	$('#divReportes').bind('tabsshow', function (event, ui) {
	    switch (ui.index) {
	        case 0:
	            $("#tblDivisiones").DataTable().draw();
	            break;
	        case 1:
	            $("#tblVendedores").DataTable().draw();
	            break;
	    }
	    $(".DTFC_LeftBodyWrapper, .DTFC_LeftBodyLiner").css('height', '');
	});

	$("#txtUsuario").autocomplete({
		source: function (request, response) {
			var Usuario = new Object();
			Usuario.Usuario = request.term;
			var Request = JSON.stringify(Usuario);
			$.ajax({
				url: "ReporteadorFacturacion.aspx/ObtenerUsuario",
				type: "json",
				data: Request,
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (Respuesta) {
					var json = JSON.parse(Respuesta.d);
				}
			});
		},
		minLength: 2,
		select: function () {
			
		}
	});

});


function ObtenerTotalesReporteador() {

	MostrarBloqueo();

    var Division = new Object();
    Division.IdSucursal = parseInt($("#txtSucursal").val());
    Division.FechaInicial = $("#txtFechaInicial").val();
    Division.FechaFinal = $("#txtFechaFinal").val();

    $("#tabDivisiones").empty();
    $("#tabVendedores").empty();

    var Request = JSON.stringify(Division);

    $.ajax({
        url: "ReporteadorFacturacion.aspx/ObtenerTotalesReporteador",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                var Modelo = json.Modelo;
                var Divisiones = Modelo.Divisiones;
                var Vendedores = Modelo.Vendedores;

                tablaDivisiones = $('<table id="tblDivisiones" class="stripe row-border order-column" cellspacing="0" height="450" width="100%"><thead><tr></tr></thead><tbody></tbody></table>');
                tablaVendedores = $('<table id="tblVendedores" class="stripe row-border order-column" cellspacing="0" height="450" width="100%"><thead><tr></tr></thead><tbody></tbody></table>');

                for (x in Divisiones[0]) {
                    $("thead  tr", tablaDivisiones).append("<th>" + x + "</th>");
                }

                for (x in Vendedores[0]) {
                    $("thead  tr", tablaVendedores).append("<th>" + x + "</th>");
                }

                $("#tabDivisiones").append(tablaDivisiones);
                $("#tabVendedores").append(tablaVendedores);

                dt = $("#tblDivisiones").DataTable({
                    "oLanguage": { "sUrl": "../JS/Spanish.json" },
                    scrollX:        true,
                    scrollCollapse: false,
                    fixedColumns: {
                        leftColumns: 5,
                        heightMatch: 'none'
                    }
                });
                for (x in Divisiones) {
                    var fila = [];
                    for (y in Divisiones[x]) {
                        fila.push(Divisiones[x][y])
                    }
                    dt.row.add(fila).draw(false);
                }
                dt.draw();

                vt = $("#tblVendedores").DataTable({
                    "oLanguage": { "sUrl": "../JS/Spanish.json" },
                    scrollX: true,
                    scrollCollapse: false,
                    fixedColumns: {
                        leftColumns: 5,
                        heightMatch: 'none'
                    }
                });
                for (x in Vendedores) {
                    var fila = [];
                    for (y in Vendedores[x]) {
                        fila.push(Vendedores[x][y])
                    }
                    vt.row.add(fila).draw(false);
                }
                vt.draw();

                setTimeout(function () {
                	$(".DTFC_LeftBodyWrapper, .DTFC_LeftBodyLiner").css('height', '');
                	console.log("que pedo");
                }, 1000);

                OcultarBloqueo();
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });

}

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
			    //sucursales.clear();
			    var select = document.getElementById('txtSucursal');
				for (x in Sucursales)
				{
				    //console.log(Sucursales[x]);
				    var opt = document.createElement('option');
				    opt.value = Sucursales[x].Valor;
				    opt.innerHTML = Sucursales[x].Descripcion;
				    select.appendChild(opt);
				}
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}