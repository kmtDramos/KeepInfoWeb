using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net;
using arquitecturaNet;
using System.IO;
using System.Diagnostics;
using System.Net.Mail;

public partial class Paginas_AgregarOportunidad : System.Web.UI.Page
{

    public static string mesActual = "";

    protected void Page_Load(object sender, EventArgs e)
	{
        string nombre = Convert.ToString(HttpContext.Current.Request["camponombre"]);
        string telefono = Convert.ToString(HttpContext.Current.Request["campotel"]);
        string celular = Convert.ToString(HttpContext.Current.Request["campocel"]);
        string correo = Convert.ToString(HttpContext.Current.Request["campocorreo"]);
        string empresa = Convert.ToString(HttpContext.Current.Request["campoempresa"]);
        string puesto = Convert.ToString(HttpContext.Current.Request["campopuesto"]);
        string direccion = Convert.ToString(HttpContext.Current.Request["campodireccion"]);
        string comentario = Convert.ToString(HttpContext.Current.Request["campocomentario"]);

        bool valid = false;

        try {
            valid = Convert.ToBoolean(Contacto(nombre, telefono, celular, correo, empresa, puesto, direccion, comentario));
        }
        catch (Exception e) {
           
        }

		string CuerpoCorreo = CUtilerias.TextoArchivo("Contacto.html");

		CUtilerias.EnviarCorreo("pruebas@pruebas.com", correo, "Graciás por su solicitud", bodyHTMLCliente());
		CUtilerias.EnviarCorreo("pruebas@pruebas.com", "dramos@grupoasercom.com", "Nueva solicitud de contacto", bodyHTMLContacto());
		
	}

    [WebMethod]
    public static bool Contacto(string nombre, string telefono, string celular, string correo, string empresa, string puesto, string direccion, string comentario)
    {
        COrganizacion organizacion = new COrganizacion();
        CCliente cliente = new CCliente();
        CClienteSucursal clienteSucursal = new CClienteSucursal();
        COportunidad oportunidad = new COportunidad();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                organizacion.NombreComercial = empresa;
                organizacion.FechaAlta = DateTime.Now;
                organizacion.FechaModificacion = DateTime.Now;
                organizacion.RFC = "XAXX010101001";
                organizacion.Notas = "";
                organizacion.Dominio = "";
                organizacion.IdTipoIndustria = 17;
                organizacion.IdUsuarioAlta = 202;
                organizacion.IdUsuarioModifico = 0;
                organizacion.Baja = false;
                organizacion.IdGrupoEmpresarial = 0;
                organizacion.RazonSocial = "";
                organizacion.IdEmpresaAlta = 1;
                organizacion.IdSegmentoMercado = 0;
                organizacion.Agregar(pConexion);

                cliente.FechaAlta = DateTime.Now;
                cliente.FechaModificacion = DateTime.Now;
                cliente.LimiteDeCredito = "$0.0";
                cliente.Correo = correo;
                cliente.IdUsuarioAlta = 202;
                cliente.IdUsuarioModifico = 0;
                cliente.IdOrganizacion = organizacion.IdOrganizacion;
                cliente.IdFormaContacto = 2;
                cliente.IdCondicionPago = 0;
                cliente.Baja = false;
                cliente.IVAActual = 16;
                cliente.IdTipoGarantia = 0;
                cliente.IdUsuarioAgente = 26;
                cliente.CuentaContable = "";
                cliente.CuentaContableDolares = "";
                cliente.IdCampana = 1;
                cliente.EsCliente = true;
                cliente.Agregar(pConexion);

                clienteSucursal.FechaAlta = DateTime.Now;
                clienteSucursal.FechaUltimaModificacion = DateTime.Now;
                clienteSucursal.IdCliente = cliente.IdCliente;
                clienteSucursal.IdSucursal = 1;
                clienteSucursal.IdUsuarioAlta = 202;
                clienteSucursal.IdUsuarioModifico = 0;
                clienteSucursal.Baja = false;
                clienteSucursal.Agregar(pConexion);

				
                

              
            }
        });

        return true;
    }
    // BODY HTML - AUTORESPUESTA
    public static string bodyHTMLCliente()
    {
        string bodyHTML =
            "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" bgcolor=\"#ffffff\" style=\"padding:0;\"> " +
    "<tbody> " +
        "<tr> " +
            "<td align=\"center\" valign=\"top\" style=\"background-color:#ffffff\"> " +
                "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> " +
                    "<tbody> " +
                        "<tr> " +
                            "<td style=\"display:none\"> " +
                                "<div style=\"display:none;font-size:0px;max-height:0px;line-height:0px;padding:0\"> Mensaje recibido</div> " +
                            "</td> " +
                        "</tr> " +

                        "<tr> " +
                            "<td> " +
                                "<table bgcolor=\"#ffffff\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:100%;max-width:560px\"> " +
                                    "<tbody> " +
                                        "<tr> " +
                                            "<td style=\"padding:12px 0 12px 0\" bgcolor=\"#ffffff\"> " +
                                                "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:540px\"> " +
                                                    "<tbody> " +
                                                        "<tr> " +
                                                            "<td> " +
                                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> " +
                                                                    "<tbody> " +
                                                                        "<tr> " +
                                                                            "<td> " +
                                                                                "<table width=\"540\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" bgcolor=\"#ffffff\" align=\"center\" style=\"width:100%;max-width:540px\"> " +
                                                                                    "<tbody> " +
                                                                                        "<tr> " +
                                                                                            "<td valign=\"middle\" align=\"left\" bgcolor=\"#ffffff\"> " +
                                                                                                "<a href=\"https://grupoasercom.com/\" target=\"_blank\" style=\" display: inline-block; border: 0; text-decoration: none;\"> " +
                                                                                                    "<img src=\"https://www.grupoasercom.com/wp-content/uploads/2018/01/logo.png\" alt=\"Grupo Asercom\" width=\"113\" height=\"74\" border=\"0\" style=\"display:block\"> " +
                                                                                                "</a> " +
                                                                                            "</td> " +
                                                                                            "<td valign=\"middle\" align=\"right\" bgcolor=\"#ffffff\"> " +
                                                                                                "<span style=\"color:#FE4F00;font-family:arial,sans-serif;font-size:22px;line-height:30px;font-weight:normal;padding:0 0 18px 0;text-align:l\"> Mensaje recibido</span> " +
                                                                                            "</td> " +
                                                                                        "</tr> " +
                                                                                    "</tbody> " +
                                                                                "</table> " +
                                                                            "</td> " +
                                                                        "</tr> " +
                                                                        "<tr> " +
                                                                            "<td align=\"left\"> " +
                                                                                "<br> <span style=\"color:#333333;font-family:arial,sans-serif;font-size:14px;line-height:24px;text-align:left;\"> Estimado(a): <strong  style=\"color:#333333;font-family:arial,sans-serif;font-size:14px;line-height:24px;text-align:left;font-weight:bold\"> Juan Perez</strong> </span> " +
                                                                                "<br><br> " +
                                                                                "<span style=\"color:#333333;font-family:arial,sans-serif;font-size:14px;line-height:24px;text-align:left;\"> Por medio del presente queremos darte las gracias por visitar el sitio de Grupo Asercom, en breve se estará reportando con usted un miembro de nuestro equipo.</ span > " +
                                                                            "</td> " +
                                                                        "</tr> " +
                                                                        "<tr> " +
                                                                            "<td align=\"left\"> " +
                                                                                "<br> <span style=\"color:#333333;font-family:arial,sans-serif;font-size:14px;line-height:23px;text-align:left;padding-top:18px\"> Atentamente:</span> " +
                                                                                "<br> <strong style=\"color:#333333;font-family:arial,sans-serif;font-size:14px;line-height:23px;text-align:left;padding-top:18px;font-weight:bold\"> Grupo Asercom</strong> " +
                                                                            "</td> " +
                                                                        "</tr> " +
                                                                        "<tr> " +
                                                                            "<td style=\"border-bottom:1px solid #D1D1D1;\"> " +
                                                                                "<br> " +
                                                                            "</td> " +
                                                                        "</tr> " +
                                                                        "<tr> " +
                                                                            "<td> " +
                                                                                "<table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"540\"> " +
                                                                                    "<tbody> " +
                                                                                        "<tr> " +
                                                                                            "<td align=\"center\" style=\"padding-top:10px;\"> " +
                                                                                                "<span style=\"font-family:arial,sans-serif;font-size:12px;line-height:20px;color:#B8B8B8;\"> Copyright &copy;2018 Grupo Asercom. Todos los derechos reservados.</span> " +
                                                                                            "</td> " +
                                                                                        "</tr> " +
                                                                                        "<tr> " +
                                                                                            "<td bgcolor=\"#ffffff\"> " +
                                                                                                "<br> " +
                                                                                            "</td> " +
                                                                                        "</tr> " +
                                                                                    "</tbody> " +
                                                                                "</table> " +
                                                                            "</td> " +
                                                                        "</tr> " +
                                                                    "</tbody> " +
                                                                "</table> " +
                                                            "</td> " +
                                                        "</tr> " +
                                                    "</tbody> " +
                                                "</table> " +
                                            "</td> " +
                                        "</tr> " +
                                    "</tbody> " +
                                "</table> " +
                            "</td> " +
                        "</tr> " +
                    "</tbody> " +
                "</table> " +
            "</td> " +
        "</tr> " +
    "</tbody> " +
"</table> ";

        return bodyHTML;
    }

    // BODY HTML - CONTACTO
    public static string bodyHTMLContacto(){

        string bodyHTML =
            "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" bgcolor=\"#ffffff\" style=\"padding:0;\"> " +
    "<tbody> " +
        "<tr> " +
            "<td align=\"center\" valign=\"top\" style=\"background-color:#ffffff\"> " +
                "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> " +
                    "<tbody> " +
                        "<tr> " +
                            "<td style=\"display:none\"> " +
                                "<div style=\"display:none;font-size:0px;max-height:0px;line-height:0px;padding:0\"> Nuevo contacto</div> " +
                            "</td> " +
                        "</tr> " +

                        "<tr> " +
                            "<td> " +
                                "<table bgcolor=\"#ffffff\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:100%;max-width:560px\"> " +
                                    "<tbody> " +
                                        "<tr> " +
                                            "<td style=\"padding:12px 0 12px 0\" bgcolor=\"#ffffff\"> " +
                                                "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:540px\"> " +
                                                    "<tbody> " +
                                                        "<tr> " +
                                                            "<td> " +
                                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> " +
                                                                    "<tbody> " +
                                                                        "<tr> " +
                                                                            "<td> " +
                                                                                "<table width=\"540\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" bgcolor=\"#ffffff\" align=\"center\" style=\"width:100%;max-width:540px\"> " +
                                                                                    "<tbody> " +
                                                                                        "<tr> " +
                                                                                            "<td valign=\"middle\" align=\"left\" bgcolor=\"#ffffff\"> " +
                                                                                                "<a href=\"https://grupoasercom.com/\" target=\"_blank\" style=\" display: inline-block; border: 0; text-decoration: none;\"> " +
                                                                                                    "<img src=\"https://www.grupoasercom.com/wp-content/uploads/2018/01/logo.png\" alt=\"Grupo Asercom\" width=\"113\" height=\"74\" border=\"0\" style=\"display:block\"> " +
                                                                                                "</a> " +
                                                                                            "</td> " +
                                                                                            "<td valign=\"middle\" align=\"right\" bgcolor=\"#ffffff\"> " +
                                                                                                "<span style=\"color:#FE4F00;font-family:arial,sans-serif;font-size:22px;line-height:30px;font-weight:normal;padding:0 0 18px 0;text-align:left\"> Nuevo Contacto</span> " +
                                                                                            "</td> " +
                                                                                        "</tr> " +
                                                                                    "</tbody> " +
                                                                                "</table> " +
                                                                            "</td> " +
                                                                        "</tr> " +
                                                                        "<tr> " +
                                                                            "<td align=\"left\"> " +
                                                                                "<br> <span style=\"color:#333333;font-family:arial,sans-serif;font-size:14px;line-height:24px;text-align:left;\">  Estimado(a): <strong  style=\"color:#333333;font-family:arial,sans-serif;font-size:14px;line-height:24px;text-align:left;font-weight:bold\">Grupo Asercom</strong> </span> " +
                                                                                  "<br> " +
                                                                                  "<span style=\"color:#333333;font-family:arial,sans-serif;font-size:14px;line-height:24px;text-align:left;\"> Un nuevo visitante ha dejado la siguiente informaci&oacute;n en el Sitio.</span> " +
                                                                                  "<br> " +
                                                                                  "<br> " +
                                                                              "</td> " +
                                                                          "</tr> " +
                                                                          "<tr> " +
                                                                              "<td> " +
                                                                                  "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%;border: 1px solid #D1D1D1;padding: 10px 14px;border-radius: 4px;\"> " +
                                                                                      "<tr> " +
                                                                                          "<th colspan=\"2\" align=\"left\"> <span style=\"color:#FE4F00;font-family:arial,sans-serif;font-size:16px;line-height:30px;text-align:left;font-weight: normal;\"> Datos del contacto</span> </th> " +
                                                                                      "</tr> " +
                                                                                      "<tr> " +
                                                                                          "<td width=\"120\" valign=\"top\"> <strong style=\"line-height: 28px;color:#333333;font-family:arial,sans-serif;font-size:13px;text-align:left;\"> Nombre</strong> " + "</td> " +
                                                                                          "<td> <span style=\"line-height: 28px;color:#000;font-family:arial,sans-serif;font-size:14px;text-align:left;\">[Nombre]</span> </td> " +
                                                                                      "</tr> " +
                                                                                      "<tr> " +
                                                                                          "<td valign=\"top\"> <strong style=\"line-height: 28px;color:#333333;font-family:arial,sans-serif;font-size:13px;text-align:left;\"> Tel&eacute;fono</strong> </td> " +
                                                                                          "<td> <span style=\"line-height: 28px;color:#000;font-family:arial,sans-serif;font-size:14px;text-align:left;\"> " + "<a href=\"tel:1234567890\" style=\"line-height: 28px;color:#FE4F00;font-family:arial,sans-serif;font-size:14px;text-align:left;text-decoration:underline;\">  1234567890</a></span> </td> " +
                                                                                        "</tr> " +
                                                                                        "<tr> " +
                                                                                            "<td valign=\"top\"> <strong style=\"line-height: 28px;color:#333333;font-family:arial,sans-serif;font-size:13px;text-align:left;\"> Celular</strong> </td> " +
                                                                                            "<td> <span style=\"line-height: 28px;color:#000;font-family:arial,sans-serif;font-size:14px;text-align:left;\"> " + "<a href=\"tel:1234567890\" style=\"line-height: 28px;color:#FE4F00;font-family:arial,sans-serif;font-size:14px;text-align:left;text-decoration:underline;\">  1234567890</a> </span> </td> " +
                                                                                          "</tr> " +
                                                                                          "<tr> " +
                                                                                              "<td valign=\"top\"> <strong style=\"line-height: 28px;color:#333333;font-family:arial,sans-serif;font-size:13px;text-align:left;\"> Correo </ strong > </ td > " +
                                                                                        "<td> <a href=\"mailto:lorem@lorem.com\" style=\"line-height: 28px;color:#FE4F00;font-family:arial,sans-serif;font-size:14px;text-align:left;text-decoration:underline;\"> lorem@lorem.com</a> </td> " +
                                                                                    "</tr> " +
                                                                                    "<tr> " +
                                                                                        "<td valign=\"top\"> <strong style=\"line-height: 28px;color:#333333;font-family:arial,sans-serif;font-size:13px;text-align:left;\"> Empresa</strong> </td> " +
                                                                                        "<td> <span style=\"line-height: 28px;color:#000;font-family:arial,sans-serif;font-size:14px;text-align:left;\"> Lorem</span> </td> " +
                                                                                    "</tr> " +
                                                                                    "<tr> " +
                                                                                        "<td valign=\"top\"> <strong style=\"line-height: 28px;color:#333333;font-family:arial,sans-serif;font-size:13px;text-align:left;\">  Puesto</strong> </td> " +
                                                                                         "<td> <span style=\"line-height: 28px;color:#000;font-family:arial,sans-serif;font-size:14px;text-align:left;\"> Lorem</span> </td> " +
                                                                                     "</tr> " +
                                                                                     "<tr> " +
                                                                                         "<td valign=\"top\"> <strong style=\"line-height: 28px;color:#333333;font-family:arial,sans-serif;font-size:13px;text-align:left;\">  Direcci&oacute;n</strong> </td> " +
                                                                                             "<td> <span style=\"line-height: 28px;color:#000;font-family:arial,sans-serif;font-size:14px;text-align:left;\"> Lorem</span> </td> " +
                                                                                         "</tr> " +
                                                                                         "<tr> " +
                                                                                             "<td valign=\"top\"> <strong style=\"line-height: 28px;color:#333333;font-family:arial,sans-serif;font-size:13px;text-align:left;\"> Comentarios</strong> </td> " +
                                                                                             "<td> <span style=\"line-height: 28px;color:#000;font-family:arial,sans-serif;font-size:14px;text-align:left;\"> Loremipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.</span> </td> " +
                                                                                         "</tr> " +
                                                                                     "</table> " +
                                                                                 "</td> " +
                                                                             "</tr> " +
                                                                             "<tr> " +
                                                                                 "<td align=\"left\"> " +
                                                                                     "<br>  <span style =\"color:#333333;font-family:arial,sans-serif;font-size:14px;line-height:23px;text-align:left;padding-top:18px;\"> Favor de dar seguimiento a este requerimiento.</span> " +
                                                                                        "<br> <strong style=\"color:#333333;font-family:arial,sans-serif;font-size:14px;line-height:23px;text-align:left;padding-top:18px;font-weight:bold\"> Grupo Asercom</strong> " +
                                                                                    "</td> " +
                                                                                "</tr> " +
                                                                                "<tr> " +
                                                                                    "<td style=\"border-bottom:1px solid #D1D1D1;\"> " +
                                                                                        "<br> " +
                                                                                    "</td> " +
                                                                                "</tr> " +
                                                                                "<tr> " +
                                                                                    "<td> " +
                                                                                        "<table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"540\"> " +
                                                                                            "<tbody> " +
                                                                                                "<tr> " +
                                                                                                    "<td align=\"center\" style=\"padding-top:10px;\"> " +
                                                                                                        "<span style=\"font-family:arial,sans-serif;font-size:12px;line-height:20px;color:#B8B8B8;\"> Copyright &copy;2018 Grupo Asercom. Todos los derechos reservados.</span> " +
                                                                                                    "</td> " +
                                                                                                "</tr> " +
                                                                                                "<tr> " +
                                                                                                    "<td bgcolor=\"#ffffff\"> " +
                                                                                                        "<br> " +
                                                                                                    "</td> " +
                                                                                                "</tr> " +
                                                                                            "</tbody> " +
                                                                                        "</table> " +
                                                                                    "</td> " +
                                                                                "</tr> " +
                                                                            "</tbody> " +
                                                                        "</table> " +
                                                                    "</td> " +
                                                                "</tr> " +
                                                            "</tbody> " +
                                                        "</table> " +
                                                    "</td> " +
                                                "</tr> " +
                                            "</tbody> " +
                                        "</table> " +
                                    "</td> " +
                                "</tr> " +
                            "</tbody> " +
                        "</table> " +
                    "</td> " +
                "</tr> " +
            "</tbody> " +
        "</table> ";

        return bodyHTML;
    }
}