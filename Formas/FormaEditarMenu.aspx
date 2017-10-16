<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaEditarMenu.aspx.cs" Inherits="FormaEditarMenu" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Editar Menú
            <div class="divTituloFormularioAcciones">
                <div class="divAcciones"><span onclick="javascript:EliminarMenu();">Eliminar</span></div>
            </div>
        </div>
        <table class="tblFormaMenu">
            <tr>
                <td class="tblFormaMenu-Etiqueta"><asp:Label ID="lblMenu" runat="server" CssClass="lblEtiqueta">Menú:</asp:Label></td>
                <td class="tblFormaMenu-Control"><asp:TextBox ID="txtMenu" runat="server" CssClass="txtCajaTexto" tabindex="2"></asp:TextBox></td>
                <td class="tblFormaMenu-Etiqueta"><asp:Label ID="lblProyectoSistema" runat="server" CssClass="lblEtiqueta">ProyectoSistema:</asp:Label></td>
                <td class="tblFormaMenu-Control"><%=ComboProyectoSistema()%></td>
            </tr>
        </table>
        <div class="divAreaBotones">
            <input type="button" id="btnCancelar" value="Cancelar" class="button" onclick="javascript:SetFormaAltaMenu();" tabindex="5" />
            <input type="button" id="btnGuardar" value="Guardar" class="button" onclick="javascript:EditarMenu();" tabindex="4" />
        </div>
    </div>
</form>
