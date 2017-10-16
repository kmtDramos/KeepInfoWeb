using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CEstatusFacturaEncabezado
{
    //Constructores

    //Metodos Especiales
    public string ObtenerEstatusFactura(int pIdFacturaEncabezado, CConexion pConexion)
    {
        CFacturaEncabezado Factura = new CFacturaEncabezado();
        Factura.LlenaObjeto(pIdFacturaEncabezado, pConexion);
        CEstatusFacturaEncabezado Estatus = new CEstatusFacturaEncabezado();
        Estatus.LlenaObjeto(Factura.IdEstatusFacturaEncabezado, pConexion);
        return Estatus.EstatusFacturaEncabezado;
    }
}