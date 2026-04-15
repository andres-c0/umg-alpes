Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class HistorialCompraDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As HistorialCompra) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_INSERTAR_HISTORIAL_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
                    cmd.Parameters.Add("P_COMPRA_AT", OracleDbType.TimeStamp).Value = entidad.CompraAt
                    cmd.Parameters.Add("P_MONTO_TOTAL_SNAPSHOT", OracleDbType.Decimal).Value = entidad.MontoTotalSnapshot

                    cmd.Parameters.Add("P_HIST_COMPRA_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_HIST_COMPRA_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As HistorialCompra)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_ACTUALIZAR_HISTORIAL_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HIST_COMPRA_ID", OracleDbType.Int32).Value = entidad.HistCompraId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
                    cmd.Parameters.Add("P_COMPRA_AT", OracleDbType.TimeStamp).Value = entidad.CompraAt
                    cmd.Parameters.Add("P_MONTO_TOTAL_SNAPSHOT", OracleDbType.Decimal).Value = entidad.MontoTotalSnapshot

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_ELIMINAR_HISTORIAL_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HIST_COMPRA_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As HistorialCompra
            Dim entidad As HistorialCompra = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_OBTENER_HISTORIAL_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HIST_COMPRA_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = Mapear(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of HistorialCompra)
            Dim lista As New List(Of HistorialCompra)

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_LISTAR_HISTORIAL_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of HistorialCompra)
            Dim lista As New List(Of HistorialCompra)

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_BUSCAR_HISTORIAL_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = "CLI_ID"
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function Mapear(ByVal dr As OracleDataReader) As HistorialCompra
            Dim entidad As New HistorialCompra With {
                .HistCompraId = Convert.ToInt32(dr("HIST_COMPRA_ID")),
                .CliId = Convert.ToInt32(dr("CLI_ID")),
                .OrdenVentaId = Convert.ToInt32(dr("ORDEN_VENTA_ID")),
                .CompraAt = Convert.ToDateTime(dr("COMPRA_AT")),
                .MontoTotalSnapshot = Convert.ToDecimal(dr("MONTO_TOTAL_SNAPSHOT")),
                .Estado = dr("ESTADO").ToString()
            }

            Return entidad
        End Function

    End Class
End Namespace