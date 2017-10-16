<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaAltaPerfil.aspx.cs" Inherits="FormaAltaPerfil" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Alta de Perfil
            <div class="divTituloFormularioAcciones"></div>
        </div>
        <table class="tblFormaPerfil">
            <tr>
                <td class="tblFormaPerfil-Etiqueta-col1"><asp:Label ID="lblPerfil" runat="server" CssClass="lblEtiqueta">Perfil:</asp:Label></td>
                <td class="tblFormaPerfil-Control"><asp:TextBox ID="txtPerfil" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
                <td class="tblFormaPerfil-Etiqueta-col2"><asp:Label ID="lblPagina" runat="server" CssClass="lblEtiqueta">Pagina de inicio:</asp:Label></td>
                <td class="tblFormaPerfil-Control"><%=ComboPagina()%></td>
            </tr>
            <tr>
                <td class="tblFormaPerfil-Etiqueta-col1"><asp:Label ID="lblEsPerfilSucursal" runat="server" CssClass="lblEtiqueta">Es perfil sucursal:</asp:Label></td>
                <td class="tblFormaPerfil-Control"><asp:CheckBox ID="chkEsPerfilSucursal" runat="server"></asp:CheckBox></td>
                <td class="tblFormaPerfil-Etiqueta-col2"></td>
                <td class="tblFormaPerfil-Control"></td>
            </tr>
        </table>
        <div class="divAreaBotones">
            <input type="button" id="btnGuardar" value="Guardar" class="button" onclick="javascript:AgregarPerfil();" tabindex="4"/>
        </div>
    </div>
</form>