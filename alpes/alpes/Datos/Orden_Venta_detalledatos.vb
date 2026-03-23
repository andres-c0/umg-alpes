Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

Public Class OrdenVentaDetalleDatos

    Private ReadOnly _conexion As ConexionOracle

    Public Sub New()
        _conexion = New ConexionOracle()
    End Sub


Public Function Insertar(entidad As OrdenVentaDetalle) As Integer

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_INSERTAR", conn)

cmd.CommandType = CommandType.StoredProcedure

cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = entidad.Cantidad
cmd.Parameters.Add("P_PRECIO_UNITARIO", OracleDbType.Decimal).Value = entidad.PrecioUnitario
cmd.Parameters.Add("P_SUBTOTAL", OracleDbType.Decimal).Value = entidad.Subtotal

Dim pOut As New OracleParameter("P_ID", OracleDbType.Int32)
pOut.Direction = ParameterDirection.Output
cmd.Parameters.Add(pOut)

conn.Open()
cmd.ExecuteNonQuery()

Return Convert.ToInt32(pOut.Value.ToString())

End Using
End Using

End Function



Public Sub Actualizar(entidad As OrdenVentaDetalle)

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_ACTUALIZAR", conn)

cmd.CommandType = CommandType.StoredProcedure

cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = entidad.OrdenVentaDetalleId
cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = entidad.Cantidad
cmd.Parameters.Add("P_PRECIO_UNITARIO", OracleDbType.Decimal).Value = entidad.PrecioUnitario
cmd.Parameters.Add("P_SUBTOTAL", OracleDbType.Decimal).Value = entidad.Subtotal

conn.Open()
cmd.ExecuteNonQuery()

End Using
End Using

End Sub



Public Sub Eliminar(id As Integer)

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_ELIMINAR", conn)

cmd.CommandType = CommandType.StoredProcedure
cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id

conn.Open()
cmd.ExecuteNonQuery()

End Using
End Using

End Sub



Public Function Listar() As DataTable

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_LISTAR", conn)

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



Public Function Buscar(ordenVentaId As Integer) As DataTable

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_ORDEN_VENTA_DETALLE.SP_BUSCAR", conn)

cmd.CommandType = CommandType.StoredProcedure

cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = ordenVentaId
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