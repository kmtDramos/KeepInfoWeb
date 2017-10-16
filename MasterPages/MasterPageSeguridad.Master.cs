using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Script.Services;

public partial class MasterPageSeguridad : System.Web.UI.MasterPage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        CSeguridad.ValidarPermisos();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = new HttpCookie("IdUsuario");
        HttpContext.Current.Response.SetCookie(cookie);
        int IdUsuario = (HttpContext.Current.Request.Cookies["IdUsuario"].Value != null) ? Convert.ToInt32(HttpContext.Current.Request.Cookies["IdUsuario"].Value.ToString()) : 0;
        HttpContext.Current.Session["IdUsuario"] = IdUsuario;
        string pagina = new System.IO.FileInfo(Page.Request.Url.AbsolutePath).Name;
        if (pagina != "InicioSesion.aspx")
        {
            string idUsuario = HttpContext.Current.Session["IdUsuario"].ToString();
            if (idUsuario == "")
            {
                idUsuario = "0";
            }
            if (Convert.ToInt32(idUsuario) == 0)
            { Response.Redirect("../InicioSesion.aspx"); }

            //Abrir Conexion
            CConexion ConexionBaseDatos = new CConexion();
            string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
            //¿La conexion se establecio?
            if (respuesta == "Conexion Establecida")
            {
                bool accesoDirecto = Convert.ToBoolean(HttpContext.Current.Request["AccesoDirecto"]);
                if (!accesoDirecto)
                {
                    CrearMenu(pagina, ConexionBaseDatos);
                    //divPanelControles.Attributes["activo"] = "false";
                }
                CrearTituloSeccion(pagina, accesoDirecto, ConexionBaseDatos);
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(idUsuario), ConexionBaseDatos);
                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
                CEmpresa Empresa = new CEmpresa();
                Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);
                divEncabezadoImagen.InnerHtml = "<img class='imgLogo' src='../Archivos/EmpresaLogo/" + Empresa.Logo + "' />";
				divEncabezadoImagen.InnerHtml += "";
            }

            //Cerrar Conexion
            ConexionBaseDatos.CerrarBaseDatos();
        }
        else
        {
            divEncabezadoImagen.InnerHtml = "<img class='imgBanner' src='" + ResolveClientUrl("~/images/banner-keep-info.jpg") + "' />";
            divEncabezadoTituloSeccion.Visible = false;
        }
        HttpCookie miSesion = new HttpCookie("IdUsuario");
        miSesion.Value = IdUsuario.ToString();
        HttpContext.Current.Response.SetCookie(miSesion);
    }

    public void CrearMenu(string pPagina, CConexion pConexion)
    {
        int IdUsuario = Convert.ToInt32(HttpContext.Current.Request.Cookies["IdUsuario"].Value.ToString());
        HttpContext.Current.Session["IdUsuario"] = IdUsuario;

        string carpeta = "";
        CPagina PaginaActual = new CPagina();
        PaginaActual.FiltrarPagina(pPagina, pConexion);
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pConexion);
        CPerfil Perfil = new CPerfil();

        if (Usuario.IdPerfil != 0)
        {
            Perfil.LlenaObjeto(Usuario.IdPerfil, pConexion);
        }
        else if (Usuario.TieneSucursalAsignada(pConexion))
        {
            CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
            Dictionary<string, object> ParametrosSucursalAsignada = new Dictionary<string, object>();
            ParametrosSucursalAsignada.Add("IdUsuario", Usuario.IdUsuario);
            ParametrosSucursalAsignada.Add("IdSucursal", Usuario.IdSucursalActual);
            SucursalAsignada.LlenaObjetoFiltros(ParametrosSucursalAsignada, pConexion);
            Perfil.LlenaObjeto(SucursalAsignada.IdPerfil, pConexion);
        }

        else
        {
            HttpContext.Current.Response.Redirect("../InicioSesion.aspx");
        }

        CPagina PaginaInicio = new CPagina();
        PaginaInicio.LlenaObjeto(Perfil.IdPagina, pConexion);
        CMenu MenuInicio = new CMenu();
        MenuInicio.LlenaObjeto(PaginaInicio.IdMenu, pConexion);
        CMenu MenuPrincipal = new CMenu();

        if (MenuInicio.IdMenu != PaginaActual.IdMenu)
        { carpeta = "../" + MenuInicio.Menu + "/"; }
        string liTituloMenuPrincipal = "<li><a href='../Paginas/" + PaginaInicio.Pagina + "'>Inicio</a></li>";

        foreach (CMenu Menu in MenuPrincipal.ObtenerMenuPrincipal(Perfil.IdPerfil, "Seguridad", pConexion))
        {
            CPagina Pagina = new CPagina();
            int bandera = 0;
            foreach (CPagina MenuSecundario in Pagina.ObtenerMenuSecundario(Menu.IdMenu, Perfil.IdPerfil, "Seguridad", pConexion))
            {
                if (bandera == 0)
                {
                    liTituloMenuPrincipal = liTituloMenuPrincipal + "<li id='li" + Menu.Menu + "'>" + Menu.Menu + "";
                    liTituloMenuPrincipal = liTituloMenuPrincipal + "<ul>";
                    bandera = 1;
                }
                if (PaginaActual.IdMenu == MenuSecundario.IdMenu)
                { carpeta = ""; }
                else
                { carpeta = "../" + Menu.Menu + "/"; }
                liTituloMenuPrincipal = liTituloMenuPrincipal + "<li><a href='../Paginas/" + MenuSecundario.Pagina + "'>" + MenuSecundario.NombreMenu + "</a></li>";
            }
            if (bandera == 1)
            { liTituloMenuPrincipal = liTituloMenuPrincipal + "</ul>"; }
            liTituloMenuPrincipal = liTituloMenuPrincipal + "</li>";
        }
        liTituloMenuPrincipal = liTituloMenuPrincipal + "<li><a href='http://www.tsk.com.mx/Procesos/consulta.asp' target='_blank'>Procesos</a></li>";
        liTituloMenuPrincipal = liTituloMenuPrincipal + "<li><a href='../InicioSesion.aspx'>Salir</a></li>";
        HtmlGenericControl ulMenu = this.Page.FindControl("ctl00$ulMenuSeguridad") as HtmlGenericControl;
        ulMenu.InnerHtml = liTituloMenuPrincipal;

        HttpCookie miSesion = new HttpCookie("IdUsuario");
        miSesion.Value = IdUsuario.ToString();
        HttpContext.Current.Response.SetCookie(miSesion);
    }

    public void CrearTituloSeccion(string pPagina, bool pAccesoDirecto, CConexion pConexion)
    {
        CPagina Pagina = new CPagina();
        Pagina.FiltrarPagina(pPagina, pConexion);
        divTituloSeccion.InnerText = Pagina.Titulo;
        if (!pAccesoDirecto)
        {
            int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(IdUsuario, pConexion);

            int Actividades = 0;

            CSelectEspecifico Select = new CSelectEspecifico();

            Select.StoredProcedure.CommandText = "sp_Usuario_ContarActividadesHoy";
            Select.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;
            Select.Llena(pConexion);

            if(Select.Registros.Read()) {
                Actividades = Convert.ToInt32(Select.Registros["Actividades"]);
            }

            Select.CerrarConsulta();

            divUsuarioSesion.InnerHtml = "Actividades: (" + Actividades + ") Usuario: <span id='spanUsuario'>" + Usuario.Nombre + ' ' + Usuario.ApellidoPaterno + ' ' + Usuario.ApellidoMaterno + "<img id='imgEditarDatosUsuario' src='../images/editar.png' style='width:12px;'/></span>";
        }
    }
}