Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class OrdenCompraDatos
        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(oc As OrdenCompra) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA.SP_INSERTAR_ORDEN_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_NUM_OC",            OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(oc.NumOc), DBNull.Value, oc.NumOc)
                    cmd.Parameters.Add("P_PROV_ID",           OracleDbType.Int32).Value    = oc.ProvId
                    cmd.Parameters.Add("P_ESTADO_OC_ID",      OracleDbType.Int32).Value    = oc.EstadoOcId
                    cmd.Parameters.Add("P_CONDICION_PAGO_ID", OracleDbType.Int32).Value    = oc.CondicionPagoId
                    cmd.Parameters.Add("P_FECHA_OC",          OracleDbType.Date).Value     = oc.FechaOc
                    cmd.Parameters.Add("P_SUBTOTAL",          OracleDbType.Decimal).Value  = oc.Subtotal
                    cmd.Parameters.Add("P_IMPUESTO",          OracleDbType.Decimal).Value  = oc.Impuesto
                    cmd.Parameters.Add("P_TOTAL",             OracleDbType.Decimal).Value  = oc.Total
                    cmd.Parameters.Add("P_OBSERVACIONES",     OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(oc.Observaciones), DBNull.Value, oc.Observaciones)
                    Dim pOut As New OracleParameter("P_ORDEN_COMPRA_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(oc As OrdenCompra)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA.SP_ACTUALIZAR_ORDEN_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID",   OracleDbType.Int32).Value    = oc.OrdenCompraId
                    cmd.Parameters.Add("P_NUM_OC",            OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(oc.NumOc), DBNull.Value, oc.NumOc)
                    cmd.Parameters.Add("P_PROV_ID",           OracleDbType.Int32).Value    = oc.ProvId
                    cmd.Parameters.Add("P_ESTADO_OC_ID",      OracleDbType.Int32).Value    = oc.EstadoOcId
                    cmd.Parameters.Add("P_CONDICION_PAGO_ID", OracleDbType.Int32).Value    = oc.CondicionPagoId
                    cmd.Parameters.Add("P_FECHA_OC",          OracleDbType.Date).Value     = oc.FechaOc
                    cmd.Parameters.Add("P_SUBTOTAL",          OracleDbType.Decimal).Value  = oc.Subtotal
                    cmd.Parameters.Add("P_IMPUESTO",          OracleDbType.Decimal).Value  = oc.Impuesto
                    cmd.Parameters.Add("P_TOTAL",             OracleDbType.Decimal).Value  = oc.Total
                    cmd.Parameters.Add("P_OBSERVACIONES",     OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(oc.Observaciones), DBNull.Value, oc.Observaciones)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ordenCompraId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA.SP_ELIMINAR_ORDEN_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID", OracleDbType.Int32).Value = ordenCompraId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA.SP_LISTAR_ORDENES_COMPRA", conn)
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
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA.SP_BUSCAR_ORDENES_COMPRA", conn)
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
