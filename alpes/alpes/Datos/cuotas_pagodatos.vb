Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

Public Class CuotasPagoDatos

    Private ReadOnly _conexion As ConexionOracle

    Public Sub New()
        _conexion = New ConexionOracle()
    End Sub


Public Function Insertar(entidad As CuotasPago) As Integer

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_INSERTAR", conn)

cmd.CommandType = CommandType.StoredProcedure

cmd.Parameters.Add("P_PAGO_ID", OracleDbType.Int32).Value = entidad.PagoId
cmd.Parameters.Add("P_NUMERO_CUOTA", OracleDbType.Int32).Value = entidad.NumeroCuota
cmd.Parameters.Add("P_MONTO", OracleDbType.Decimal).Value = entidad.Monto
cmd.Parameters.Add("P_FECHA_VENCIMIENTO", OracleDbType.Date).Value = entidad.FechaVencimiento

Dim pOut As New OracleParameter("P_ID", OracleDbType.Int32)
pOut.Direction = ParameterDirection.Output
cmd.Parameters.Add(pOut)

conn.Open()
cmd.ExecuteNonQuery()

Return Convert.ToInt32(pOut.Value.ToString())

End Using
End Using

End Function



Public Sub Actualizar(entidad As CuotasPago)

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_ACTUALIZAR", conn)

cmd.CommandType = CommandType.StoredProcedure

cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = entidad.CuotasPagoId
cmd.Parameters.Add("P_PAGO_ID", OracleDbType.Int32).Value = entidad.PagoId
cmd.Parameters.Add("P_NUMERO_CUOTA", OracleDbType.Int32).Value = entidad.NumeroCuota
cmd.Parameters.Add("P_MONTO", OracleDbType.Decimal).Value = entidad.Monto
cmd.Parameters.Add("P_FECHA_VENCIMIENTO", OracleDbType.Date).Value = entidad.FechaVencimiento

conn.Open()
cmd.ExecuteNonQuery()

End Using
End Using

End Sub



Public Sub Eliminar(id As Integer)

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_ELIMINAR", conn)

cmd.CommandType = CommandType.StoredProcedure
cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id

conn.Open()
cmd.ExecuteNonQuery()

End Using
End Using

End Sub



Public Function Listar() As DataTable

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_LISTAR", conn)

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



Public Function Buscar(pagoId As Integer) As DataTable

Using conn = _conexion.ObtenerConexion()

Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_BUSCAR", conn)

cmd.CommandType = CommandType.StoredProcedure

cmd.Parameters.Add("P_PAGO_ID", OracleDbType.Int32).Value = pagoId
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