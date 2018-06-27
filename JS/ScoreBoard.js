/**/

$(function () {
	ObtenerResultados();
	setInterval(ObtenerResultados, 1000 * 60);
});

function ObtenerResultados() {
	$.ajax({
		url: "ScoreBoard.aspx/ObtenerResultado",
		type: "post",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var Resultados = json.Modelo.Resultados;
				$("#ScoreBoard tbody").html('');
				for (x in Resultados)
				{
					var tr = $("<tr></tr>");
					for (y in Resultados[x]) {
						$(tr).append($("<td>" + Resultados[x][y] + "</td>"));
					}
					$("#ScoreBoard tbody").append(tr);
				}
			}
		}
	});
}