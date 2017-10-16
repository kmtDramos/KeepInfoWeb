<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaAltaUsuario.aspx.cs" Inherits="FormaAltaUsuario" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Alta de usuario
            <div class="divTituloFormularioAcciones"></div>
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
                <td><%=ComboPerfil()%></td>
            </tr>
            <tr>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblUsuario" runat="server" CssClass="lblEtiqueta">Usuario:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:TextBox ID="txtUsuario" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblContrasena" runat="server" CssClass="lblEtiqueta">Contraseña:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:TextBox ID="txtContrasena" runat="server" TextMode="password" CssClass="txtCajaTexto"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblAgente" runat="server" CssClass="lblEtiqueta">Agente:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:CheckBox ID="chkEsAgente" runat="server"></asp:CheckBox></td>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblVendedor" runat="server" CssClass="lblEtiqueta">Vendedor:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:CheckBox ID="chkEsVendedor" runat="server"></asp:CheckBox></td>
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
            <input type="button" id="btnAgregarUsuario" value="Guardar" class="button" />
        </div>
    </div>
</form>