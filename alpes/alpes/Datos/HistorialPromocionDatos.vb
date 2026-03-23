Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class HistorialPromocionDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(historialPromocion As HistorialPromocion) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_INSERTAR_HISTORIAL_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = historialPromocion.OrdenVentaId
                    cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = historialPromocion.PromocionId
                    cmd.Parameters.Add("P_MONTO_DESCUENTO_SNAPSHOT", OracleDbType.Decimal).Value = historialPromocion.MontoDescuentoSnapshot

                    Dim pOut As New OracleParameter("P_HISTORIAL_PROMOCION_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(historialPromocion As HistorialPromocion)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_ACTUALIZAR_HISTORIAL_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HISTORIAL_PROMOCION_ID", OracleDbType.Int32).Value = historialPromocion.HistorialPromocionId
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = historialPromocion.OrdenVentaId
                    cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = historialPromocion.PromocionId
                    cmd.Parameters.Add("P_MONTO_DESCUENTO_SNAPSHOT", OracleDbType.Decimal).Value = historialPromocion.MontoDescuentoSnapshot

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(historialPromocionId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_ELIMINAR_HISTORIAL_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_HISTORIAL_PROMOCION_ID", OracleDbType.Int32).Value = historialPromocionId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_LISTAR_HISTORIAL_PROMOCION", conn)
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
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_BUSCAR_HISTORIAL_PROMOCION", conn)
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
