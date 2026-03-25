Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client

Public Class PoliticaEnvioDatos

    Private ReadOnly _conexion As Datos.ConexionOracle

    Public Sub New()
        _conexion = New Datos.ConexionOracle()
    End Sub

    Public Function Insertar(ByVal entidad As PoliticaEnvio) As Integer
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_POLITICA_ENVIO.SP_INSERTAR_POLITICA_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_TITULO", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Titulo), CType(DBNull.Value, Object), entidad.Titulo)
            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Descripcion), CType(DBNull.Value, Object), entidad.Descripcion)
            cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = If(entidad.VigenciaInicio.HasValue, CType(entidad.VigenciaInicio.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = If(entidad.VigenciaFin.HasValue, CType(entidad.VigenciaFin.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_POLITICA_ENVIO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                cn.Open()
                cmd.ExecuteNonQuery()
                Return Convert.ToInt32(cmd.Parameters("P_POLITICA_ENVIO_ID").Value.ToString())
            End Using
        End Using
    End Function

    Public Sub Actualizar(ByVal entidad As PoliticaEnvio)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_POLITICA_ENVIO.SP_ACTUALIZAR_POLITICA_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_POLITICA_ENVIO_ID", OracleDbType.Int32).Value = entidad.PoliticaEnvioId
            cmd.Parameters.Add("P_TITULO", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Titulo), CType(DBNull.Value, Object), entidad.Titulo)
            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Descripcion), CType(DBNull.Value, Object), entidad.Descripcion)
            cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = If(entidad.VigenciaInicio.HasValue, CType(entidad.VigenciaInicio.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = If(entidad.VigenciaFin.HasValue, CType(entidad.VigenciaFin.Value, Object), DBNull.Value)

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Eliminar(ByVal id As Integer)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_POLITICA_ENVIO.SP_ELIMINAR_POLITICA_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_POLITICA_ENVIO_ID", OracleDbType.Int32).Value = id

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ObtenerPorId(ByVal id As Integer) As DataTable
        Dim dt As New DataTable()

        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_POLITICA_ENVIO.SP_OBTENER_POLITICA_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_POLITICA_ENVIO_ID", OracleDbType.Int32).Value = id
                cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                Using da As New OracleDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function

    Public Function Listar() As DataTable
        Dim dt As New DataTable()

        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_POLITICA_ENVIO.SP_LISTAR_POLITICAS_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                Using da As New OracleDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function

    Public Function Buscar(ByVal criterio As String, ByVal valor As String) As DataTable
        Dim dt As New DataTable()

        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_POLITICA_ENVIO.SP_BUSCAR_POLITICAS_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                Using da As New OracleDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function

End Class
