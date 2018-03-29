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
        string nombre = Convert.ToString(HttpContext.Current.Request["nombre"]);
        string telefono = Convert.ToString(HttpContext.Current.Request["telefono"]);
        string celular = Convert.ToString(HttpContext.Current.Request["celular"]);
        string correo = Convert.ToString(HttpContext.Current.Request["correo"]);
        string empresa = Convert.ToString(HttpContext.Current.Request["empresa"]);
        string puesto = Convert.ToString(HttpContext.Current.Request["puesto"]);
        string direccion = Convert.ToString(HttpContext.Current.Request["direccion"]);
        string comentario = Convert.ToString(HttpContext.Current.Request["comentario"]);
        string idDivision = Convert.ToString(HttpContext.Current.Request["servicio"]);
        /*
        string[] division = {
            "",
            "Infraestructura y Comunicaciones",
            "Energía",
            "Cyber Seguridad",
            "Protección y Proyectos Especiales",
            "Administrados de Impresión",
            "Servicios y Soporte TI" };*/
        string msgToCliente = "";
        string msgToAdmin = "";
        
        //string div = division[idDivision];
        
        bool valid = false;

        try {
            valid = Convert.ToBoolean(Contacto(nombre, telefono, celular, correo, empresa, puesto, direccion, comentario, idDivision));
            if (valid)
            {
                msgToCliente = CUtilerias.TextoArchivo(@"C:\inetpub\wwwroot\KeepInfoWeb\Templates\tmplAutorespuesta.html");
                msgToCliente = msgToCliente.Replace("[Nombre]", nombre);

                msgToAdmin = CUtilerias.TextoArchivo(@"C:\inetpub\wwwroot\KeepInfoWeb\Templates\tmplContacto.html");
                msgToAdmin = msgToAdmin.Replace("[Nombre]", nombre);
                msgToAdmin = msgToAdmin.Replace("[Telefono]", telefono);
                msgToAdmin = msgToAdmin.Replace("[Celular]", celular);
                msgToAdmin = msgToAdmin.Replace("[Correo]", correo);
                msgToAdmin = msgToAdmin.Replace("[Empresa]", empresa);
                msgToAdmin = msgToAdmin.Replace("[Puesto]", puesto);
                msgToAdmin = msgToAdmin.Replace("[Direccion]", direccion);
                msgToAdmin = msgToAdmin.Replace("[Comentarios]", comentario);
                //msgToAdmin = msgToAdmin.Replace("[Division]", div);

                // from, to, subject, msg
                CUtilerias.EnviarCorreo("asercom@grupoasercom.com", correo, "Gracias por visitar nuestro Sitio", msgToCliente);
                CUtilerias.EnviarCorreo("asercom@grupoasercom.com", "fespino@grupoasercom.com,dramos@grupoasercom.com,aabril@keepmoving.com.mx,ahernandez@grupoasercom.com", "Grupo Asercom, un nuevo visitante a dejado información en el Sitio", msgToAdmin);

                Response.Redirect("https://www.grupoasercom.com/gracias/");
            }
        }
        catch (Exception m) {
            string error = m.Message;
        }
        
    }

    [WebMethod]
    public static bool Contacto(string nombre, string telefono, string celular, string correo, string empresa, string puesto, string direccion, string comentario, string idDivision)
    {
        COrganizacion organizacion = new COrganizacion();
        CCliente cliente = new CCliente();
        CClienteSucursal clienteSucursal = new CClienteSucursal();
        COportunidad oportunidad = new COportunidad();
        bool valid = false;
        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                try
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

                    int division = 0;
                    if (idDivision == "") {
                        idDivision = "2";
                    }
                    switch (idDivision) {
                        case "1":
                            //infraestructura y comunicaciones
                            division = 7;//7 23
                            break;
                        case "2":
                            //energia
                            division = 5;
                            break;
                        case "3":
                            //Cyber Seguridad
                            division = 24;
                            break;
                        case "4":
                            //Proteccion y proyectos especiales
                            division = 4;//4 11
                            break;
                        case "5":
                            //Servicios Administrados de Impresión
                            division = 25;
                            break;
                        case "6":
                            //Servicios y Soporte TI
                            division = 21;
                            break;
                    }

                    oportunidad.Oportunidad = "Campaña de Internet";
                    oportunidad.FechaCreacion = DateTime.Now;
                    oportunidad.IdUsuarioCreacion = 202;
                    oportunidad.IdNivelInteresOportunidad = 2;
                    oportunidad.Baja = false;
                    oportunidad.Monto = 1;
                    oportunidad.IdCliente = cliente.IdCliente;
                    oportunidad.IdSucursal = 1;
                    oportunidad.Cotizaciones = 0;
                    oportunidad.Pedidos = 0;
                    oportunidad.Proyectos = 0;
                    oportunidad.Facturas = 0;
                    oportunidad.Neto = 0;
                    oportunidad.IdDivision = division;
                    oportunidad.IdCampana = 1;
                    oportunidad.Clasificacion = false;
                    oportunidad.Facturado = false;
                    oportunidad.Cerrado = false;
                    oportunidad.EsProyecto = false;
                    oportunidad.Costo = 0;
                    oportunidad.Autorizado = false;
                    oportunidad.Agregar(pConexion);
                    
                    valid = true;

                }catch(Exception e)
                {
                    valid = false;
                }
   
            }
        });

        return valid;
    }
}