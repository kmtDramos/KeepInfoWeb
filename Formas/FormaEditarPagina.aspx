<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaEditarPagina.aspx.cs" Inherits="FormaEditarPagina" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Editar página
            <div class="divTituloFormularioAcciones">
                <div class="divAcciones"><span onclick="javascript:EliminarPagina();">Eliminar</span></div>
            </div>
        </div>
        <table class="tblFormaPagina">
            <tr>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblPagina" runat="server" CssClass="lblEtiqueta">Página:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:TextBox ID="txtPagina" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblTitulo" runat="server" CssClass="lblEtiqueta">Título:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:TextBox ID="txtTitulo" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblNombreMenu" runat="server" CssClass="lblEtiqueta">Nombre menú:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:TextBox ID="txtNombreMenu" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblMenu" runat="server" CssClass="lblEtiqueta">Menú padre:</asp:Label></td>
                <td class="tblFormaPagina-Control"><%=ComboMenu()%></td>
            </tr>
            <tr>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblValidarSucursal" runat="server" CssClass="lblEtiqueta">Validar sucursal:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:CheckBox ID="chkValidarSucursal" runat="server"></asp:CheckBox></td>
                <td class="tblFormaPagina-Etiqueta"></td>
                <td class="tblFormaPagina-Control"></td>
            </tr>
        </table>
        <div class="divAreaBotones">
            <input type="button" id="btnCancelar" value="Cancelar" class="button" onclick="javascript:SetFormaAltaPagina();" tabindex="6"/>
            <input type="button" id="btnGuardar" value="Guardar" class="button" onclick="javascript:EditarPagina();" tabindex="5"/>
        </div>
    </div>
</form>
