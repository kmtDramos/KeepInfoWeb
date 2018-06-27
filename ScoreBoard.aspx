<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScoreBoard.aspx.cs" Inherits="ScoreBoard" %>
<DOCTYPE html>
<html>
<head>
	<title>ScoreBoard</title>
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<link rel="stylesheet" href="css/bootstrap.min.css">
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.6.0/css/font-awesome.min.css">
	<link rel="stylesheet" href="css/mdb.min.css" />
	<script src="https://code.jquery.com/jquery-1.12.4.min.js" integrity="sha256-ZosEbRLbNQzLpnKIkEdrPv7lOy9C27hHQ+Xp8a4MxAQ=" crossorigin="anonymous"></script>
	<script src="js/bootstrap.min.js"></script>
	<script type="text/javascript" src="js/ScoreBoard.js?_=<%=DateTime.Now.Ticks %>"></script>
	<script type="text/javascript" src="js/mdb.min.js"></script>
</head>

<body class="grey lighten-3">
	<div class="container p-2">
		<div class="card">
			<div class="car-header elegant-color-dark lighten-1 white-text text-center">
				<h1 class="p-2">Resultado de Ventas Mes Actual</h1>
			</div>
			<div class="card-body">
				<table class="table" id="ScoreBoard">
					<thead>
						<tr class="orange lighten-2">
							<th>#</th>
							<th>Agente</th>
							<th>Mes</th>
							<th>Semana</th>
						</tr>
					</thead>
					<tbody></tbody>
				</table>
			</div>
		</div>
	</div>
</body>
</html>