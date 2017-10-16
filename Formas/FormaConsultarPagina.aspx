<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaConsultarPagina.aspx.cs" Inherits="FormaConsultarPagina" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Consultar página
            <div class="divTituloFormularioAcciones">
                <div class="divAcciones"><span onclick="javascript:EliminarPagina();">Eliminar</span></div>
            </div>
        </div>
        <table class="tblFormaPagina">
            <tr>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblPagina" runat="server" CssClass="lblEtiqueta">Página:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:Label ID="lblPaginaConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblTitulo" runat="server" CssClass="lblEtiqueta">Título:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:Label ID="lblTituloConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
            </tr>
            <tr>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblNombreMenu" runat="server" CssClass="lblEtiqueta">Nombre menú:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:Label ID="lblNombreMenuConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblMenuPadre" runat="server" CssClass="lblEtiqueta">Menú padre:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:Label ID="lblMenuPadreConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
            </tr>
            <tr>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblValidarSucursal" runat="server" CssClass="lblEtiqueta">Validar sucursal:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:CheckBox ID="chkValidarSucursal" runat="server" Enabled="False"></asp:CheckBox></td>
                <td class="tblFormaPagina-Etiqueta"></td>
                <td class="tblFormaPagina-Control"></td>
            </tr>
        </table>
        <div class="divAreaBotones">
            <input type="button" id="btnRegresar" value="Regresar" class="button" onclick="javascript:SetFormaAltaPagina();"/>
            <input type="button" id="btnEditar" value="Editar" class="button" runat="server" />
        </div>
    </div>
</form>
