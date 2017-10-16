<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaAltaMenu.aspx.cs" Inherits="FormaAltaMenu" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Alta de Menú
            <div class="divTituloFormularioAcciones"></div>
        </div>
        <table class="tblFormaMenu">
            <tr>
                <td class="tblFormaMenu-Etiqueta"><asp:Label ID="lblMenu" runat="server" CssClass="lblEtiqueta">Menú:</asp:Label></td>
                <td class="tblFormaMenu-Control"><asp:TextBox ID="txtMenu" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
                <td class="tblFormaMenu-Etiqueta"><asp:Label ID="lblProyectoSistema" runat="server" CssClass="lblEtiqueta">ProyectoSistema:</asp:Label></td>
                <td class="tblFormaMenu-Control"><%=ComboProyectoSistema()%></td>
            </tr>
        </table>
        <div class="divAreaBotones">
            <input type="button" id="btnGuardar" value="Guardar" class="button" onclick="javascript:AgregarMenu();" />
        </div>
    </div>
</form>


