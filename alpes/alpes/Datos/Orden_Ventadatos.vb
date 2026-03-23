Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

Public Class OrdenVentaDatos

    Private ReadOnly _conexion As ConexionOracle

    Public Sub New()
        _conexion = New ConexionOracle()
    End Sub



Public Function Insertar(entidad As OrdenVenta) As Integer

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_INSERTAR", conn)

cmd.CommandType = CommandType.StoredProcedure

cmd.Parameters.Add("P_CLIENTE_ID", OracleDbType.Int32).Value = entidad.ClienteId
cmd.Parameters.Add("P_FECHA", OracleDbType.Date).Value = entidad.Fecha
cmd.Parameters.Add("P_TOTAL", OracleDbType.Decimal).Value = entidad.Total
cmd.Parameters.Add("P_ESTADO_ORDEN_ID", OracleDbType.Int32).Value = entidad.EstadoOrdenId

Dim pOut As New OracleParameter("P_ID", OracleDbType.Int32)
pOut.Direction = ParameterDirection.Output
cmd.Parameters.Add(pOut)

conn.Open()
cmd.ExecuteNonQuery()

Return Convert.ToInt32(pOut.Value.ToString())

End Using
End Using

End Function



Public Sub Actualizar(entidad As OrdenVenta)

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_ACTUALIZAR", conn)

cmd.CommandType = CommandType.StoredProcedure

cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
cmd.Parameters.Add("P_CLIENTE_ID", OracleDbType.Int32).Value = entidad.ClienteId
cmd.Parameters.Add("P_FECHA", OracleDbType.Date).Value = entidad.Fecha
cmd.Parameters.Add("P_TOTAL", OracleDbType.Decimal).Value = entidad.Total
cmd.Parameters.Add("P_ESTADO_ORDEN_ID", OracleDbType.Int32).Value = entidad.EstadoOrdenId

conn.Open()
cmd.ExecuteNonQuery()

End Using
End Using

End Sub



Public Sub Eliminar(id As Integer)

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_ELIMINAR", conn)

cmd.CommandType = CommandType.StoredProcedure
cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id

conn.Open()
cmd.ExecuteNonQuery()

End Using
End Using

End Sub



Public Function Listar() As DataTable

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_LISTAR", conn)

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



Public Function Buscar(clienteId As Integer) As DataTable

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_BUSCAR", conn)

cmd.CommandType = CommandType.StoredProcedure

cmd.Parameters.Add("P_CLIENTE_ID", OracleDbType.Int32).Value = clienteId
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