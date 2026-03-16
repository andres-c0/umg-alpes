Option Strict On
Option Explicit On

Imports System.Configuration
Imports System.Data
Imports Oracle.ManagedDataAccess.Client

Public Class TarifaEnvioDatos

    Private ReadOnly _connectionString As String

    Public Sub New()
        _connectionString = ConfigurationManager.ConnectionStrings("OracleConnection").ConnectionString
    End Sub

    Public Function Insertar(ByVal entidad As TarifaEnvio) As Integer
        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_INSERTAR_TARIFA_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = entidad.ZonaEnvioId
            cmd.Parameters.Add("P_TIPO_ENTREGA_ID", OracleDbType.Int32).Value = entidad.TipoEntregaId
            cmd.Parameters.Add("P_PESO_DESDE_KG", OracleDbType.Decimal).Value = entidad.PesoDesdeKg
            cmd.Parameters.Add("P_PESO_HASTA_KG", OracleDbType.Decimal).Value = entidad.PesoHastaKg
            cmd.Parameters.Add("P_COSTO", OracleDbType.Decimal).Value = entidad.Costo
            cmd.Parameters.Add("P_TARIFA_ENVIO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                cn.Open()
                cmd.ExecuteNonQuery()
                Return Convert.ToInt32(cmd.Parameters("P_TARIFA_ENVIO_ID").Value.ToString())
            End Using
        End Using
    End Function

    Public Sub Actualizar(ByVal entidad As TarifaEnvio)
        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_ACTUALIZAR_TARIFA_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_TARIFA_ENVIO_ID", OracleDbType.Int32).Value = entidad.TarifaEnvioId
            cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = entidad.ZonaEnvioId
            cmd.Parameters.Add("P_TIPO_ENTREGA_ID", OracleDbType.Int32).Value = entidad.TipoEntregaId
            cmd.Parameters.Add("P_PESO_DESDE_KG", OracleDbType.Decimal).Value = entidad.PesoDesdeKg
            cmd.Parameters.Add("P_PESO_HASTA_KG", OracleDbType.Decimal).Value = entidad.PesoHastaKg
            cmd.Parameters.Add("P_COSTO", OracleDbType.Decimal).Value = entidad.Costo

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Eliminar(ByVal id As Integer)
        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_ELIMINAR_TARIFA_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_TARIFA_ENVIO_ID", OracleDbType.Int32).Value = id

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ObtenerPorId(ByVal id As Integer) As DataTable
        Dim dt As New DataTable()

        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_OBTENER_TARIFA_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_TARIFA_ENVIO_ID", OracleDbType.Int32).Value = id
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
            Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_LISTAR_TARIFAS_ENVIO", cn)
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
            Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_BUSCAR_TARIFAS_ENVIO", cn)
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
