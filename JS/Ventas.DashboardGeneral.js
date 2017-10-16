/**/

$(function () {
	MantenerSesion();
	setInterval(MantenerSesion, 1000 * 60 * 2.5);

	ObtenerReporteDashboard();

	$("#btnPantallaCompleta").click(function (event) {
		
	});

});


function ObtenerReporteDashboard()
{
	$.ajax({
		url: "DashboardGeneral.aspx/ObtenerReporteDashboard",
		type: "post",
		dataType: "json",
		contentType: "application/json",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				var Usuarios = json.Modelo.Usuarios;
				for (x in Usuarios)
				{
					var tr = $("<tr></tr>");
					for (y in Usuarios[x])
					{
						td = $("<td>" + Usuarios[x][y] + "</td>");
						$(tr).append(td);
					}
					$("tbody", "#tblReporte").append(tr);
				}
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}
