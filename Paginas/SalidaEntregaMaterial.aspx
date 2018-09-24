<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="SalidaEntregaMaterial.aspx.cs" Inherits="Paginas_SalidaEntregaMaterial" Title="Entrega Material" %>
<asp:Content ID="headCatalogoTiempoEntrega" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">

    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    
	<!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>

	<script type="text/javascript" src="../js/Almacen.SalidaEntregaMaterial.js?_=<%= DateTime.Now.Ticks %>"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
	<div id="dialogConsultarSolicitudEntregaMaterial" title ="Consultar Solicitud Entrega Material"></div>
    <div id="dialogEditarSolicitudEntregaMaterial" title ="Editar Solicitud Entrega Material"></div>

    <div id="dialogAgregarSalidaMaterial" title ="Agregar Salida Material"></div>
    <div id="dialogConsultarSalidaMaterial" title ="Consultar Salida Material"></div>

    <div id="divReportes" style="margin:10px;">
		<ul>
			<li><a href="#tabEntregaMaterial">Solicitud de Entrega Material</a></li>
			<li><a href="#tabSalidaMaterial">Salida Material</a></li>
		</ul>
		<div id="tabEntregaMaterial" style="font-size:9px;min-height:450px;">
            <div id="divContenido">
                <div style="padding:10px;">
                    <div class="divAreaBotonesDialog">
                        <!--input type='button' id='btnObtenerFormaAgregarCatalago' value='+ Agregar Produto' class='buttonLTR'/-->
                    </div>
		            <div id="divGridEntregaMaterial" class="divContGrid renglon-bottom">
			            <div id="divContGrid">
				            <!--INICIO GRID DE Entrega Material-->
				            <table id="grdEntregaMaterial"></table>
				            <div id="pagEntregaMaterial"></div>
				            <!--FIN DE GRID DE Entrega Material-->
			            </div>
		            </div>
	            </div>
            </div>
		</div>
		<div id="tabSalidaMaterial" style="font-size:9px;min-height:450px;">
            <div id="divContenido">
                <div id="divFiltrosSalidaMaterial"></div>  
                <div class="divAreaBotonesDialog">
                    <input type='button' id='btnObtenerFormaAgregarSalidaMaterial' value='+ Agregar Salida Material' class='buttonLTR'/>
                </div>
                <div id="divGridSalidaMaterial" class="divContGrid renglon-bottom">
                    <div id="divContGrid">
                        <!--INICIO GRID DE MEDIO ENTERO-->
                        <table id="grdSalidaMaterial"></table>
                        <div id="pagSalidaMaterial"></div>
                        <!--FIN DE GRID DE MEDIO ENTERO-->
                    </div>
                </div>
            </div>
		</div>
	</div>
    
    
</asp:Content>