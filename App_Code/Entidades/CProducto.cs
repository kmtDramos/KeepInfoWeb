using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;


public partial class CProducto
{
    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();

    //Constructores

    //Metodos Especiales
    public static JObject ObtenerJsonProducto(JObject pModelo, int pIdProducto, CConexion pConexion)
    {
        CProducto Producto = new CProducto();
        Producto.LlenaObjeto(pIdProducto, pConexion);
        pModelo.Add("IdProducto", Producto.IdProducto);
        pModelo.Add("Producto", Producto.Producto);
        pModelo.Add("Clave", Producto.Clave);
        pModelo.Add("NumeroParte", Producto.NumeroParte);
        pModelo.Add("Modelo", Producto.Modelo);
        pModelo.Add("CodigoBarra", Producto.CodigoBarra);
        pModelo.Add("Descripcion", Producto.Descripcion);
        pModelo.Add("Costo", Producto.Costo);
        pModelo.Add("MargenUtilidad", Producto.MargenUtilidad);
        pModelo.Add("IdTipoIVA", Producto.IdTipoIVA);
        pModelo.Add("Precio", Producto.Precio);
        pModelo.Add("ValorMedida", Producto.ValorMedida);
        pModelo.Add("Imagen", Producto.Imagen);
        pModelo.Add("ClaveProdServ", Producto.ClaveProdServ);

        if (Producto.IdTipoIVA == 1)
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pConexion);
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
            pModelo.Add("IVA", Sucursal.IVAActual);
            pModelo.Add("PrecioIVAIncluido", (Convert.ToDecimal(Producto.Precio)) + (((Convert.ToDecimal(Producto.Precio)) * (Convert.ToDecimal(Sucursal.IVAActual))) / 100));
        }
        else
        {
            pModelo.Add("IVA", 0);
            pModelo.Add("PrecioIVAIncluido", Producto.Precio);
        }

        CMarca Marca = new CMarca();
        Marca.LlenaObjeto(Producto.IdMarca, pConexion);
        pModelo.Add("IdMarca", Marca.IdMarca);
        pModelo.Add("Marca", Marca.Marca);

        CGrupo Grupo = new CGrupo();
        Grupo.LlenaObjeto(Producto.IdGrupo, pConexion);
        pModelo.Add("IdGrupo", Grupo.IdGrupo);
        pModelo.Add("Grupo", Grupo.Grupo);

        CCategoria Categoria = new CCategoria();
        Categoria.LlenaObjeto(Producto.IdCategoria, pConexion);
        pModelo.Add("IdCategoria", Categoria.IdCategoria);
        pModelo.Add("Categoria", Categoria.Categoria);

        CSubCategoria SubCategoria = new CSubCategoria();
        SubCategoria.LlenaObjeto(Producto.IdSubCategoria, pConexion);
        pModelo.Add("IdSubCategoria", SubCategoria.IdSubCategoria);
        pModelo.Add("SubCategoria", SubCategoria.SubCategoria);

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(Producto.IdTipoMoneda, pConexion);
        pModelo.Add("IdTipoMoneda", TipoMoneda.IdTipoMoneda);
        pModelo.Add("TipoMoneda", TipoMoneda.TipoMoneda);

        CTipoVenta TipoVenta = new CTipoVenta();
        TipoVenta.LlenaObjeto(Producto.IdTipoVenta, pConexion);
        pModelo.Add("IdTipoVenta", TipoVenta.IdTipoVenta);
        pModelo.Add("TipoVenta", TipoVenta.TipoVenta);

        CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
        UnidadCompraVenta.LlenaObjeto(Producto.IdUnidadCompraVenta, pConexion);
        pModelo.Add("IdUnidadCompraVenta", UnidadCompraVenta.IdUnidadCompraVenta);
        pModelo.Add("UnidadCompraVenta", UnidadCompraVenta.UnidadCompraVenta);

        CTipoIVA TipoIVA = new CTipoIVA();
        TipoIVA.LlenaObjeto(Producto.IdTipoIVA, pConexion);
        pModelo.Add("TipoIVA", TipoIVA.TipoIVA);

        return pModelo;
    }

    public string ObtenerJsonProducto(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }
}
