Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

Public Class PagoDatos

    Private ReadOnly _conexion As ConexionOracle

    Public Sub New()
        _conexion = New ConexionOracle()
    End Sub


Public Function Insertar(entidad As Pago) As Integer

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_PAGO.SP_INSERTAR", conn)

cmd.CommandType = CommandType.StoredProcedure

cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
cmd.Parameters.Add("P_METODO_PAGO_ID", OracleDbType.Int32).Value = entidad.MetodoPagoId
cmd.Parameters.Add("P_FECHA_PAGO", OracleDbType.Date).Value = entidad.FechaPago
cmd.Parameters.Add("P_MONTO", OracleDbType.Decimal).Value = entidad.Monto

Dim pOut As New OracleParameter("P_ID", OracleDbType.Int32)
pOut.Direction = ParameterDirection.Output
cmd.Parameters.Add(pOut)

conn.Open()
cmd.ExecuteNonQuery()

Return Convert.ToInt32(pOut.Value.ToString())

End Using
End Using

End Function



Public Sub Actualizar(entidad As Pago)

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_PAGO.SP_ACTUALIZAR", conn)

cmd.CommandType = CommandType.StoredProcedure

cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = entidad.PagoId
cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
cmd.Parameters.Add("P_METODO_PAGO_ID", OracleDbType.Int32).Value = entidad.MetodoPagoId
cmd.Parameters.Add("P_FECHA_PAGO", OracleDbType.Date).Value = entidad.FechaPago
cmd.Parameters.Add("P_MONTO", OracleDbType.Decimal).Value = entidad.Monto

conn.Open()
cmd.ExecuteNonQuery()

End Using
End Using

End Sub



Public Sub Eliminar(id As Integer)

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_PAGO.SP_ELIMINAR", conn)

cmd.CommandType = CommandType.StoredProcedure
cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id

conn.Open()
cmd.ExecuteNonQuery()

End Using
End Using

End Sub



Public Function Listar() As DataTable

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_PAGO.SP_LISTAR", conn)

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

Using cmd As New OracleCommand("PKG_PAGO.SP_BUSCAR", conn)

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