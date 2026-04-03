Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class Orden_Venta_DetalleDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Orden_Venta_Detalle) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_INSERTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", entidad.orden_venta_id)
                    cmd.Parameters.Add("P_PRODUCTO_ID", entidad.producto_id)
                    cmd.Parameters.Add("P_CANTIDAD", entidad.cantidad)
                    cmd.Parameters.Add("P_PRECIO_UNITARIO_SNAPSHOT", entidad.precio_unitario_snapshot)
                    cmd.Parameters.Add("P_SUBTOTAL_LINEA", entidad.subtotal_linea)
                    cmd.Parameters.Add("P_ESTADO", entidad.estado)

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        Return Convert.ToInt32(cmd.Parameters("P_ID").Value)
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Orden_Venta_Detalle)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_ACTUALIZAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_DET_ID", entidad.orden_venta_det_id)
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", entidad.orden_venta_id)
                    cmd.Parameters.Add("P_PRODUCTO_ID", entidad.producto_id)
                    cmd.Parameters.Add("P_CANTIDAD", entidad.cantidad)
                    cmd.Parameters.Add("P_PRECIO_UNITARIO_SNAPSHOT", entidad.precio_unitario_snapshot)
                    cmd.Parameters.Add("P_SUBTOTAL_LINEA", entidad.subtotal_linea)
                    cmd.Parameters.Add("P_ESTADO", entidad.estado)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_ELIMINAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", id)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Orden_Venta_Detalle
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_OBTENER", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", id)
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            Return Mapear(dr)
                        End If
                    End Using
                End Using
            End Using

            Return Nothing
        End Function

        Public Function Listar() As List(Of Orden_Venta_Detalle)
            Dim lista As New List(Of Orden_Venta_Detalle)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_LISTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As Integer) As List(Of Orden_Venta_Detalle)
            Dim lista As New List(Of Orden_Venta_Detalle)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_BUSCAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_VALOR", valor)
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function Mapear(ByVal dr As OracleDataReader) As Orden_Venta_Detalle
            Dim entidad As New Orden_Venta_Detalle()

            entidad.orden_venta_det_id = Convert.ToInt32(dr("orden_venta_det_id"))
            entidad.orden_venta_id = Convert.ToInt32(dr("orden_venta_id"))
            entidad.producto_id = Convert.ToInt32(dr("producto_id"))
            entidad.cantidad = Convert.ToInt32(dr("cantidad"))
            entidad.precio_unitario_snapshot = Convert.ToDecimal(dr("precio_unitario_snapshot"))
            entidad.subtotal_linea = Convert.ToDecimal(dr("subtotal_linea"))
            entidad.created_at = Convert.ToDateTime(dr("created_at"))
            entidad.updated_at = If(IsDBNull(dr("updated_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("updated_at")))
            entidad.estado = dr("estado").ToString()

            Return entidad
        End Function

    End Class
End Namespace
