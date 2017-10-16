using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class CUsuario
{
    //Contructores
    public SqlCommand StoredProcedure = new SqlCommand();

    public CUsuario(int pIdUsuario, string pNombre)
    {
        idUsuario = pIdUsuario;
        usuario = "";
        contrasena = "";
        idPerfil = 1;
        nombre = pNombre;
        apellidoPaterno = "";
        apellidoMaterno = "";
        fechaNacimiento = DateTime.Now;
        correo = "";
        fechaIngreso = DateTime.Now;
        baja = false;
    }

    //Metodos Especiales
    public void EditarDatos(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_Usuario_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Editar.StoredProcedure.Parameters.AddWithValue("@pUsuario", usuario);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", idPerfil);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNombre", nombre);
        Editar.StoredProcedure.Parameters.AddWithValue("@pApellidoPaterno", apellidoPaterno);
        Editar.StoredProcedure.Parameters.AddWithValue("@pApellidoMaterno", apellidoMaterno);
        Editar.StoredProcedure.Parameters.AddWithValue("@pFechaNacimiento", fechaNacimiento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
        Editar.StoredProcedure.Parameters.AddWithValue("@pFechaIngreso", fechaIngreso);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEsAgente", esAgente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pAlcance1", Alcance1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pAlcance2", Alcance2);
        Editar.StoredProcedure.Parameters.AddWithValue("@pMeta", Meta);
        Editar.StoredProcedure.Parameters.AddWithValue("@pClientesNuevos", ClientesNuevos);
        Editar.Update(pConexion);
    }

    public void EditarSucursalPredeterminada(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_Usuario_Editar_SucursalPredeterminada";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalPredeterminada", idSucursalPredeterminada);
        Editar.Update(pConexion);
    }

    public List<object> LlenaObjetosFiltroNombre(int pIdentificador, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_Usuario_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 21);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pNombre", pIdentificador);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdEstatus", 0);
        ObtenObjeto.Llena<CUsuario>(typeof(CUsuario), pConexion);
        return ObtenObjeto.ListaRegistros;
    }

    public void IniciarSesion(string pUsuario, string pContrasena, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_Usuario_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 6);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pUsuario", pUsuario);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pContrasena", pContrasena);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        ObtenObjeto.Llena<CUsuario>(typeof(CUsuario), pConexion);
        foreach (CUsuario CU in ObtenObjeto.ListaRegistros)
        {
            idUsuario = CU.IdUsuario;
            usuario = CU.Usuario;
            contrasena = CU.Contrasena;
            idPerfil = CU.IdPerfil;
            nombre = CU.Nombre;
            apellidoPaterno = CU.ApellidoPaterno;
            apellidoMaterno = CU.ApellidoMaterno;
            fechaNacimiento = CU.FechaNacimiento;
            correo = CU.Correo;
            fechaIngreso = CU.FechaIngreso;
            idSucursalPredeterminada = CU.IdSucursalPredeterminada;
            idSucursalActual = CU.IdSucursalActual;
            baja = CU.Baja;
        }
    }

    public bool ExisteUsuario(int pIdUsuario, string pUsuario, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_Usuario_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 7);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToString(pIdUsuario));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pUsuario", pUsuario);
        ObtenObjeto.Llena<CUsuario>(typeof(CUsuario), pConexion);
        foreach (CUsuario CU in ObtenObjeto.ListaRegistros)
        {
            Usuario = CU.Usuario;
        }
        if (Usuario == null || Usuario == "")
        { return false; }
        else
        { return true; }
    }

    public bool VerificaUsuarioAsignadoAlmacen(int pIdUsuario, int pIdExistenciaDistribuida, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_Usuario_ConsultaAsignadoAlmacen";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(pIdUsuario));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", Convert.ToInt32(0));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaDistribuida", Convert.ToInt32(pIdExistenciaDistribuida));

        ObtenObjeto.Llena<CUsuario>(typeof(CUsuario), pConexion);
        foreach (CUsuario CU in ObtenObjeto.ListaRegistros)
        {
            IdUsuario = CU.IdUsuario;
        }
        if (IdUsuario == null || IdUsuario == 0)
        { return false; }
        else
        { return true; }
    }

    public bool ExisteCorreo(int pIdUsuario, string pCorreo, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_Usuario_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 8);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToString(pIdUsuario));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pCorreo", pCorreo);
        ObtenObjeto.Llena<CUsuario>(typeof(CUsuario), pConexion);
        foreach (CUsuario CU in ObtenObjeto.ListaRegistros)
        {
            Correo = CU.Correo;
        }
        if (Correo == null || Correo == "")
        { return false; }
        else
        { return true; }
    }

    public static bool PermisoUsuarioSesion(string Permiso)
    {
        bool permiso = false;

        CConexion pConexion = new CConexion();
        pConexion.ConectarBaseDatosSqlServer();

        CUsuario UsuarioSesion = new CUsuario();
        UsuarioSesion.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pConexion);

        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_Usuario_Permiso";
        Select.StoredProcedure.Parameters.Add("Permiso", SqlDbType.VarChar, 255).Value = Permiso;
        Select.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = UsuarioSesion.IdUsuario;
        Select.Llena(pConexion);

        if (Select.Registros.Read())
        {
            if (Convert.ToInt32(Select.Registros["Permiso"]) == 1)
            {
                permiso = true;
            }
        }

        Select.CerrarConsulta();
        pConexion.CerrarBaseDatosSqlServer();

        return permiso;
    }

    public static bool PermisoUsuario(int IdUsuario, string Permiso, CConexion pConexion)
    {
        bool permiso = false;
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_Usuario_Permiso";
        Select.StoredProcedure.Parameters.Add("Permiso", SqlDbType.VarChar, 255).Value = Permiso;
        Select.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;
        Select.Llena(pConexion);

        if (Select.Registros.Read())
        {
            if (Convert.ToInt32(Select.Registros["Permiso"]) == 1)
            {
                permiso = true;
            }
        }

        Select.CerrarConsulta();
        return permiso;
    }

    public bool Permiso(string pPagina, CConexion pConexion)
    {
        bool permiso = true;
        CPaginaPermiso PaginaPermisos = new CPaginaPermiso();
        foreach (CPaginaPermiso PPagina in PaginaPermisos.PermisosPagina(pPagina, pConexion))
        {
            permiso = true;
            CPerfilPermiso PerfilPermisos = new CPerfilPermiso();
            foreach (CPerfilPermiso Permisos in PerfilPermisos.PermisosAsignados(IdPerfil, pConexion))
            {
                if (Permisos.IdOpcion == PPagina.IdOpcion)
                {
                    permiso = false;
                    break;
                }
            }
            if (permiso == true)
            { break; }
        }
        return permiso;
    }

    public bool PermisoPerfilSucursal(string pPagina, int pIdPerfil, CConexion pConexion)
    {
        bool permiso = true;
        CPaginaPermiso PaginaPermisos = new CPaginaPermiso();
        foreach (CPaginaPermiso PPagina in PaginaPermisos.PermisosPagina(pPagina, pConexion))
        {
            permiso = true;
            CPerfilPermiso PerfilPermisos = new CPerfilPermiso();
            foreach (CPerfilPermiso Permisos in PerfilPermisos.PermisosAsignados(pIdPerfil, pConexion))
            {
                if (Permisos.IdOpcion == PPagina.IdOpcion)
                {
                    permiso = false;
                    break;
                }
            }
            if (permiso == true)
            { break; }
        }
        return permiso;
    }

    public string TienePermisos(string[] pOpciones, CConexion pConexion)
    {
        //Obtiene el idUsuario que esta en sesion
        if (HttpContext.Current.Session["IdUsuario"].ToString() == "")
        { idUsuario = 0; }
        else
        { idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]); }

        bool permiso = false;
        string mensaje = "";
        LlenaObjeto(idUsuario, pConexion);

        if (idPerfil == 1)
        {
            permiso = true;
        }
        else
        {
            CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdUsuario", idUsuario);
            Parametros.Add("IdSucursal", idSucursalActual);
            SucursalAsignada.LlenaObjetoFiltros(Parametros, pConexion);

            List<object> ListaOpciones = COpcion.ObtienePermisosOpciones(SucursalAsignada.IdPerfil, pConexion);
            for (int i = 0; i <= pOpciones.Length - 1; i++)
            {
                permiso = false;
                foreach (COpcion Opcion in ListaOpciones)
                {
                    if (Opcion.Comando == pOpciones[i])
                    {
                        ListaOpciones.Remove(Opcion);
                        permiso = true;
                        break;
                    }
                }
                if (permiso == false)
                { break; }
            }
        }

        //Devuelve mensaje
        if (permiso == false)
        {
            mensaje = mensaje + "<p>Favor de completar los siguientes requisitos:</p>";
            mensaje = mensaje + "<span>*</span> No tiene los permisos suficientes, consultalo con el administrador.<br />";
        }
        return mensaje;
    }

    public string TienePermisos(string[] pOpciones, int pIdPerfil, CConexion pConexion)
    {
        //Obtiene el idUsuario que esta en sesion
        if (HttpContext.Current.Session["IdUsuario"].ToString() == "")
        { idUsuario = 0; }
        else
        { idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]); }

        bool permiso = false;
        string mensaje = "";
        LlenaObjeto(idUsuario, pConexion);

        List<object> ListaOpciones = COpcion.ObtienePermisosOpciones(pIdPerfil, pConexion);
        for (int i = 0; i <= pOpciones.Length - 1; i++)
        {
            permiso = false;
            foreach (COpcion Opcion in ListaOpciones)
            {
                if (Opcion.Comando == pOpciones[i])
                {
                    ListaOpciones.Remove(Opcion);
                    permiso = true;
                    break;
                }
            }
            if (permiso == false)
            { break; }
        }

        //Devuelve mensaje
        if (permiso == false)
        {
            mensaje = mensaje + "<p>Favor de completar los siguientes requisitos:</p>";
            mensaje = mensaje + "<span>*</span> No tiene los permisos suficientes, consultalo con el administrador.<br />";
        }
        return mensaje;
    }

    public void CambiarContrasena(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_Usuario_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Editar.StoredProcedure.Parameters.AddWithValue("@pContrasena", contrasena);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Editar.Update(pConexion);
    }

    public bool TieneSucursalAsignada(CConexion pConexion)
    {
        bool tieneSucursalAsignada = false;
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_Usuario_Consultar_TieneSucursalAsignada";
        Select.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            if (Convert.ToInt32(Select.Registros["NoSucursalesAsignadas"]) > 0)
            {
                tieneSucursalAsignada = true;
            }
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return tieneSucursalAsignada;
    }

    public int SucursalPredeterminada(CConexion pConexion)
    {
        int idSucursalPredeterminada = 0;
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_Usuario_Consultar_SucursalPredeterminada";
        Select.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            if (Convert.ToInt32(Select.Registros["IdSucursal"]) > 0)
            {
                idSucursalPredeterminada = Convert.ToInt32(Select.Registros["IdSucursal"]);
            }
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return idSucursalPredeterminada;
    }

    public static JArray ObtenerJsonUsuario(CConexion pConexion)
    {
        CUsuario Usuario = new CUsuario();
        JArray JUsuarios = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("Opcion", 14);
        foreach (CUsuario oUsuario in Usuario.LlenaObjetosFiltros_NombreCompleto(ParametrosTI, pConexion))
        {
            JObject JUsuario = new JObject();
            JUsuario.Add("Valor", oUsuario.IdUsuario);
            JUsuario.Add("Descripcion", oUsuario.Nombre);
            JUsuarios.Add(JUsuario);
        }
        return JUsuarios;
    }

    public static JArray ObtenerJsonUsuario(int pIdUsuario, CConexion pConexion)
    {
        CUsuario Usuario = new CUsuario();
        JArray JUsuarios = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CUsuario oUsuario in Usuario.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JUsuario = new JObject();
            JUsuario.Add("Valor", oUsuario.IdUsuario);
            JUsuario.Add("Descripcion", oUsuario.Usuario);
            if (oUsuario.IdUsuario == pIdUsuario)
            {
                JUsuario.Add(new JProperty("Selected", 1));
            }
            else
            {
                JUsuario.Add(new JProperty("Selected", 0));
            }
            JUsuarios.Add(JUsuario);
        }
        return JUsuarios;
    }

    public static JArray ObtenerJsonUsuarioNombre(int pIdUsuario, CConexion pConexion)
    {
        CUsuario Usuario = new CUsuario();
        JArray JUsuarios = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CUsuario oUsuario in Usuario.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JUsuario = new JObject();
            JUsuario.Add("Valor", oUsuario.IdUsuario);
            JUsuario.Add("Descripcion", oUsuario.Nombre + ' ' + oUsuario.ApellidoPaterno + ' ' + oUsuario.ApellidoMaterno);
            if (oUsuario.IdUsuario == pIdUsuario)
            {
                JUsuario.Add(new JProperty("Selected", 1));
            }
            else
            {
                JUsuario.Add(new JProperty("Selected", 0));
            }
            JUsuarios.Add(JUsuario);
        }
        return JUsuarios;
    }

    public List<object> LlenaObjetosFiltros_NombreCompleto(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_Usuario_Consulta";
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
        Obten.Llena<CUsuario>(typeof(CUsuario), pConexion);
        return Obten.ListaRegistros;
    }

    public static JArray ObtenerUsuariosPorSucursalAsignada(int pIdSucursal, CConexion pConexion)
    {
        int idUsuarioSesion = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        CSelectEspecifico UsuariosAsignados = new CSelectEspecifico();
        UsuariosAsignados.StoredProcedure.CommandText = "sp_SucursalAsignada_Consultar_ObtenerUsuariosPorSucursalAsignada";
        UsuariosAsignados.StoredProcedure.Parameters.AddWithValue("pIdSucursal", pIdSucursal);
        UsuariosAsignados.Llena(pConexion);

        JArray JAUsuariosAsignados = new JArray();
        while (UsuariosAsignados.Registros.Read())
        {
            JObject JUsuarioAsignado = new JObject();
            JUsuarioAsignado.Add("Valor", Convert.ToInt32(UsuariosAsignados.Registros["IdUsuario"]));
            JUsuarioAsignado.Add("Descripcion", Convert.ToString(UsuariosAsignados.Registros["NombreCompleto"]));
            if (Convert.ToInt32(UsuariosAsignados.Registros["IdUsuario"]) == idUsuarioSesion)
            {
                JUsuarioAsignado.Add(new JProperty("Selected", 1));
            }
            else
            {
                JUsuarioAsignado.Add(new JProperty("Selected", 0));
            }
            JAUsuariosAsignados.Add(JUsuarioAsignado);
        }
        UsuariosAsignados.Registros.Close();
        return JAUsuariosAsignados;
    }

    public static JArray ObtenerJsonUsuarioAgenteTodos(int pIdUsuario, CConexion pConexion)
    {
        CUsuario Usuario = new CUsuario();
        CUsuario UsuarioSesion = new CUsuario();
        UsuarioSesion.LlenaObjeto(pIdUsuario, pConexion);

        JArray JUsuarios = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("EsAgente", true);
        foreach (CUsuario oUsuario in Usuario.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
            Dictionary<string, object> ParametrosSucursalAsignada = new Dictionary<string, object>();
            ParametrosSucursalAsignada.Add("Baja", 0);
            ParametrosSucursalAsignada.Add("IdSucursal", UsuarioSesion.IdSucursalActual);
            ParametrosSucursalAsignada.Add("IdUsuario", oUsuario.IdUsuario);
            SucursalAsignada.LlenaObjetoFiltros(ParametrosSucursalAsignada, pConexion);

            if (SucursalAsignada.IdSucursalAsignada != 0 && SucursalAsignada.Baja == false)
            {
                JObject JUsuario = new JObject();
                JUsuario.Add("Valor", oUsuario.IdUsuario);
                JUsuario.Add("Usuario", oUsuario.Usuario);
                JUsuario.Add("Descripcion", oUsuario.Nombre + ' ' + oUsuario.ApellidoPaterno + ' ' + oUsuario.ApellidoMaterno);
                JUsuarios.Add(JUsuario);
            }
        }
        return JUsuarios;
    }

    public static JArray ObtenerJsonUsuarioAgente(int pIdUsuario, int pIdUsuarioAgente, CConexion pConexion)
    {
        CUsuario Usuario = new CUsuario();
        CUsuario UsuarioSesion = new CUsuario();
        UsuarioSesion.LlenaObjeto(pIdUsuario, pConexion);

        JArray JUsuarios = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("EsAgente", true);
        foreach (CUsuario oUsuario in Usuario.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
            Dictionary<string, object> ParametrosSucursalAsignada = new Dictionary<string, object>();
            ParametrosSucursalAsignada.Add("Baja", 0);
            ParametrosSucursalAsignada.Add("IdSucursal", UsuarioSesion.IdSucursalActual);
            ParametrosSucursalAsignada.Add("IdUsuario", oUsuario.IdUsuario);
            SucursalAsignada.LlenaObjetoFiltros(ParametrosSucursalAsignada, pConexion);

            if (SucursalAsignada.IdSucursalAsignada != 0 && SucursalAsignada.Baja == false)
            {
                JObject JUsuario = new JObject();
                JUsuario.Add("Valor", oUsuario.IdUsuario);
                JUsuario.Add("Usuario", oUsuario.Usuario);
                JUsuario.Add("Descripcion", oUsuario.Nombre + ' ' + oUsuario.ApellidoPaterno + ' ' + oUsuario.ApellidoMaterno);
                if (oUsuario.IdUsuario == pIdUsuarioAgente)
                {
                    JUsuario.Add(new JProperty("Selected", 1));
                }
                else
                {
                    JUsuario.Add(new JProperty("Selected", 0));
                }
                JUsuarios.Add(JUsuario);
            }
        }
        return JUsuarios;
    }

    public static JArray ObtenerJsonAgentes(int pIdEmpresa, CConexion pConexion) {
        JArray aAgentes = new JArray();

        CSelectEspecifico SelectEspecifico = new CSelectEspecifico();
        SelectEspecifico.StoredProcedure.CommandText = "sp_Usuario_AgentePorEmpresa";
        SelectEspecifico.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", pIdEmpresa);
        SelectEspecifico.Llena(pConexion);

        while (SelectEspecifico.Registros.Read()) {
            JObject oAgente = new JObject();
            oAgente.Add("Valor", Convert.ToInt32(SelectEspecifico.Registros["IdUsuario"]));
            oAgente.Add("Descripcion", Convert.ToString(SelectEspecifico.Registros["Agente"]));
            aAgentes.Add(oAgente);
        }
        SelectEspecifico.CerrarConsulta();
        return aAgentes;
    }

    public string ObtenerJsonAgente(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }

    public void ValidarSession() {
        int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (IdUsuario == 0)
        {
            HttpContext.Current.Response.Redirect("../InicioSesion.aspx");
        }
    }

}