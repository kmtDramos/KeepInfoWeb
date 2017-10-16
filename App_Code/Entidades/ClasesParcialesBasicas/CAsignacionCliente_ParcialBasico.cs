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

public partial class CAsignacionCliente
{
    //Propiedades Privadas
    private int idAsignacionCliente;
    private int idUsuario;
    private int idCliente;
    private int idUsuarioAlta;
    private int idUsuarioModifico;
    private DateTime fechaAlta;
    private DateTime fechaModifico;
    private bool existe;
    private bool baja;

    //Propiedades
    public int IdAsignacionCliente
    {
        get { return idAsignacionCliente; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idAsignacionCliente = value;
        }
    }

    public int IdUsuario
    {
        get { return idUsuario; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUsuario = value;
        }
    }

    public int IdCliente
    {
        get { return idCliente; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idCliente = value;
        }
    }

    public int IdUsuarioAlta
    {
        get { return idUsuarioAlta; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUsuarioAlta = value;
        }
    }

    public int IdUsuarioModifico
    {
        get { return idUsuarioModifico; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUsuarioModifico = value;
        }
    }

    public DateTime FechaAlta
    {
        get { return fechaAlta; }
        set { fechaAlta = value; }
    }

    public DateTime FechaModifico
    {
        get { return fechaModifico; }
        set { fechaModifico = value; }
    }

    public bool Existe
    {
        get { return existe; }
        set { existe = value; }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CAsignacionCliente()
    {
        idAsignacionCliente = 0;
        idUsuario = 0;
        idCliente = 0;
        idUsuarioAlta = 0;
        idUsuarioModifico = 0;
        fechaAlta = new DateTime(1, 1, 1);
        fechaModifico = new DateTime(1, 1, 1);
        existe = false;
        baja = false;
    }

    public CAsignacionCliente(int pIdAsignacionCliente)
    {
        idAsignacionCliente = pIdAsignacionCliente;
        idUsuario = 0;
        idCliente = 0;
        idUsuarioAlta = 0;
        idUsuarioModifico = 0;
        fechaAlta = new DateTime(1, 1, 1);
        fechaModifico = new DateTime(1, 1, 1);
        existe = false;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsignacionCliente_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAsignacionCliente>(typeof(CAsignacionCliente), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsignacionCliente_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CAsignacionCliente>(typeof(CAsignacionCliente), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsignacionCliente_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdAsignacionCliente", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAsignacionCliente>(typeof(CAsignacionCliente), pConexion);
        foreach (CAsignacionCliente O in Obten.ListaRegistros)
        {
            idAsignacionCliente = O.IdAsignacionCliente;
            idUsuario = O.IdUsuario;
            idCliente = O.IdCliente;
            idUsuarioAlta = O.IdUsuarioAlta;
            idUsuarioModifico = O.IdUsuarioModifico;
            fechaAlta = O.FechaAlta;
            fechaModifico = O.FechaModifico;
            existe = O.Existe;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsignacionCliente_ConsultarFiltros";
        foreach (KeyValuePair<string, object> parametro in pParametros)
        {
            if (parametro.Key == "Opcion")
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@" + parametro.Key, parametro.Value);
            }
            else
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@p" + parametro.Key, parametro.Value);
            }
        }
        Obten.Llena<CAsignacionCliente>(typeof(CAsignacionCliente), pConexion);
        foreach (CAsignacionCliente O in Obten.ListaRegistros)
        {
            idAsignacionCliente = O.IdAsignacionCliente;
            idUsuario = O.IdUsuario;
            idCliente = O.IdCliente;
            idUsuarioAlta = O.IdUsuarioAlta;
            idUsuarioModifico = O.IdUsuarioModifico;
            fechaAlta = O.FechaAlta;
            fechaModifico = O.FechaModifico;
            existe = O.Existe;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsignacionCliente_ConsultarFiltros";
        foreach (KeyValuePair<string, object> parametro in pParametros)
        {
            if (parametro.Key == "Opcion")
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@" + parametro.Key, parametro.Value);
            }
            else
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@p" + parametro.Key, parametro.Value);
            }
        }
        Obten.Llena<CAsignacionCliente>(typeof(CAsignacionCliente), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_AsignacionCliente_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAsignacionCliente", 0);
        Agregar.StoredProcedure.Parameters["@pIdAsignacionCliente"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
        if (fechaAlta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        if (fechaModifico.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaModifico", fechaModifico);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pExiste", existe);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idAsignacionCliente = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdAsignacionCliente"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_AsignacionCliente_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdAsignacionCliente", idAsignacionCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
        if (fechaAlta.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        if (fechaModifico.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaModifico", fechaModifico);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pExiste", existe);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_AsignacionCliente_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdAsignacionCliente", idAsignacionCliente);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}