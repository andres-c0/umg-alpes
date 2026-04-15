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
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = entidad.Cantidad
                    cmd.Parameters.Add("P_PRECIO_UNITARIO_SNAPSHOT", OracleDbType.Decimal).Value = entidad.PrecioUnitarioSnapshot
                    cmd.Parameters.Add("P_SUBTOTAL_LINEA", OracleDbType.Decimal).Value = entidad.SubtotalLinea

                    If String.IsNullOrWhiteSpace(entidad.Estado) Then
                        cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado
                    End If

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Orden_Venta_Detalle)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_DET_ID", OracleDbType.Int32).Value = entidad.OrdenVentaDetId
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = entidad.Cantidad
                    cmd.Parameters.Add("P_PRECIO_UNITARIO_SNAPSHOT", OracleDbType.Decimal).Value = entidad.PrecioUnitarioSnapshot
                    cmd.Parameters.Add("P_SUBTOTAL_LINEA", OracleDbType.Decimal).Value = entidad.SubtotalLinea
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Orden_Venta_Detalle
            Dim entidad As Orden_Venta_Detalle = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_OBTENER", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearOrdenVentaDetalle(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Orden_Venta_Detalle)
            Dim lista As New List(Of Orden_Venta_Detalle)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearOrdenVentaDetalle(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As Integer) As List(Of Orden_Venta_Detalle)
            Dim lista As New List(Of Orden_Venta_Detalle)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_BUSCAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_VALOR", OracleDbType.Int32).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearOrdenVentaDetalle(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearOrdenVentaDetalle(ByVal dr As OracleDataReader) As Orden_Venta_Detalle
            Dim entidad As New Orden_Venta_Detalle()

            entidad.OrdenVentaDetId = Convert.ToInt32(dr("orden_venta_det_id"))
            entidad.OrdenVentaId = Convert.ToInt32(dr("orden_venta_id"))
            entidad.ProductoId = Convert.ToInt32(dr("producto_id"))
            entidad.Cantidad = Convert.ToInt32(dr("cantidad"))
            entidad.PrecioUnitarioSnapshot = Convert.ToDecimal(dr("precio_unitario_snapshot"))
            entidad.SubtotalLinea = Convert.ToDecimal(dr("subtotal_linea"))
            entidad.CreatedAt = Convert.ToDateTime(dr("created_at"))

            If IsDBNull(dr("updated_at")) Then
                entidad.UpdatedAt = Nothing
            Else
                entidad.UpdatedAt = Convert.ToDateTime(dr("updated_at"))
            End If

            entidad.Estado = dr("estado").ToString()

            Return entidad
        End Function

    End Class
End Namespace