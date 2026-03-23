Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class PagoProveedorDatos
        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(pp As PagoProveedor) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO_PROVEEDOR.SP_INSERTAR_PAGO_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CUENTA_PAGAR_ID", OracleDbType.Int32).Value    = pp.CuentaPagarId
                    cmd.Parameters.Add("P_MONTO",           OracleDbType.Decimal).Value  = pp.Monto
                    cmd.Parameters.Add("P_FECHA_PAGO",      OracleDbType.Date).Value     = pp.FechaPago
                    cmd.Parameters.Add("P_REFERENCIA",      OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(pp.Referencia), DBNull.Value, pp.Referencia)
                    Dim pOut As New OracleParameter("P_PAGO_PROVEEDOR_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(pp As PagoProveedor)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO_PROVEEDOR.SP_ACTUALIZAR_PAGO_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PAGO_PROVEEDOR_ID", OracleDbType.Int32).Value    = pp.PagoProveedorId
                    cmd.Parameters.Add("P_CUENTA_PAGAR_ID",   OracleDbType.Int32).Value    = pp.CuentaPagarId
                    cmd.Parameters.Add("P_MONTO",             OracleDbType.Decimal).Value  = pp.Monto
                    cmd.Parameters.Add("P_FECHA_PAGO",        OracleDbType.Date).Value     = pp.FechaPago
                    cmd.Parameters.Add("P_REFERENCIA",        OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(pp.Referencia), DBNull.Value, pp.Referencia)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(pagoProveedorId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO_PROVEEDOR.SP_ELIMINAR_PAGO_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PAGO_PROVEEDOR_ID", OracleDbType.Int32).Value = pagoProveedorId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO_PROVEEDOR.SP_LISTAR_PAGOS_PROVEEDOR", conn)
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
                Using cmd As New OracleCommand("PKG_PAGO_PROVEEDOR.SP_BUSCAR_PAGOS_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR",    OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR",   OracleDbType.RefCursor).Direction = ParameterDirection.Output
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
