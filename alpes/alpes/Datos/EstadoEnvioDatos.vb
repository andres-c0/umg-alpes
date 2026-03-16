Option Strict On
Option Explicit On

Imports System.Configuration
Imports System.Data
Imports Oracle.ManagedDataAccess.Client

Public Class EstadoEnvioDatos

    Private ReadOnly _connectionString As String

    Public Sub New()
        _connectionString = ConfigurationManager.ConnectionStrings("OracleConnection").ConnectionString
    End Sub

    Public Function Insertar(ByVal entidad As EstadoEnvio) As Integer
        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_ESTADO_ENVIO.SP_INSERTAR_ESTADO_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Codigo), CType(DBNull.Value, Object), entidad.Codigo)
            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Descripcion), CType(DBNull.Value, Object), entidad.Descripcion)
            cmd.Parameters.Add("P_ESTADO_ENVIO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                cn.Open()
                cmd.ExecuteNonQuery()
                Return Convert.ToInt32(cmd.Parameters("P_ESTADO_ENVIO_ID").Value.ToString())
            End Using
        End Using
    End Function

    Public Sub Actualizar(ByVal entidad As EstadoEnvio)
        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_ESTADO_ENVIO.SP_ACTUALIZAR_ESTADO_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_ESTADO_ENVIO_ID", OracleDbType.Int32).Value = entidad.EstadoEnvioId
            cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Codigo), CType(DBNull.Value, Object), entidad.Codigo)
            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Descripcion), CType(DBNull.Value, Object), entidad.Descripcion)

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Eliminar(ByVal id As Integer)
        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_ESTADO_ENVIO.SP_ELIMINAR_ESTADO_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_ESTADO_ENVIO_ID", OracleDbType.Int32).Value = id

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ObtenerPorId(ByVal id As Integer) As DataTable
        Dim dt As New DataTable()

        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_ESTADO_ENVIO.SP_OBTENER_ESTADO_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_ESTADO_ENVIO_ID", OracleDbType.Int32).Value = id
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

        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_ESTADO_ENVIO.SP_LISTAR_ESTADOS_ENVIO", cn)
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

        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_ESTADO_ENVIO.SP_BUSCAR_ESTADOS_ENVIO", cn)
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
