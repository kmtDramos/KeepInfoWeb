<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Addenda.aspx.cs" Inherits="Addenda" Title="Addenda" %>
<asp:Content ID="headAddenda" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.Addenda.js"></script>
</asp:Content>
<asp:Content ID="bodyAddenda" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarAddenda" title ="Agregar addenda"></div>
<div id="dialogConsultarAddenda" title ="Consultar addenda"></div>
<div id="dialogEditarAddenda" title ="Editar addenda"></div>
<div id="dialogConsultarEstructuraAddendaConsultar" title ="Consultar estructura addenda"></div>
<div id="dialogEditarEstructuraAddenda" title ="Editar estructura addenda"></div>

<div id="dialogConsultarEstructuraAddenda" title ="Estructura addenda">
    <div id="divFormaConsultarEstructuraAddenda"></div>
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarEstructuraAddenda" value="+ Agregar Elemento" class="buttonLTR" />
    </div>
    <div id="divGridEstructuraAddenda" class="divContGrid renglon-bottom">
        <div id="divContGridEstructuraAddenda">
            <table id="grdEstructuraAddenda"></table>
            <div id="pagEstructuraAddenda"></div>
        </div>
    </div>
</div>

<div id="dialogAgregarEstructuraAddenda" title="Agregar serie de factura">
    <div id="divFormaAgregarEstructuraAddenda"></div>
    <div id="divGridEstructuraAddenda" class="divContGrid renglon-bottom">
        <div id="divContGridEstructuraAddenda">
            <table id="grdEstructuraAddenda"></table>
            <div id="pagEstructuraAddenda"></div>
        </div>
    </div>    
</div>

<div id="divContenido">
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarAddenda" value="+ Agregar addenda" class="buttonLTR" />
    </div>
    <div id="divGridAddenda" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE ADDENDA-->
            <table id="grdAddenda"></table>
            <div id="pagAddenda"></div>
            <!--FIN DE GRID DE ADDENDA-->
        </div>
    </div>
</div>
</asp:Content>
