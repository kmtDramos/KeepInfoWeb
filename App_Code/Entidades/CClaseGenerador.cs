using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CClaseGenerador
{
    //Propiedades Privadas
    private List<string> arrClase;
    private int identacionClase;
    private List<string> arrClaseConfigurar;

    //Constructores

    //Metodos Especiales
    public void EjecutaGenerador(string pConsulta, CConexion pConexion)
    {
        CConsultaAccion Crear = new CConsultaAccion();
        Crear.StoredProcedure.CommandText = "spc_GeneradorTabla";
        Crear.StoredProcedure.Parameters.AddWithValue("@pConsulta", pConsulta);
        //if (pConsulta.Length <= 8000)
        //{
        //    Crear.StoredProcedure.Parameters.AddWithValue("@pConsulta", pConsulta);
        //}
        //else
        //{
        //    Crear.StoredProcedure.Parameters.AddWithValue("@pConsulta", pConsulta.Substring(0, 7999));
        //    Crear.StoredProcedure.Parameters.AddWithValue("@pConsulta2", pConsulta.Substring(7999, (pConsulta.Length) - 7999));
        //}
        Crear.Insert(pConexion);
    }

    public bool ValidaNombreClase(string pClase, CConexion pConexion)
    {
        CSelectEspecifico ObtenObjeto = new CSelectEspecifico();
        ObtenObjeto.StoredProcedure.CommandText = "sp_ClaseGenerador_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 3);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pClase", pClase);
        bool validacion = false;
        ObtenObjeto.Llena(pConexion);
        if (ObtenObjeto.Registros.Read())
        {
            if (Convert.ToInt32(ObtenObjeto.Registros["NoClases"]) > 0)
            {
                validacion = true;
            }
        }
        ObtenObjeto.CerrarConsulta();
        return validacion;
    }

    public bool ValidaNombreClase(string pClase, int pIdClaseGenerador, CConexion pConexion)
    {
        CSelectEspecifico ObtenObjeto = new CSelectEspecifico();
        ObtenObjeto.StoredProcedure.CommandText = "sp_ClaseGenerador_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 5);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pClase", pClase);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdClaseGenerador", pIdClaseGenerador);
        bool validacion = false;
        ObtenObjeto.Llena(pConexion);
        if (ObtenObjeto.Registros.Read())
        {
            if (Convert.ToInt32(ObtenObjeto.Registros["NoClases"]) > 0)
            {
                validacion = true;
            }
        }
        ObtenObjeto.CerrarConsulta();
        return validacion;
    }

    public string CrearTabla(CClaseGenerador pClaseGenerador, List<CClaseAtributo> pClaseAtributo, CConexion pConexion)
    {
        string crearTabla = "";
        crearTabla = crearTabla + "CREATE TABLE " + pClaseGenerador.Clase + " (\n";
        CClaseAtributo ClaseAtributo = new CClaseAtributo();
        ClaseAtributo.IdClaseGenerador = pClaseGenerador.IdClaseGenerador;

        crearTabla = crearTabla + "Id" + pClaseGenerador.Clase + " INT IDENTITY(1,1) NOT NULL,";
        foreach (CClaseAtributo OCA in pClaseAtributo)
        {
            switch (OCA.TipoAtributo)
            {
                case "I":
                    crearTabla = crearTabla + OCA.Atributo + " INT NULL,\n";
                    break;
                case "S":
                    crearTabla = crearTabla + OCA.Atributo + " VARCHAR(" + OCA.Longitud + ") COLLATE SQL_Latin1_General_CP1_CI_AS NULL,\n";
                    break;
                case "D":
                    crearTabla = crearTabla + OCA.Atributo + " DECIMAL(" + OCA.Longitud + "," + OCA.Decimales + ") NULL,\n";
                    break;
                case "DT":
                    crearTabla = crearTabla + OCA.Atributo + " DATETIME NULL,\n";
                    break;
                case "B":
                    crearTabla = crearTabla + OCA.Atributo + " BIT NULL,\n";
                    break;
            }
        }

        if (pClaseGenerador.ManejaBaja == true)
        {
            crearTabla = crearTabla + "Baja BIT NULL,\n";
        }

        crearTabla = crearTabla + "CONSTRAINT PK_" + pClaseGenerador.Clase + " PRIMARY KEY CLUSTERED (Id" + pClaseGenerador.Clase + " ASC) WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF)\n";
        crearTabla = crearTabla + ")";
        pClaseGenerador.EjecutaGenerador(crearTabla, pConexion);

		// Crear archivo de consulta de sql
		string nombreArchivo = pClaseGenerador.Clase + ".sql";
		//StreamWriter archivo = File.CreateText("c:\\\\inetpub\\" + nombreArchivo);
		//archivo.Write(crearTabla);

        return crearTabla;
    }

    public void EditarTabla(CClaseGenerador pClaseGenerador, List<CClaseAtributo> pClaseAtributo, string pClaseAnterior, CConexion pConexion)
    {
        bool seRespaldo = false;
        if (pClaseAnterior != pClaseGenerador.Clase)
        {
            CConsultaAccion EditarNombreTabla = new CConsultaAccion();
            EditarNombreTabla.StoredProcedure.CommandText = "SP_RENAME";
            EditarNombreTabla.StoredProcedure.Parameters.AddWithValue("@objname", pClaseAnterior);
            EditarNombreTabla.StoredProcedure.Parameters.AddWithValue("@newname", pClaseGenerador.Clase);
            EditarNombreTabla.Update(pConexion);
        }

        CClaseGenerador ClaseGenerador = new CClaseGenerador();
        ClaseGenerador.LlenaObjeto(pClaseGenerador.IdClaseGenerador, pConexion);

        if (ClaseGenerador.ManejaBaja == false && pClaseGenerador.ManejaBaja == true)
        {
            CConsultaAccion AgregarColumnaBaja = new CConsultaAccion();
            AgregarColumnaBaja.StoredProcedure.CommandText = "spc_AgregarColumnaTabla_Agregar";
            AgregarColumnaBaja.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            AgregarColumnaBaja.StoredProcedure.Parameters.AddWithValue("@pTabla", pClaseGenerador.Clase);
            AgregarColumnaBaja.StoredProcedure.Parameters.AddWithValue("@pColumna", "Baja");
            AgregarColumnaBaja.StoredProcedure.Parameters.AddWithValue("@pTipoDato", "BIT");
            AgregarColumnaBaja.Insert(pConexion);
        }
        else if (ClaseGenerador.ManejaBaja == true && pClaseGenerador.ManejaBaja == false)
        {
            CConsultaAccion EliminarColumnaBaja = new CConsultaAccion();
            EliminarColumnaBaja.StoredProcedure.CommandText = "spc_EliminarColumnaTabla_Eliminar";
            EliminarColumnaBaja.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            EliminarColumnaBaja.StoredProcedure.Parameters.AddWithValue("@pTabla", pClaseGenerador.Clase);
            EliminarColumnaBaja.StoredProcedure.Parameters.AddWithValue("@pColumna", "Baja");
            EliminarColumnaBaja.Delete(pConexion);
        }

        List<CClaseAtributo> AtributosActuales = new List<CClaseAtributo>();
        CClaseAtributo AtributoActual = new CClaseAtributo();
        foreach (CClaseAtributo OAtributo in AtributoActual.LlenaObjetos_FiltroIdClaseGenerador(pClaseGenerador.IdClaseGenerador, pConexion))
        {
            AtributosActuales.Add(OAtributo);
        }

        //Agrega o edita los campos de la tabla.
        foreach (CClaseAtributo OAtributo in pClaseAtributo)
        {
            if (OAtributo.IdClaseAtributo == 0)
            {
                CConsultaAccion ConsultaAccion = new CConsultaAccion();
                ConsultaAccion.StoredProcedure.CommandText = "spc_AgregarColumnaTabla_Agregar";
                ConsultaAccion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
                ConsultaAccion.StoredProcedure.Parameters.AddWithValue("@pTabla", pClaseGenerador.Clase);
                ConsultaAccion.StoredProcedure.Parameters.AddWithValue("@pColumna", OAtributo.Atributo);
                ConsultaAccion.StoredProcedure.Parameters.AddWithValue("@pTipoDato", ObtenerTipoDato(OAtributo));
                ConsultaAccion.Insert(pConexion);
            }
            else
            {
                foreach (CClaseAtributo OAtributoActual in AtributosActuales)
                {
                    if (OAtributo.IdClaseAtributo == OAtributoActual.IdClaseAtributo)
                    {
                        if (OAtributo.Atributo != OAtributoActual.Atributo)
                        {
                            CConsultaAccion EditarNombreTabla = new CConsultaAccion();
                            EditarNombreTabla.StoredProcedure.CommandText = "SP_RENAME";
                            EditarNombreTabla.StoredProcedure.Parameters.AddWithValue("@objname", pClaseGenerador.Clase + "." + OAtributoActual.Atributo);
                            EditarNombreTabla.StoredProcedure.Parameters.AddWithValue("@newname", OAtributo.Atributo);
                            EditarNombreTabla.Update(pConexion);
                        }

                        if (OAtributo.Longitud != OAtributoActual.Longitud || OAtributo.Decimales != OAtributoActual.Decimales)
                        {
                            CConsultaAccion EditarColumnaTabla = new CConsultaAccion();
                            EditarColumnaTabla.StoredProcedure.CommandText = "spc_EditarColumnaTabla_Editar";
                            EditarColumnaTabla.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
                            EditarColumnaTabla.StoredProcedure.Parameters.AddWithValue("@pTabla", pClaseGenerador.Clase);
                            EditarColumnaTabla.StoredProcedure.Parameters.AddWithValue("@pColumna", OAtributo.Atributo);
                            EditarColumnaTabla.StoredProcedure.Parameters.AddWithValue("@pTipoDato", ObtenerTipoDato(OAtributo));
                            EditarColumnaTabla.Update(pConexion);
                        }
                        break;
                    }
                }
            }
        }

        //Elimina los campos de la tabla.
        foreach (CClaseAtributo OAtributoActual in AtributosActuales)
        {
            bool eliminar = true;
            foreach (CClaseAtributo OAtributo in pClaseAtributo)
            {
                if (OAtributo.IdClaseAtributo == OAtributoActual.IdClaseAtributo)
                {
                    eliminar = false;
                    break;
                }
            }
            if (eliminar == true)
            {
                if (eliminar == true && seRespaldo == false)
                {
                    CConsultaAccion GenenrarRespaldo = new CConsultaAccion();
                    GenenrarRespaldo.StoredProcedure.CommandText = "spc_GenerarRespaldo_Agregar";
                    GenenrarRespaldo.StoredProcedure.Parameters.AddWithValue("@pNombreBaseDatos", "KeepRisingProject");
                    GenenrarRespaldo.Insert(pConexion);
                }

                CConsultaAccion EliminarColumnaTabla = new CConsultaAccion();
                EliminarColumnaTabla.StoredProcedure.CommandText = "spc_EliminarColumnaTabla_Eliminar";
                EliminarColumnaTabla.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
                EliminarColumnaTabla.StoredProcedure.Parameters.AddWithValue("@pTabla", pClaseGenerador.Clase);
                EliminarColumnaTabla.StoredProcedure.Parameters.AddWithValue("@pColumna", OAtributoActual.Atributo);
                EliminarColumnaTabla.Delete(pConexion);
            }
        }
    }

    public string CrearStoredProcedures(CClaseGenerador pClaseGenerador, List<CClaseAtributo> pAtributos, CConexion pConexion)
    {
        string spb_Consultar = "";
        string spb_ConsultarFiltros = "";
        string spb_Agregar = "";
        string spb_Editar = "";
        string spb_Eliminar = "";
        string parametros = "";
        string parametrosInserts = "";
        string llavePrimaria = "Id" + pClaseGenerador.Clase;
        string atributosSaltoLinea = "";
        string atributosSinSaltoLinea = "";
        string atributosParametros = "";
        int identacion = 0;

        //-----Tipo de Stored Procedures (CONSULTA,AGREGAR,EDITAR,ELIMINAR)
        spb_Consultar = spb_Consultar + "CREATE PROCEDURE spb_" + pClaseGenerador.Clase + "_Consultar" + Convert.ToChar(13);
        spb_ConsultarFiltros = spb_ConsultarFiltros + "CREATE PROCEDURE spb_" + pClaseGenerador.Clase + "_ConsultarFiltros" + Convert.ToChar(13);
        spb_Agregar = spb_Agregar + "CREATE PROCEDURE spb_" + pClaseGenerador.Clase + "_Agregar" + Convert.ToChar(13);
        spb_Editar = spb_Editar + "CREATE PROCEDURE spb_" + pClaseGenerador.Clase + "_Editar" + Convert.ToChar(13);
        spb_Eliminar = spb_Eliminar + "CREATE PROCEDURE spb_" + pClaseGenerador.Clase + "_Eliminar" + Convert.ToChar(13);

        //-----Parametros de la tabla
        identacion++;
        parametros = parametros + ObtenerIdentacion(identacion) + "@Opcion INT=NULL," + Convert.ToChar(13);
        parametros = parametros + ObtenerIdentacion(identacion) + "@pId" + pClaseGenerador.Clase + " INT=NULL," + Convert.ToChar(13);
        atributosSaltoLinea = atributosSaltoLinea + ObtenerIdentacion(identacion + 1) + "Id" + pClaseGenerador.Clase + "," + Convert.ToChar(13);
        foreach (CClaseAtributo OAtributo in pAtributos)
        {
            switch (OAtributo.TipoAtributo)
            {
                case "I":
                    parametros = parametros + ObtenerIdentacion(identacion) + "@p" + OAtributo.Atributo + " INT=NULL," + Convert.ToChar(13);
                    break;
                case "S":
                    parametros = parametros + ObtenerIdentacion(identacion) + "@p" + OAtributo.Atributo + " VARCHAR(" + OAtributo.Longitud + ")=NULL," + Convert.ToChar(13);
                    break;
                case "D":
                    parametros = parametros + ObtenerIdentacion(identacion) + "@p" + OAtributo.Atributo + " DECIMAL(" + OAtributo.Longitud + "," + OAtributo.Decimales + ")=NULL," + Convert.ToChar(13);

                    break;
                case "DT":
                    parametros = parametros + ObtenerIdentacion(identacion) + "@p" + OAtributo.Atributo + " DATETIME=NULL," + Convert.ToChar(13);
                    break;
                case "B":
                    parametros = parametros + ObtenerIdentacion(identacion) + "@p" + OAtributo.Atributo + " INT=NULL," + Convert.ToChar(13);
                    break;
            }
            identacion++;

            atributosSaltoLinea = atributosSaltoLinea + ObtenerIdentacion(identacion) + OAtributo.Atributo + "," + Convert.ToChar(13);
            atributosParametros = atributosParametros + ObtenerIdentacion(identacion) + OAtributo.Atributo + " = " + "@p" + OAtributo.Atributo + "," + Convert.ToChar(13);
            atributosSinSaltoLinea = atributosSinSaltoLinea + OAtributo.Atributo + ",";
            parametrosInserts = parametrosInserts + "@p" + OAtributo.Atributo + ",";
            identacion--;
        }

        if (pClaseGenerador.ManejaBaja == true)
        {
            parametros = parametros + ObtenerIdentacion(identacion) + "@pBaja BIT=NULL," + Convert.ToChar(13);
            atributosParametros = atributosParametros + ObtenerIdentacion(identacion + 1) + "Baja" + " = " + "@pBaja," + Convert.ToChar(13);
            atributosSinSaltoLinea = atributosSinSaltoLinea + "Baja,";
            atributosSaltoLinea = atributosSaltoLinea + ObtenerIdentacion(identacion + 1) + "Baja," + Convert.ToChar(13);
            parametrosInserts = parametrosInserts + "@pBaja" + ",";
        }

        parametros = parametros.Remove(parametros.Length - 2) + Convert.ToChar(13) + "AS" + Convert.ToChar(13) + Convert.ToChar(13);
        parametrosInserts = parametrosInserts.Remove(parametrosInserts.Length - 1);
        atributosParametros = atributosParametros.Remove(atributosParametros.Length - 2);
        atributosSaltoLinea = atributosSaltoLinea.Remove(atributosSaltoLinea.Length - 2);
        atributosSinSaltoLinea = atributosSinSaltoLinea.Remove(atributosSinSaltoLinea.Length - 1);

        string parametroObteniendoId = parametros.Replace("@pId" + pClaseGenerador.Clase + " INT=NULL", "@pId" + pClaseGenerador.Clase + " INT=NULL OUTPUT"); ;
        string paramtroSinOpcion = parametros.Replace("@Opcion INT=NULL,", "");
        spb_Consultar = spb_Consultar + parametros;
        spb_ConsultarFiltros = spb_ConsultarFiltros + parametros;
        spb_Agregar = spb_Agregar + parametroObteniendoId;
        spb_Editar = spb_Editar + parametros;
        spb_Eliminar = spb_Eliminar + parametros;

        //-----Consulta sin filtro
        spb_Consultar = spb_Consultar + "IF @Opcion=1 BEGIN" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "SELECT" + Convert.ToChar(13);
        spb_Consultar = spb_Consultar + atributosSaltoLinea + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "FROM" + Convert.ToChar(13);
        identacion++;
        spb_Consultar = spb_Consultar + ObtenerIdentacion(identacion) + pClaseGenerador.Clase + Convert.ToChar(13) + "END" + Convert.ToChar(13) + Convert.ToChar(13);
        identacion--;

        //-----Consulta con filtro
        spb_Consultar = spb_Consultar + "IF @Opcion=2 BEGIN" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "SELECT" + Convert.ToChar(13);
        spb_Consultar = spb_Consultar + atributosSaltoLinea + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "FROM" + Convert.ToChar(13);
        identacion++;
        spb_Consultar = spb_Consultar + ObtenerIdentacion(identacion) + pClaseGenerador.Clase + Convert.ToChar(13);
        identacion--;
        spb_Consultar = spb_Consultar + ObtenerIdentacion(identacion) + "WHERE" + Convert.ToChar(13);
        identacion++;
        spb_Consultar = spb_Consultar + ObtenerIdentacion(identacion) + llavePrimaria + "=@p" + llavePrimaria + Convert.ToChar(13) + "END" + Convert.ToChar(13) + Convert.ToChar(13);
        identacion--;

        if (pClaseGenerador.ManejaBaja)
        {
            spb_Consultar = spb_Consultar + "IF @Opcion=3 BEGIN" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "SELECT" + Convert.ToChar(13);
            spb_Consultar = spb_Consultar + atributosSaltoLinea + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "FROM" + Convert.ToChar(13);
            identacion++;
            spb_Consultar = spb_Consultar + ObtenerIdentacion(identacion) + pClaseGenerador.Clase + Convert.ToChar(13);
            identacion--;
            spb_Consultar = spb_Consultar + ObtenerIdentacion(identacion) + "WHERE" + Convert.ToChar(13);
            identacion++;
            spb_Consultar = spb_Consultar + ObtenerIdentacion(identacion) + "Baja=@pBaja" + Convert.ToChar(13) + "END" + Convert.ToChar(13) + Convert.ToChar(13);
            identacion--;

            if (llavePrimaria.Length > 0)
            {
                spb_Consultar = spb_Consultar + "IF @Opcion=4 BEGIN" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "SELECT" + Convert.ToChar(13);
                spb_Consultar = spb_Consultar + atributosSaltoLinea + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "FROM" + Convert.ToChar(13);
                identacion++;
                spb_Consultar = spb_Consultar + ObtenerIdentacion(identacion) + pClaseGenerador.Clase + Convert.ToChar(13);
                identacion--;
                spb_Consultar = spb_Consultar + ObtenerIdentacion(identacion) + "WHERE" + Convert.ToChar(13);
                identacion++;
                spb_Consultar = spb_Consultar + ObtenerIdentacion(identacion) + llavePrimaria + "=@p" + llavePrimaria + " AND " + Convert.ToChar(13);
                spb_Consultar = spb_Consultar + ObtenerIdentacion(identacion) + "Baja=@pBaja" + Convert.ToChar(13) + "END";
                identacion--;
            }
        }


        //-----Crea sp_Consulta
        pClaseGenerador.EjecutaGenerador(spb_Consultar, pConexion);

        //-----Crea sp_Consulta Filtros
        spb_ConsultarFiltros = spb_ConsultarFiltros + "IF @Opcion=1 BEGIN" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "SELECT" + Convert.ToChar(13);
        spb_ConsultarFiltros = spb_ConsultarFiltros + atributosSaltoLinea + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "FROM" + Convert.ToChar(13);
        identacion++;
        spb_ConsultarFiltros = spb_ConsultarFiltros + ObtenerIdentacion(identacion) + pClaseGenerador.Clase + Convert.ToChar(13);
        identacion--;
        spb_ConsultarFiltros = spb_ConsultarFiltros + ObtenerIdentacion(identacion) + "WHERE" + Convert.ToChar(13);
        identacion++;

        spb_ConsultarFiltros = spb_ConsultarFiltros + ObtenerIdentacion(identacion) + "(@pId" + pClaseGenerador.Clase + " IS NULL OR Id" + pClaseGenerador.Clase + " = @pId" + pClaseGenerador.Clase + ")" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "AND ";
        string columnaOrden = "";
        foreach (CClaseAtributo OAtributo in pAtributos)
        {
            if (columnaOrden == "")
            {
                columnaOrden = OAtributo.Atributo;
            }
            spb_ConsultarFiltros = spb_ConsultarFiltros + "(@p" + OAtributo.Atributo + " IS NULL OR " + OAtributo.Atributo + " = @p" + OAtributo.Atributo + ")" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "AND ";
        }

        if (pClaseGenerador.ManejaBaja)
        {
            spb_ConsultarFiltros = spb_ConsultarFiltros + "(@pBaja IS NULL OR Baja = @pBaja)" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "AND ";
        }

        spb_ConsultarFiltros = spb_ConsultarFiltros.Substring(0, spb_ConsultarFiltros.Length - 6);
        identacion--;
        spb_ConsultarFiltros = spb_ConsultarFiltros + ObtenerIdentacion(identacion) + "ORDER BY " + Convert.ToChar(13);
        identacion++;
        spb_ConsultarFiltros = spb_ConsultarFiltros + ObtenerIdentacion(identacion) + columnaOrden + Convert.ToChar(13);
        identacion--;
        spb_ConsultarFiltros = spb_ConsultarFiltros + "END" + Convert.ToChar(13) + Convert.ToChar(13);
        spb_ConsultarFiltros = spb_ConsultarFiltros + "IF @Opcion=2 BEGIN" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "SELECT" + Convert.ToChar(13);
        spb_ConsultarFiltros = spb_ConsultarFiltros + atributosSaltoLinea + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "FROM" + Convert.ToChar(13);
        identacion++;
        spb_ConsultarFiltros = spb_ConsultarFiltros + ObtenerIdentacion(identacion) + pClaseGenerador.Clase + Convert.ToChar(13);
        identacion--;
        spb_ConsultarFiltros = spb_ConsultarFiltros + ObtenerIdentacion(identacion) + "WHERE" + Convert.ToChar(13);
        identacion++;

        spb_ConsultarFiltros = spb_ConsultarFiltros + ObtenerIdentacion(identacion) + "(@pId" + pClaseGenerador.Clase + " IS NULL OR Id" + pClaseGenerador.Clase + " = @pId" + pClaseGenerador.Clase + ")" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "AND ";
        foreach (CClaseAtributo OAtributo in pAtributos)
        {
            if (OAtributo.TipoAtributo == "S")
            {
                spb_ConsultarFiltros = spb_ConsultarFiltros + "(@p" + OAtributo.Atributo + " IS NULL OR " + OAtributo.Atributo + " LIKE '%'+@p" + OAtributo.Atributo + "+'%')" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "AND ";
            }
            else
            {
                spb_ConsultarFiltros = spb_ConsultarFiltros + "(@p" + OAtributo.Atributo + " IS NULL OR " + OAtributo.Atributo + " = @p" + OAtributo.Atributo + ")" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "AND ";
            }
        }

        if (pClaseGenerador.ManejaBaja)
        {
            spb_ConsultarFiltros = spb_ConsultarFiltros + "(@pBaja IS NULL OR Baja = @pBaja)" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "AND ";
        }

        spb_ConsultarFiltros = spb_ConsultarFiltros.Substring(0, spb_ConsultarFiltros.Length - 6);
        identacion--;
        spb_ConsultarFiltros = spb_ConsultarFiltros + ObtenerIdentacion(identacion) + "ORDER BY" + Convert.ToChar(13);
        identacion++;
        spb_ConsultarFiltros = spb_ConsultarFiltros + ObtenerIdentacion(identacion) + columnaOrden + Convert.ToChar(13);
        identacion--;
        spb_ConsultarFiltros = spb_ConsultarFiltros + "END";

        spb_ConsultarFiltros = spb_ConsultarFiltros.Replace("@Opcion INT=NULL,", "@Opcion INT=1,");

        //-----Crea sp_Consultar_filtros
        pClaseGenerador.EjecutaGenerador(spb_ConsultarFiltros, pConexion);

        //-----Agregar
        spb_Agregar = spb_Agregar + "IF @Opcion=1 BEGIN" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "INSERT INTO " + pClaseGenerador.Clase + Convert.ToChar(13);
        identacion++;
        spb_Agregar = spb_Agregar + ObtenerIdentacion(identacion) + "(" + atributosSinSaltoLinea + ")" + Convert.ToChar(13);
        identacion--;
        spb_Agregar = spb_Agregar + ObtenerIdentacion(identacion) + "VALUES" + Convert.ToChar(13);
        identacion++;
        spb_Agregar = spb_Agregar + ObtenerIdentacion(identacion) + "(" + parametrosInserts + ")" + Convert.ToChar(13) + Convert.ToChar(13);
        identacion--;
        spb_Agregar = spb_Agregar + ObtenerIdentacion(identacion) + "SET @pId" + pClaseGenerador.Clase + "= @@IDENTITY" + Convert.ToChar(13) + "END";
        identacion--;

        //-----Crea sp_Agregar
        pClaseGenerador.EjecutaGenerador(spb_Agregar, pConexion);

        //-----Editar
        spb_Editar = spb_Editar + "IF @Opcion=1 BEGIN" + Convert.ToChar(13) + ObtenerIdentacion(identacion) + "UPDATE" + Convert.ToChar(13);
        identacion++;
        spb_Editar = spb_Editar + ObtenerIdentacion(identacion) + pClaseGenerador.Clase + Convert.ToChar(13);
        identacion--;
        spb_Editar = spb_Editar + ObtenerIdentacion(identacion) + "SET" + Convert.ToChar(13);
        spb_Editar = spb_Editar + atributosParametros + Convert.ToChar(13);
        spb_Editar = spb_Editar + ObtenerIdentacion(identacion) + "WHERE" + Convert.ToChar(13);
        identacion++;
        spb_Editar = spb_Editar + ObtenerIdentacion(identacion) + llavePrimaria + " = @p" + llavePrimaria + Convert.ToChar(13);
        identacion--;
        spb_Editar = spb_Editar + "END";

        //-----Crea sp_Editar
        pClaseGenerador.EjecutaGenerador(spb_Editar, pConexion);

        //-----Eliminar
        spb_Eliminar = spb_Eliminar + "IF @Opcion=1 BEGIN" + Convert.ToChar(13);
        spb_Eliminar = spb_Eliminar + ObtenerIdentacion(identacion) + "DELETE FROM " + pClaseGenerador.Clase + " WHERE " + llavePrimaria + "=@p" + llavePrimaria + Convert.ToChar(13);
        spb_Eliminar = spb_Eliminar + "END" + Convert.ToChar(13) + Convert.ToChar(13);
        identacion--;

        //-----Eliminar Baja Logica
        if (pClaseGenerador.ManejaBaja)
        {
            spb_Eliminar = spb_Eliminar + "IF @Opcion=2 BEGIN" + Convert.ToChar(13);
            identacion++;
            spb_Eliminar = spb_Eliminar + ObtenerIdentacion(identacion) + "UPDATE " + Convert.ToChar(13);
            identacion++;
            spb_Eliminar = spb_Eliminar + ObtenerIdentacion(identacion) + pClaseGenerador.Clase + Convert.ToChar(13);
            identacion--;
            spb_Eliminar = spb_Eliminar + ObtenerIdentacion(identacion) + "SET" + Convert.ToChar(13);
            identacion++;
            spb_Eliminar = spb_Eliminar + ObtenerIdentacion(identacion) + "Baja = @pBaja" + Convert.ToChar(13);
            identacion--;
            spb_Eliminar = spb_Eliminar + ObtenerIdentacion(identacion) + "WHERE" + Convert.ToChar(13);
            identacion++;
            spb_Eliminar = spb_Eliminar + ObtenerIdentacion(identacion) + llavePrimaria + " = @p" + llavePrimaria + Convert.ToChar(13);
            identacion--;
            spb_Eliminar = spb_Eliminar + "END";
        }

        //-----Crea sp_Editar
        pClaseGenerador.EjecutaGenerador(spb_Eliminar, pConexion);

        return spb_Eliminar;
    }

    public void EliminarStoredProcedures(CClaseGenerador pClaseGenerador, CConexion pConexion)
    {
        CClaseGenerador ClaseGeneradorActual = new CClaseGenerador();
        ClaseGeneradorActual.LlenaObjeto(pClaseGenerador.IdClaseGenerador, pConexion);
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spc_EliminarStoredProcedures_Eliminar";

        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pStoredProcedures", "spb_" + ClaseGeneradorActual.Clase + "_Consultar");
        Eliminar.Delete(pConexion);

        Eliminar.StoredProcedure.Parameters.Clear();
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pStoredProcedures", "spb_" + ClaseGeneradorActual.Clase + "_ConsultarFiltros");
        Eliminar.Delete(pConexion);

        Eliminar.StoredProcedure.Parameters.Clear();
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pStoredProcedures", "spb_" + ClaseGeneradorActual.Clase + "_Agregar");
        Eliminar.Delete(pConexion);

        Eliminar.StoredProcedure.Parameters.Clear();
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pStoredProcedures", "spb_" + ClaseGeneradorActual.Clase + "_Editar");
        Eliminar.Delete(pConexion);

        Eliminar.StoredProcedure.Parameters.Clear();
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pStoredProcedures", "spb_" + ClaseGeneradorActual.Clase + "_Eliminar");
        Eliminar.Delete(pConexion);
    }

    public string ObtenerIdentacion(int pIndentacion)
    {
        string identacion = "";
        for (int i = 0; i < pIndentacion; i++)
        {
            identacion = identacion + Convert.ToChar(9);
        }
        return identacion;
    }

    public void CrearClase(CClaseGenerador pClaseGenerador, List<CClaseAtributo> pAtributos, CConexion pConexion)
    {
        string llavePrimaria = "";
        InicializarPagina();
        identacionClase = 0;
        llavePrimaria = "Id" + pClaseGenerador.Clase;
        L("using System;");
        L("using System.Collections.Generic;");
        L("using System.Linq;");
        L("using System.Text;");
        L("using System.Data;");
        L("using System.Data.SqlClient;");
        L("using System.Data.OleDb;");
        L("using System.IO;");
        L("using System.Web;");
        L("using Newtonsoft.Json;");
        L("using Newtonsoft.Json.Converters;");
        L("using Newtonsoft.Json.Linq;");
        L("");
        L("public partial class C" + pClaseGenerador.Clase);
        L("{");
        identacionClase++;
        L("//Propiedades Privadas");
        L("private int id" + pClaseGenerador.Clase + ";");
        foreach (CClaseAtributo OAtributo in pAtributos)
        {
            switch (OAtributo.TipoAtributo)
            {
                case "I":
                    L("private int " + FormatoAtributo(OAtributo.Atributo) + ";");
                    break;
                case "S":
                    L("private string " + FormatoAtributo(OAtributo.Atributo) + ";");
                    break;
                case "D":
                    L("private decimal " + FormatoAtributo(OAtributo.Atributo) + ";");
                    break;
                case "DT":
                    L("private DateTime " + FormatoAtributo(OAtributo.Atributo) + ";");
                    break;
                case "B":
                    L("private bool " + FormatoAtributo(OAtributo.Atributo) + ";");
                    break;
            }
        }
        if (pClaseGenerador.ManejaBaja == true)
        {
            L("private bool baja;");
        }
        L("");
        L("//Propiedades");
        L("public int Id" + pClaseGenerador.Clase);
        L("{");
        identacionClase++;
        L("get { return id" + pClaseGenerador.Clase + "; }");
        L("set");
        L("{");
        identacionClase++;
        L("id" + pClaseGenerador.Clase + " = value;");
        identacionClase--;
        L("}");
        identacionClase--;
        L("}");
        L("");
        foreach (CClaseAtributo OAtributo in pAtributos)
        {
            switch (OAtributo.TipoAtributo)
            {
                case "I":
                    L("public int " + FormatoGetSet(OAtributo.Atributo));
                    L("{");
                    identacionClase++;
                    L("get { return " + FormatoAtributo(OAtributo.Atributo) + "; }");
                    L("set");
                    L("{");
                    identacionClase++;
                    L("" + FormatoAtributo(OAtributo.Atributo) + " = value;");
                    identacionClase--;
                    L("}");
                    identacionClase--;
                    L("}");
                    L("");
                    break;
                case "S":
                    L("public string " + FormatoGetSet(OAtributo.Atributo));
                    L("{");
                    identacionClase++;
                    L("get { return " + FormatoAtributo(OAtributo.Atributo) + "; }");
                    L("set");
                    L("{");
                    identacionClase++;
                    L("" + FormatoAtributo(OAtributo.Atributo) + " = value;");
                    identacionClase--;
                    L("}");
                    identacionClase--;
                    L("}");
                    L("");
                    break;
                case "D":
                    L("public decimal " + FormatoGetSet(OAtributo.Atributo));
                    L("{");
                    identacionClase++;
                    L("get { return " + FormatoAtributo(OAtributo.Atributo) + "; }");
                    L("set");
                    L("{");
                    identacionClase++;
                    L("" + FormatoAtributo(OAtributo.Atributo) + " = value;");
                    identacionClase--;
                    L("}");
                    identacionClase--;
                    L("}");
                    L("");
                    break;
                case "DT":
                    L("public DateTime " + FormatoGetSet(OAtributo.Atributo));
                    L("{");
                    identacionClase++;
                    L("get { return " + FormatoAtributo(OAtributo.Atributo) + "; }");
                    L("set { " + FormatoAtributo(OAtributo.Atributo) + " = value; }");
                    identacionClase--;
                    L("}");
                    L("");
                    break;
                case "B":
                    L("public bool " + FormatoGetSet(OAtributo.Atributo));
                    L("{");
                    identacionClase++;
                    L("get { return " + FormatoAtributo(OAtributo.Atributo) + "; }");
                    L("set { " + FormatoAtributo(OAtributo.Atributo) + " = value; }");
                    identacionClase--;
                    L("}");
                    L("");
                    break;
            }
        }

        if (pClaseGenerador.ManejaBaja)
        {
            L("public bool Baja");
            L("{");
            identacionClase++;
            L("get { return baja; }");
            L("set { baja = value; }");
            identacionClase--;
            L("}");
            L("");
        }

        L("//Constructores");
        L("public C" + pClaseGenerador.Clase + "()");
        L("{");
        identacionClase++;
        L("id" + pClaseGenerador.Clase + " = 0;");
        foreach (CClaseAtributo OAtributo in pAtributos)
        {
            switch (OAtributo.TipoAtributo)
            {
                case "I":
                    L(FormatoAtributo(OAtributo.Atributo) + " = 0;");
                    break;
                case "S":
                    L(FormatoAtributo(OAtributo.Atributo) + " = \"\";");
                    break;
                case "D":
                    L(FormatoAtributo(OAtributo.Atributo) + " = 0;");
                    break;
                case "DT":
                    L(FormatoAtributo(OAtributo.Atributo) + " = new DateTime(1, 1, 1);");
                    break;
                case "B":
                    L(FormatoAtributo(OAtributo.Atributo) + " = false;");
                    break;
            }
        }

        if (pClaseGenerador.ManejaBaja == true)
        {
            L("baja = false;");
        }

        identacionClase--;
        L("}");
        L("");
        L("public C" + pClaseGenerador.Clase + "(int pId" + pClaseGenerador.Clase + ")");
        L("{");
        identacionClase++;
        L("id" + pClaseGenerador.Clase + " = pId" + pClaseGenerador.Clase + ";");
        foreach (CClaseAtributo OAtributo in pAtributos)
        {
            switch (OAtributo.TipoAtributo)
            {
                case "I":
                    L(FormatoAtributo(OAtributo.Atributo) + " = 0;");
                    break;
                case "S":
                    L(FormatoAtributo(OAtributo.Atributo) + " = \"\";");
                    break;
                case "D":
                    L(FormatoAtributo(OAtributo.Atributo) + " = 0;");
                    break;
                case "DT":
                    L(FormatoAtributo(OAtributo.Atributo) + " = new DateTime(1, 1, 1);");
                    break;
                case "B":
                    L(FormatoAtributo(OAtributo.Atributo) + " = false;");
                    break;
            }
        }

        if (pClaseGenerador.ManejaBaja == true)
        {
            L("baja = false;");
        }

        identacionClase--;
        L("}");
        L("");
        L("//Metodos Basicos");
        if (pClaseGenerador.ManejaBaja)
        {
            //Si trae baja
            L("public List<object> LlenaObjetos(CConexion pConexion)");
            L("{");
            identacionClase++;
            L("CSelect Obten = new CSelect();");
            L("Obten.StoredProcedure.CommandText = " + "\"spb_" + pClaseGenerador.Clase + "_Consultar\";");
            L("Obten.StoredProcedure.Parameters.AddWithValue(\"@Opcion\", 1);");
            L("Obten.StoredProcedure.Parameters.AddWithValue(\"@pBaja\", 0);");
            L("Obten.Llena<C" + pClaseGenerador.Clase + ">(typeof(C" + pClaseGenerador.Clase + "), pConexion);");
            L("return Obten.ListaRegistros;");
            identacionClase--;
            L("}");
            L("");
            L("public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)");
            L("{");
            identacionClase++;
            L("CSelect Obten = new CSelect();");
            L("Obten.StoredProcedure.CommandText = " + "\"spb_" + pClaseGenerador.Clase + "_Consultar\";");
            L("Obten.StoredProcedure.Parameters.AddWithValue(\"@Opcion\", 1);");
            L("Obten.StoredProcedure.Parameters.AddWithValue(\"@pBaja\", 0);");
            L("Obten.Columnas = new string[pColumnas.Length];");
            L("Obten.Columnas = pColumnas;");
            L("Obten.Llena<C" + pClaseGenerador.Clase + ">(typeof(C" + pClaseGenerador.Clase + "), pConexion);");
            L("return Obten.ListaRegistros;");
            identacionClase--;
            L("}");
            L("");
            L("public void LlenaObjeto(int pIdentificador, CConexion pConexion)");
            L("{");
            identacionClase++;
            L("CSelect Obten = new CSelect();");
            L("Obten.StoredProcedure.CommandText = " + "\"spb_" + pClaseGenerador.Clase + "_Consultar\";");
            L("Obten.StoredProcedure.Parameters.AddWithValue(\"@Opcion\", 2);");
            L("Obten.StoredProcedure.Parameters.AddWithValue(\"@pId" + pClaseGenerador.Clase + "\", pIdentificador);");
            L("Obten.StoredProcedure.Parameters.AddWithValue(\"@pBaja\", 0);");
            L("Obten.Llena<C" + pClaseGenerador.Clase + ">(typeof(C" + pClaseGenerador.Clase + "), pConexion);");
            L("foreach (C" + pClaseGenerador.Clase + " O in Obten.ListaRegistros)");
            L("{");
            identacionClase++;
            L(FormatoAtributo("id" + pClaseGenerador.Clase) + " = O." + FormatoGetSet("Id" + pClaseGenerador.Clase) + ";");

            foreach (CClaseAtributo OAtributo in pAtributos)
            {
                L(FormatoAtributo(OAtributo.Atributo) + " = O." + FormatoGetSet(OAtributo.Atributo) + ";");
            }

            if (pClaseGenerador.ManejaBaja)
            {
                L(FormatoAtributo("baja") + " = O." + FormatoGetSet("Baja") + ";");
            }

            identacionClase--;
            L("}");
            identacionClase--;
            L("}");
            L("");
        }
        else
        {
            //No trae baja
            L("public List<object> LlenaObjetos(CConexion pConexion)");
            L("{");
            identacionClase++;
            L("CSelect Obten = new CSelect();");
            L("Obten.StoredProcedure.CommandText = " + "\"spb_" + pClaseGenerador.Clase + "_Consultar\";");
            L("Obten.StoredProcedure.Parameters.AddWithValue(\"@Opcion\", 1);");
            L("Obten.Llena<C" + pClaseGenerador.Clase + ">(typeof(C" + pClaseGenerador.Clase + "), pConexion);");
            L("return Obten.ListaRegistros;");
            identacionClase--;
            L("}");
            L("");
            L("public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)");
            L("{");
            identacionClase++;
            L("CSelect Obten = new CSelect();");
            L("Obten.StoredProcedure.CommandText = " + "\"spb_" + pClaseGenerador.Clase + "_Consultar\";");
            L("Obten.StoredProcedure.Parameters.AddWithValue(\"@Opcion\", 1);");
            L("Obten.Columnas = new string[pColumnas.Length];");
            L("Obten.Columnas = pColumnas;");
            L("Obten.Llena<C" + pClaseGenerador.Clase + ">(typeof(C" + pClaseGenerador.Clase + "), pConexion);");
            L("return Obten.ListaRegistros;");
            identacionClase--;
            L("}");
            L("");
            L("public void LlenaObjeto(int pIdentificador, CConexion pConexion)");
            L("{");
            identacionClase++;
            L("CSelect Obten = new CSelect();");
            L("Obten.StoredProcedure.CommandText = " + "\"spb_" + pClaseGenerador.Clase + "_Consultar\";");
            L("Obten.StoredProcedure.Parameters.AddWithValue(\"@Opcion\", 2);");
            L("Obten.StoredProcedure.Parameters.AddWithValue(\"@pId" + pClaseGenerador.Clase + "\", pIdentificador);");
            L("Obten.Llena<C" + pClaseGenerador.Clase + ">(typeof(C" + pClaseGenerador.Clase + "), pConexion);");
            L("foreach (C" + pClaseGenerador.Clase + " O in Obten.ListaRegistros)");
            L("{");
            identacionClase++;
            L(FormatoAtributo("id" + pClaseGenerador.Clase) + " = O." + FormatoGetSet("Id" + pClaseGenerador.Clase) + ";");
            foreach (CClaseAtributo OAtributo in pAtributos)
            {
                L(FormatoAtributo(OAtributo.Atributo) + " = O." + FormatoGetSet(OAtributo.Atributo) + ";");
            }

            if (pClaseGenerador.ManejaBaja)
            {
                L(FormatoAtributo("baja") + " = O." + FormatoGetSet("Baja") + ";");
            }
            identacionClase--;
            L("}");
            identacionClase--;
            L("}");
            L("");
        }

        L("public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)");
        L("{");
        identacionClase++;
        L("CSelect Obten = new CSelect();");
        L("Obten.StoredProcedure.CommandText = " + "\"spb_" + pClaseGenerador.Clase + "_ConsultarFiltros\";");
        L("foreach (KeyValuePair<string, object> parametro in pParametros)");
        L("{");
        identacionClase++;
        L("if (parametro.Key == \"Opcion\")");
        L("{");
        identacionClase++;
        L("Obten.StoredProcedure.Parameters.AddWithValue(\"@\"+parametro.Key, parametro.Value);");
        identacionClase--;
        L("}");
        L("else");
        L("{");
        identacionClase++;
        L("Obten.StoredProcedure.Parameters.AddWithValue(\"@p\"+parametro.Key, parametro.Value);");
        identacionClase--;
        L("}");
        identacionClase--;
        L("}");
        L("Obten.Llena<C" + pClaseGenerador.Clase + ">(typeof(C" + pClaseGenerador.Clase + "), pConexion);");
        L("foreach (C" + pClaseGenerador.Clase + " O in Obten.ListaRegistros)");
        L("{");
        identacionClase++;
        L(FormatoAtributo("id" + pClaseGenerador.Clase) + " = O." + FormatoGetSet("Id" + pClaseGenerador.Clase) + ";");
        foreach (CClaseAtributo OAtributo in pAtributos)
        {
            L(FormatoAtributo(OAtributo.Atributo) + " = O." + FormatoGetSet(OAtributo.Atributo) + ";");
        }

        if (pClaseGenerador.ManejaBaja)
        {
            L(FormatoAtributo("baja") + " = O." + FormatoGetSet("Baja") + ";");
        }
        identacionClase--;
        L("}");
        identacionClase--;
        L("}");
        L("");
        L("public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)");
        L("{");
        identacionClase++;
        L("CSelect Obten = new CSelect();");
        L("Obten.StoredProcedure.CommandText = " + "\"spb_" + pClaseGenerador.Clase + "_ConsultarFiltros\";");
        L("foreach (KeyValuePair<string, object> parametro in pParametros)");
        L("{");
        identacionClase++;
        L("if (parametro.Key == \"Opcion\")");
        L("{");
        identacionClase++;
        L("Obten.StoredProcedure.Parameters.AddWithValue(\"@\"+parametro.Key, parametro.Value);");
        identacionClase--;
        L("}");
        L("else");
        L("{");
        identacionClase++;
        L("Obten.StoredProcedure.Parameters.AddWithValue(\"@p\"+parametro.Key, parametro.Value);");
        identacionClase--;
        L("}");
        identacionClase--;
        L("}");
        L("Obten.Llena<C" + pClaseGenerador.Clase + ">(typeof(C" + pClaseGenerador.Clase + "), pConexion);");
        L("return Obten.ListaRegistros;");
        identacionClase--;
        L("}");
        L("");
        L("public void Agregar(CConexion pConexion)");
        L("{");
        identacionClase++;
        L("CConsultaAccion Agregar = new CConsultaAccion();");
        L("Agregar.StoredProcedure.CommandText = " + "\"spb_" + pClaseGenerador.Clase + "_Agregar\";");
        L("Agregar.StoredProcedure.Parameters.AddWithValue(\"@Opcion\", 1);");
        L("Agregar.StoredProcedure.Parameters.AddWithValue(\"@pId" + pClaseGenerador.Clase + "\", 0);");
        L("Agregar.StoredProcedure.Parameters[\"@pId" + pClaseGenerador.Clase + "\"].Direction = ParameterDirection.Output;");
        foreach (CClaseAtributo OAtributo in pAtributos)
        {
            if (OAtributo.TipoAtributo == "DT")
            {
                L("if(" + FormatoAtributo(OAtributo.Atributo) + ".Year != 1)");
                L("{");
                identacionClase++;
                L("Agregar.StoredProcedure.Parameters.AddWithValue(\"@p" + FormatoGetSet(OAtributo.Atributo) + "\", " + FormatoAtributo(OAtributo.Atributo) + ");");
                identacionClase--;
                L("}");
            }
            else
            {
                L("Agregar.StoredProcedure.Parameters.AddWithValue(\"@p" + FormatoGetSet(OAtributo.Atributo) + "\", " + FormatoAtributo(OAtributo.Atributo) + ");");
            }
        }
        if (pClaseGenerador.ManejaBaja)
        {
            L("Agregar.StoredProcedure.Parameters.AddWithValue(\"@p" + FormatoGetSet("Baja") + "\", " + FormatoAtributo("baja") + ");");
        }
        L("Agregar.Insert(pConexion);");
        L("id" + pClaseGenerador.Clase + "= Convert.ToInt32(Agregar.StoredProcedure.Parameters[\"@pId" + pClaseGenerador.Clase + "\"].Value);");
        identacionClase--;
        L("}");
        L("");
        L("public void Editar(CConexion pConexion)");
        L("{");
        identacionClase++;
        L("CConsultaAccion Editar = new CConsultaAccion();");
        L("Editar.StoredProcedure.CommandText = " + "\"spb_" + pClaseGenerador.Clase + "_Editar\";");
        L("Editar.StoredProcedure.Parameters.AddWithValue(\"@Opcion\", 1);");
        L("Editar.StoredProcedure.Parameters.AddWithValue(\"@pId" + pClaseGenerador.Clase + "\", id" + FormatoGetSet(pClaseGenerador.Clase) + ");");
        foreach (CClaseAtributo OAtributo in pAtributos)
        {
            if (OAtributo.TipoAtributo == "DT")
            {
                L("if(" + FormatoAtributo(OAtributo.Atributo) + ".Year != 1)");
                L("{");
                identacionClase++;
                L("Editar.StoredProcedure.Parameters.AddWithValue(\"@p" + FormatoGetSet(OAtributo.Atributo) + "\", " + FormatoAtributo(OAtributo.Atributo) + ");");
                identacionClase--;
                L("}");
            }
            else
            {
                L("Editar.StoredProcedure.Parameters.AddWithValue(\"@p" + FormatoGetSet(OAtributo.Atributo) + "\", " + FormatoAtributo(OAtributo.Atributo) + ");");
            }
        }
        if (pClaseGenerador.ManejaBaja)
        {
            L("Editar.StoredProcedure.Parameters.AddWithValue(\"@pBaja\", baja);");
        }
        L("Editar.Update(pConexion);");
        identacionClase--;
        L("}");
        L("");
        L("public void Eliminar(CConexion pConexion)");
        L("{");
        identacionClase++;
        L("CConsultaAccion Eliminar = new CConsultaAccion();");
        L("Eliminar.StoredProcedure.CommandText = " + "\"spb_" + pClaseGenerador.Clase + "_Eliminar\";");
        if (pClaseGenerador.ManejaBaja)
        {
            L("Eliminar.StoredProcedure.Parameters.AddWithValue(\"@Opcion\", 2);");
            L("Eliminar.StoredProcedure.Parameters.AddWithValue(\"@p" + FormatoGetSet("Id" + pClaseGenerador.Clase) + "\", " + FormatoAtributo("id" + pClaseGenerador.Clase) + ");");
            L("Eliminar.StoredProcedure.Parameters.AddWithValue(\"@pBaja\", baja);");
            L("Eliminar.Delete(pConexion);");
        }
        else
        {
            L("Eliminar.StoredProcedure.Parameters.AddWithValue(\"@Opcion\", 1);");
            L("Eliminar.StoredProcedure.Parameters.AddWithValue(\"@p" + FormatoGetSet("Id" + pClaseGenerador.Clase) + "\", " + FormatoAtributo("id" + pClaseGenerador.Clase) + ");");
            L("Eliminar.Delete(pConexion);");
        }
        identacionClase--;
        L("}");
        identacionClase--;
        L("}");
        EscribirPagina("ClaseParcialBase");
    }

    public void CrearClaseConfigurable(CClaseGenerador pClaseGenerador, CConexion pConexion)
    {
        InicializarPagina();
        identacionClase = 0;
        L("using System;");
        L("using System.Collections.Generic;");
        L("using System.Linq;");
        L("using System.Text;");
        L("using System.Data;");
        L("using System.Data.SqlClient;");
        L("using System.Data.OleDb;");
        L("using System.IO;");
        L("using System.Web;");
        L("");        
        L("public partial class C" + pClaseGenerador.Clase);
        L("{");
        identacionClase++;
        L("//Constructores");
        L("");
        L("//Metodos Especiales");
        identacionClase--;
        L("");
        L("}");        
        EscribirPagina("ClaseParcialConfigurable");
    }

    private void L(string pLinea)
    {
        arrClase.Add(ObtenerIdentacion(identacionClase) + pLinea);
    }

    private void InicializarPagina()
    {
        arrClase = new List<string>();
    }

    private void EscribirPagina(string pTipoParcial)
    {
        string ruta = AppDomain.CurrentDomain.BaseDirectory;
        switch (pTipoParcial)
        {
            case "ClaseParcialBase":
                File.WriteAllLines(ruta + "App_Code\\Entidades\\ClasesParcialesBasicas\\C" + Clase + "_ParcialBasico.cs", arrClase.ToArray());
                break;
            case "ClaseParcialConfigurable":
                File.WriteAllLines(ruta + "App_Code\\Entidades\\C" + Clase + ".cs", arrClase.ToArray());
                break;
        }
    }

    private string FormatoAtributo(string aFormatear)
    {
        string attrARegresar = aFormatear.Substring(0, 1).ToLower() + aFormatear.Substring(1, aFormatear.Length - 1);
        return attrARegresar;
    }

    private string FormatoGetSet(string aFormatear)
    {
        string attrParaGetSet = aFormatear.Substring(0, 1).ToUpper() + aFormatear.Substring(1, aFormatear.Length - 1);
        return attrParaGetSet;
    }

    private string ObtenerTipoDato(CClaseAtributo pAtributo)
    {
        string tipoDato = "";
        switch (pAtributo.TipoAtributo)
        {
            case "I":
                tipoDato = "INT";
                break;
            case "S":
                tipoDato = "VARCHAR(" + pAtributo.Longitud + ")";
                break;
            case "D":
                tipoDato = "DECIMAL(" + pAtributo.Longitud + ", " + pAtributo.Decimales + ")";
                break;
            case "DT":
                tipoDato = "DATETIME";
                break;
            case "B":
                tipoDato = "BIT";
                break;
        }
        return tipoDato;
    }
}