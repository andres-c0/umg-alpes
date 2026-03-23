Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class CuentaPagarProveedorDatos
        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(cpp As CuentaPagarProveedor) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUENTA_PAGAR_PROVEEDOR.SP_INSERTAR_CUENTA_PAGAR_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PROV_ID",           OracleDbType.Int32).Value    = cpp.ProvId
                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID",   OracleDbType.Int32).Value    = cpp.OrdenCompraId
                    cmd.Parameters.Add("P_SALDO",             OracleDbType.Decimal).Value  = cpp.Saldo
                    cmd.Parameters.Add("P_FECHA_VENCIMIENTO", OracleDbType.Date).Value     = If(cpp.FechaVencimiento = Nothing, DBNull.Value, cpp.FechaVencimiento)
                    cmd.Parameters.Add("P_ESTADO_CP",         OracleDbType.Varchar2).Value = cpp.EstadoCp
                    Dim pOut As New OracleParameter("P_CUENTA_PAGAR_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(cpp As CuentaPagarProveedor)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUENTA_PAGAR_PROVEEDOR.SP_ACTUALIZAR_CUENTA_PAGAR_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CUENTA_PAGAR_ID",   OracleDbType.Int32).Value    = cpp.CuentaPagarId
                    cmd.Parameters.Add("P_PROV_ID",           OracleDbType.Int32).Value    = cpp.ProvId
                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID",   OracleDbType.Int32).Value    = cpp.OrdenCompraId
                    cmd.Parameters.Add("P_SALDO",             OracleDbType.Decimal).Value  = cpp.Saldo
                    cmd.Parameters.Add("P_FECHA_VENCIMIENTO", OracleDbType.Date).Value     = If(cpp.FechaVencimiento = Nothing, DBNull.Value, cpp.FechaVencimiento)
                    cmd.Parameters.Add("P_ESTADO_CP",         OracleDbType.Varchar2).Value = cpp.EstadoCp
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(cuentaPagarId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUENTA_PAGAR_PROVEEDOR.SP_ELIMINAR_CUENTA_PAGAR_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CUENTA_PAGAR_ID", OracleDbType.Int32).Value = cuentaPagarId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUENTA_PAGAR_PROVEEDOR.SP_LISTAR_CUENTAS_PAGAR_PROVEEDOR", conn)
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
                Using cmd As New OracleCommand("PKG_CUENTA_PAGAR_PROVEEDOR.SP_BUSCAR_CUENTAS_PAGAR_PROVEEDOR", conn)
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
