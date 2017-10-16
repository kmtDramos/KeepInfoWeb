<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaEditarUsuario.aspx.cs" Inherits="FormaEditarUsuario" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Editar Usuario
            <div class="divTituloFormularioAcciones">
                <div class="divAcciones">
                    <!--<span onclick="javascript:EliminarUsuario();">Eliminar</span>-->
                </div>
            </div>
        </div>
        <table class="tblFormaUsuario">
            <tr>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblNombre" runat="server" CssClass="lblEtiqueta">Nombre:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:TextBox ID="txtNombre" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblApellidoPaterno" runat="server" CssClass="lblEtiqueta">Apellido Paterno:</asp:Label></td>
                <td class=""><asp:TextBox ID="txtApellidoPaterno" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblApellidoMaterno" runat="server" CssClass="lblEtiqueta">Apellido Materno:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:TextBox ID="txtApellidoMaterno" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblFechaNacimiento" runat="server" CssClass="lblEtiqueta">Fecha de Nacimiento:</asp:Label></td>
                <td><asp:TextBox ID="txtFechaNacimiento" runat="server" ReadOnly="True" CssClass="txtCajaTexto"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblCorreo" runat="server" CssClass="lblEtiqueta">Correo:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:TextBox ID="txtCorreo" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblPerfil" runat="server" CssClass="lblEtiqueta">Perfil:</asp:Label></td>
                <td><select id="cmbPerfil" class="cmbComboBox"><option value="0">Elegir una opción...</option></select></td>
            </tr>
            <tr>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblUsuario" runat="server" CssClass="lblEtiqueta">Usuario:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:TextBox ID="txtUsuario" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblContrasena" runat="server" CssClass="lblEtiqueta">Contraseña:</asp:Label></td>
                <td><img id="imgEditarContrasena" src="../images/editar.png" class="cursorPointer" style="width:16px;" /></td>
            </tr>
            <tr>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblEsAgente" runat="server" CssClass="lblEtiqueta">Agente:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:CheckBox ID="chkEsAgente" runat="server"></asp:CheckBox></td>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblEsVendedor" runat="server" CssClass="lblEtiqueta">Vendedor:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:CheckBox ID="chkEsVendedor" runat="server"></asp:CheckBox></td>
            </tr>
            <tr>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblAlcance1" runat="server" CssClass="lblEtiqueta">Alcance 1:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:TextBox ID="txtAlcance1" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblAlcance2" runat="server" CssClass="lblEtiqueta">Alcance 2:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:TextBox ID="txtAlcance2" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblMeta" runat="server" CssClass="lblEtiqueta">Meta:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:TextBox ID="txtMeta" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblClientesNuevos" runat="server" CssClass="lblEtiqueta">Clientes Nuevos:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:TextBox ID="txtClientesNuevos" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
            </tr>
        </table>
        <div class="divAreaBotones">
            <input type="button" id="btnFormaAltaUsuario" value="Cancelar" class="button" />
            <input type="button" id="btnEditarUsuario" value="Editar" class="button" />
        </div>
    </div>
</form>
