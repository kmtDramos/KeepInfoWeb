<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Usuario.aspx.cs" Inherits="Usuario" Title="Seguridad" %>
<asp:Content ID="headSeguridadUsuarios" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Seguridad.Usuario.js?_=<%=DateTime.Now.Ticks %>"></script>
</asp:Content>
<asp:Content ID="bodySeguridadUsuarios" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
       <div id="dialogConsultarSucursalAsignada" title="Asignar sucursal"></div>
    <div id="dialogEditarContrasena" title="Editar contraseña">
        <div id="divEditarContrasena">
            <table class="tblEditarContrasena">
                <tr>
                    <td class="tblEditarContrasena-Etiqueta"><span id="lblContrasenaAdministrador" class="lblEtiqueta">Contraseña administrador:</span></td>
                    <td class="tblEditarContrasena-Control"><input type="password" id="txtContrasenaAdministrador" value="" /></td>
                </tr>
                <tr>
                    <td class="tblEditarContrasena-Etiqueta"><span id="lblContrasenaNueva" class="lblEtiqueta">Contraseña nueva:</span></td>
                    <td class="tblEditarContrasena-Control"><input type="password" id="txtContrasenaNueva" value="" /></td>
                </tr>
            </table>
        </div>
    </div>
    <!--Dialogs-->
    <div id="divContenido">
        <div id="divVistaFormas" class="renglon-top"></div>
        <div id="divGridUsuarios" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE USUARIOS-->
                <table id="grdUsuarios"></table>
                <div id="pagUsuarios"></div>
                <!--FIN DE GRID DE USUARIOS-->
            </div>
        </div>
    </div>
</asp:Content>