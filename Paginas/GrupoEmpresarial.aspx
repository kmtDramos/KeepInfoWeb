<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="GrupoEmpresarial.aspx.cs" Inherits="GrupoEmpresarial" Title="Grupo empresarial" %>
<asp:Content ID="headGrupoEmpresarial" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/librerias/jquery.maskedinput-1.2.2.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.GrupoEmpresarial.js"></script>
</asp:Content>
<asp:Content ID="bodyGrupoEmpresarial" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarGrupoEmpresarial" title ="Agregar grupo empresarial"></div>
    <div id="dialogConsultarGrupoEmpresarial" title ="Consultar grupo empresarial"></div>
    <div id="dialogEditarGrupoEmpresarial" title ="Editar grupo empresarial"></div>
    <div id="divContenido">
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarGrupoEmpresarial" value="+ Agregar grupo empresarial" class="buttonLTR" />
        </div>
        <div id="divGridGrupoEmpresarial" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE GRUPO EMPRESARIAL-->
                <table id="grdGrupoEmpresarial"></table>
                <div id="pagGrupoEmpresarial"></div>
                <!--FIN DE GRID DE GRUPO EMPRESARIAL-->
            </div>
        </div>
    </div>
</asp:Content>
