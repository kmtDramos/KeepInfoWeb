<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaEditarPerfil.aspx.cs" Inherits="FormaEditarPerfil" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Editar Perfil
            <div class="divTituloFormularioAcciones">
                <div class="divAcciones"><span onclick="javascript:EliminarPerfil();">Eliminar</span></div>
            </div>
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
            <input type="button" id="btnCancelar" value="Cancelar" class="button" onclick="javascript:SetFormaAltaPerfil();" tabindex="5"/>
            <input type="button" id="btnGuardar" value="Guardar" class="button" onclick="javascript:EditarPerfil();" tabindex="4"/>
        </div>
    </div>
</form>
