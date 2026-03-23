Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class HistorialCompraDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(historialCompra As HistorialCompra) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_INSERTAR_HISTORIAL_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = historialCompra.CliId
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = historialCompra.OrdenVentaId
                    cmd.Parameters.Add("P_COMPRA_AT", OracleDbType.TimeStamp).Value = historialCompra.CompraAt
                    cmd.Parameters.Add("P_MONTO_TOTAL_SNAPSHOT", OracleDbType.Decimal).Value = historialCompra.MontoTotalSnapshot

                    Dim pOut As New OracleParameter("P_HIST_COMPRA_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(historialCompra As HistorialCompra)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_ACTUALIZAR_HISTORIAL_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HIST_COMPRA_ID", OracleDbType.Int32).Value = historialCompra.HistCompraId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = historialCompra.CliId
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = historialCompra.OrdenVentaId
                    cmd.Parameters.Add("P_COMPRA_AT", OracleDbType.TimeStamp).Value = historialCompra.CompraAt
                    cmd.Parameters.Add("P_MONTO_TOTAL_SNAPSHOT", OracleDbType.Decimal).Value = historialCompra.MontoTotalSnapshot

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(historialCompraId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_ELIMINAR_HISTORIAL_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_HIST_COMPRA_ID", OracleDbType.Int32).Value = historialCompraId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_LISTAR_HISTORIAL_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output
                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

        Public Function Buscar(criterio As String, valor As String) As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_BUSCAR_HISTORIAL_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output
                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

    End Class

End Namespace
