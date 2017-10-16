<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaConsultarUsuario.aspx.cs" Inherits="FormaConsultarUsuario" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Consultar Usuario
            <div class="divTituloFormularioAcciones">
                <div class="divAcciones">
                    <!--<span onclick="javascript:EliminarUsuario();">Eliminar</span>-->
                </div>
            </div>
        </div>
        <table class="tblFormaUsuario">
            <tr>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblNombre" runat="server" CssClass="lblEtiqueta">Nombre:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:Label ID="lblNombreConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblApellidoPaterno" runat="server" CssClass="lblEtiqueta">Apellido Paterno:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:Label ID="lblApellidoPaternoConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
            </tr>
            <tr>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblApellidoMaterno" runat="server" CssClass="lblEtiqueta">Apellido Materno:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:Label ID="lblApellidoMaternoConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblFechaNacimiento" runat="server" CssClass="lblEtiqueta">Fecha de Nacimiento:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:Label ID="lblFechaNacimientoConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
            </tr>
            <tr>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblCorreo" runat="server" CssClass="lblEtiqueta">Correo:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:Label ID="lblCorreoConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblPerfil" runat="server" CssClass="lblEtiqueta">Perfil:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:Label ID="lblPerfilConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
            </tr>
            <tr>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblUsuario" runat="server" CssClass="lblEtiqueta">Usuario:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:Label ID="lblUsuarioConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
                <td class="tblFormaUsuario-Etiqueta"><asp:Label ID="lblContrasena" runat="server" CssClass="lblEtiqueta">Contraseña:</asp:Label></td>
                <td class="tblFormaUsuario-Control"><asp:Label ID="lblContrasenaConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
            </tr>
            <tr>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblEsAgente" runat="server" CssClass="lblEtiqueta">Agente:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:CheckBox ID="chkEsAgente" runat="server" Enabled="False"></asp:CheckBox></td>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="Label1" runat="server" CssClass="lblEtiqueta">Vendedor:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:CheckBox ID="chkEsVendedor" runat="server" Enabled="False"></asp:CheckBox></td>
            </tr>
            <tr>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblAlcance1" runat="server" CssClass="lblEtiqueta">Alcance 1:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:Label ID="lblAlcance1Consulta" runat="server" CssClass="lblEtiqueta"></asp:Label></td>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblAlcance2" runat="server" CssClass="lblEtiqueta">Alcance 2:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:Label ID="lblAlcance2Consulta" runat="server" CssClass="lblEtiqueta"></asp:Label></td>
            </tr>
            <tr>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblMeta" runat="server" CssClass="lblEtiqueta">Meta:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:Label ID="lblMetaConsulta" runat="server" CssClass="lblEtiqueta"></asp:Label></td>
                <td class="tblFormaPagina-Etiqueta"><asp:Label ID="lblClientesNuevos" runat="server" CssClass="lblEtiqueta">Clientes Nuevos:</asp:Label></td>
                <td class="tblFormaPagina-Control"><asp:Label ID="lblClientesNuevosConsulta" runat="server" CssClass="lblEtiqueta"></asp:Label></td>
            </tr>
        </table>
        <div class="divAreaBotones">
            <input type="button" id="btnFormaAltaUsuario" value="Regresar" class="button" />
            <input type="button" id="btnFormaEditarUsuario" value="Editar" class="button" />
        </div>
    </div>
</form>