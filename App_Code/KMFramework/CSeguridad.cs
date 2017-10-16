using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

public class CSeguridad
{
    public static void ValidarPermisos()
    {
        string pagina = HttpContext.Current.Request.Url.LocalPath;
        pagina = pagina.Substring(pagina.LastIndexOf("/") + 1);

        if (pagina != "InicioSesion.aspx")
        {
            if (Convert.ToString(HttpContext.Current.Session["IdUsuario"]) == "")
            { HttpContext.Current.Response.Redirect("../InicioSesion.aspx"); }
            else
            {
                CConexion ConexionBaseDatos = new CConexion();
                string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

                CPagina Pagina = new CPagina();
                Dictionary<string, object> ParametrosPagina = new Dictionary<string, object>();
                ParametrosPagina.Add("Pagina", pagina);
                foreach (CPagina oPagina in Pagina.LlenaObjetoFiltros(ParametrosPagina, ConexionBaseDatos))
                {
                    Pagina = oPagina;
                }

                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

                //--SIN PERFIL DIRECTO, PERFIL RELACIONADO CON LA SUCURSAL ASIGNADA.
                if (Usuario.IdPerfil == 0)
                {
                    if (Pagina.ValidarSucursal == true)
                    {
                        CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
                        Dictionary<string, object> ParametrosSucursalAsignada = new Dictionary<string, object>();
                        ParametrosSucursalAsignada.Add("IdUsuario", Usuario.IdUsuario);
                        ParametrosSucursalAsignada.Add("IdSucursal", Usuario.IdSucursalActual);
                        SucursalAsignada.LlenaObjetoFiltros(ParametrosSucursalAsignada, ConexionBaseDatos);

                        CPerfil Perfil = new CPerfil();
                        Perfil.LlenaObjeto(SucursalAsignada.IdPerfil, ConexionBaseDatos);

                        CPagina PaginaInicio = new CPagina();
                        PaginaInicio.LlenaObjeto(Perfil.IdPagina, ConexionBaseDatos);

                        if (Usuario.TienePermisos(new string[] { "controlTotal" }, Perfil.IdPerfil, ConexionBaseDatos) != "")
                        {
                            if (Usuario.PermisoPerfilSucursal(Pagina.Pagina, SucursalAsignada.IdPerfil, ConexionBaseDatos) == true)
                            {
                                HttpContext.Current.Response.Redirect(PaginaInicio.Pagina);
                            }
                        }
                    }
                    else
                    {
                        CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
                        Dictionary<string, object> ParametrosSucursalAsignada = new Dictionary<string, object>();
                        ParametrosSucursalAsignada.Add("IdUsuario", Usuario.IdUsuario);
                        ParametrosSucursalAsignada.Add("IdSucursal", Usuario.IdSucursalActual);
                        SucursalAsignada.LlenaObjetoFiltros(ParametrosSucursalAsignada, ConexionBaseDatos);

                        CPerfil Perfil = new CPerfil();
                        Perfil.LlenaObjeto(SucursalAsignada.IdPerfil, ConexionBaseDatos);

                        CPagina PaginaInicio = new CPagina();
                        PaginaInicio.LlenaObjeto(Perfil.IdPagina, ConexionBaseDatos);
                        if (Usuario.TienePermisos(new string[] { "controlTotal" }, Perfil.IdPerfil, ConexionBaseDatos) != "")
                        {
                            if (Usuario.PermisoPerfilSucursal(Pagina.Pagina, SucursalAsignada.IdPerfil, ConexionBaseDatos) == true)
                            {
                                HttpContext.Current.Response.Redirect(PaginaInicio.Pagina);
                            }
                        }
                    }
                }
                //--CON PERFIL DIRECTO Y PERFIL ASIGNADO A SUCURSAL.
                else
                {
                    if (Pagina.ValidarSucursal == true)
                    {
                        CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
                        Dictionary<string, object> ParametrosSucursalAsignada = new Dictionary<string, object>();
                        ParametrosSucursalAsignada.Add("IdUsuario", Usuario.IdUsuario);
                        ParametrosSucursalAsignada.Add("IdSucursal", Usuario.IdSucursalActual);
                        SucursalAsignada.LlenaObjetoFiltros(ParametrosSucursalAsignada, ConexionBaseDatos);

                        CPerfil Perfil = new CPerfil();
                        Perfil.LlenaObjeto(SucursalAsignada.IdPerfil, ConexionBaseDatos);

                        CPagina PaginaInicio = new CPagina();
                        PaginaInicio.LlenaObjeto(Perfil.IdPagina, ConexionBaseDatos);
                        if (Usuario.TieneSucursalAsignada(ConexionBaseDatos) == true && Usuario.TienePermisos(new string[] { "controlTotal" }, Perfil.IdPerfil, ConexionBaseDatos) != "" && Usuario.PermisoPerfilSucursal(Pagina.Pagina, SucursalAsignada.IdPerfil, ConexionBaseDatos) == true)
                        {
                            if (PaginaInicio.ValidarSucursal == true || PaginaInicio.Pagina == pagina)
                            {
                                Perfil.LlenaObjeto(Usuario.IdPerfil, ConexionBaseDatos);
                                PaginaInicio.LlenaObjeto(Perfil.IdPagina, ConexionBaseDatos);

                                if (Usuario.TienePermisos(new string[] { "controlTotal" }, ConexionBaseDatos) == "" || Usuario.Permiso(pagina, ConexionBaseDatos))
                                {
                                    if (PaginaInicio.ValidarSucursal == true || PaginaInicio.Pagina == pagina)
                                    {
                                        HttpContext.Current.Response.Redirect("../InicioSesion.aspx");
                                    }
                                    else
                                    {
                                        HttpContext.Current.Response.Redirect(PaginaInicio.Pagina);
                                    }
                                }
                            }
                        }
                        else if (Usuario.TieneSucursalAsignada(ConexionBaseDatos) == false)
                        {
                            Perfil.LlenaObjeto(Usuario.IdPerfil, ConexionBaseDatos);
                            PaginaInicio.LlenaObjeto(Perfil.IdPagina, ConexionBaseDatos);

                            if (Usuario.TienePermisos(new string[] { "controlTotal" }, ConexionBaseDatos) == "" || Usuario.Permiso(pagina, ConexionBaseDatos))
                            {
                                if (PaginaInicio.ValidarSucursal == true || PaginaInicio.Pagina == pagina)
                                {
                                    HttpContext.Current.Response.Redirect("../InicioSesion.aspx");
                                }
                                else
                                {
                                    HttpContext.Current.Response.Redirect(PaginaInicio.Pagina);
                                }
                            }
                        }
                    }
                    else
                    {
                        CPerfil Perfil = new CPerfil();
                        Perfil.LlenaObjeto(Usuario.IdPerfil, ConexionBaseDatos);

                        CPagina PaginaInicio = new CPagina();
                        PaginaInicio.LlenaObjeto(Perfil.IdPagina, ConexionBaseDatos);

                        if (Usuario.TienePermisos(new string[] { "controlTotal" }, ConexionBaseDatos) != "")
                        {
                            if (Usuario.Permiso(pagina, ConexionBaseDatos) == false)
                            {
                                if (PaginaInicio.ValidarSucursal == true || PaginaInicio.Pagina == pagina)
                                {
                                    HttpContext.Current.Response.Redirect("../InicioSesion.aspx");
                                }
                                else
                                {
                                    HttpContext.Current.Response.Redirect(PaginaInicio.Pagina);
                                }
                            }
                        }
                    }
                }

                //Cerrar Conexion
                ConexionBaseDatos.CerrarBaseDatos();
            }
        }
    }
}