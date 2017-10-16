<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaConsultarPerfil.aspx.cs" Inherits="FormaConsultarPerfil" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Consultar Perfil
            <div class="divTituloFormularioAcciones">
                <div class="divAcciones"><span onclick="javascript:EliminarPerfil();">Eliminar</span></div>
            </div>
        </div>
        <table class="tblFormaPerfil">
            <tr>
                <td class="tblFormaPerfil-Etiqueta-col1"><asp:Label ID="lblPerfil" runat="server" CssClass="lblEtiqueta">Perfil:</asp:Label></td>
                <td class="tblFormaPerfil-Control"><asp:Label ID="lblPerfilConsulta" runat="server"></asp:Label></td>
                <td class="tblFormaPerfil-Etiqueta-col2"><asp:Label ID="lblPagina" runat="server" CssClass="lblEtiqueta">Pagina de inicio:</asp:Label></td>
                <td class="tblFormaPerfil-Control"><asp:Label ID="lblPaginaConsulta" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="tblFormaPerfil-Etiqueta-col1"><asp:Label ID="lblEsPerfilSucursal" runat="server" CssClass="lblEtiqueta">Es perfil sucursal:</asp:Label></td>
                <td class="tblFormaPerfil-Control"><asp:CheckBox ID="chkEsPerfilSucursal" runat="server" Enabled="False"></asp:CheckBox></td>
                <td class="tblFormaPerfil-Etiqueta-col2"></td>
                <td class="tblFormaPerfil-Control"></td>
            </tr>
        </table>
        <div class="divAreaBotones">
            <input type="button" id="btnRegresar" value="Regresar" class="button" onclick="javascript:SetFormaAltaPerfil();"/>
            <input type="button" id="btnEditar" value="Editar" class="button" runat="server" />
        </div>
    </div>
</form>
