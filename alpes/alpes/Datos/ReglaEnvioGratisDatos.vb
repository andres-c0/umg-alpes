Option Strict On
Option Explicit On

Imports System.Configuration
Imports System.Data
Imports Oracle.ManagedDataAccess.Client

Public Class ReglaEnvioGratisDatos

    Private ReadOnly _connectionString As String

    Public Sub New()
        _connectionString = ConfigurationManager.ConnectionStrings("OracleConnection").ConnectionString
    End Sub

    Public Function Insertar(ByVal entidad As ReglaEnvioGratis) As Integer
        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_INSERTAR_REGLA_ENVIO_GRATIS", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = entidad.ZonaEnvioId
            cmd.Parameters.Add("P_MONTO_MINIMO", OracleDbType.Decimal).Value = If(entidad.MontoMinimo.HasValue, CType(entidad.MontoMinimo.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_PESO_MAX_KG", OracleDbType.Decimal).Value = If(entidad.PesoMaxKg.HasValue, CType(entidad.PesoMaxKg.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = If(entidad.VigenciaInicio.HasValue, CType(entidad.VigenciaInicio.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = If(entidad.VigenciaFin.HasValue, CType(entidad.VigenciaFin.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_REGLA_ENVIO_GRATIS_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                cn.Open()
                cmd.ExecuteNonQuery()
                Return Convert.ToInt32(cmd.Parameters("P_REGLA_ENVIO_GRATIS_ID").Value.ToString())
            End Using
        End Using
    End Function

    Public Sub Actualizar(ByVal entidad As ReglaEnvioGratis)
        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_ACTUALIZAR_REGLA_ENVIO_GRATIS", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_REGLA_ENVIO_GRATIS_ID", OracleDbType.Int32).Value = entidad.ReglaEnvioGratisId
            cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = entidad.ZonaEnvioId
            cmd.Parameters.Add("P_MONTO_MINIMO", OracleDbType.Decimal).Value = If(entidad.MontoMinimo.HasValue, CType(entidad.MontoMinimo.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_PESO_MAX_KG", OracleDbType.Decimal).Value = If(entidad.PesoMaxKg.HasValue, CType(entidad.PesoMaxKg.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = If(entidad.VigenciaInicio.HasValue, CType(entidad.VigenciaInicio.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = If(entidad.VigenciaFin.HasValue, CType(entidad.VigenciaFin.Value, Object), DBNull.Value)

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Eliminar(ByVal id As Integer)
        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_ELIMINAR_REGLA_ENVIO_GRATIS", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_REGLA_ENVIO_GRATIS_ID", OracleDbType.Int32).Value = id

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ObtenerPorId(ByVal id As Integer) As DataTable
        Dim dt As New DataTable()

        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_OBTENER_REGLA_ENVIO_GRATIS", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_REGLA_ENVIO_GRATIS_ID", OracleDbType.Int32).Value = id
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
            Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_LISTAR_REGLAS_ENVIO_GRATIS", cn)
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
            Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_BUSCAR_REGLAS_ENVIO_GRATIS", cn)
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
