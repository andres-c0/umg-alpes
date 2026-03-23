Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class OrdenCompraDetalleDatos
        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(det As OrdenCompraDetalle) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA_DETALLE.SP_INSERTAR_ORDEN_COMPRA_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID", OracleDbType.Int32).Value   = det.OrdenCompraId
                    cmd.Parameters.Add("P_MP_ID",           OracleDbType.Int32).Value   = det.MpId
                    cmd.Parameters.Add("P_CANTIDAD",        OracleDbType.Decimal).Value = det.Cantidad
                    cmd.Parameters.Add("P_COSTO_UNITARIO",  OracleDbType.Decimal).Value = det.CostoUnitario
                    cmd.Parameters.Add("P_SUBTOTAL_LINEA",  OracleDbType.Decimal).Value = det.SubtotalLinea
                    Dim pOut As New OracleParameter("P_ORDEN_COMPRA_DET_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(det As OrdenCompraDetalle)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA_DETALLE.SP_ACTUALIZAR_ORDEN_COMPRA_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ORDEN_COMPRA_DET_ID", OracleDbType.Int32).Value   = det.OrdenCompraDetId
                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID",     OracleDbType.Int32).Value   = det.OrdenCompraId
                    cmd.Parameters.Add("P_MP_ID",               OracleDbType.Int32).Value   = det.MpId
                    cmd.Parameters.Add("P_CANTIDAD",            OracleDbType.Decimal).Value = det.Cantidad
                    cmd.Parameters.Add("P_COSTO_UNITARIO",      OracleDbType.Decimal).Value = det.CostoUnitario
                    cmd.Parameters.Add("P_SUBTOTAL_LINEA",      OracleDbType.Decimal).Value = det.SubtotalLinea
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ordenCompraDetId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA_DETALLE.SP_ELIMINAR_ORDEN_COMPRA_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ORDEN_COMPRA_DET_ID", OracleDbType.Int32).Value = ordenCompraDetId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA_DETALLE.SP_LISTAR_ORDEN_COMPRA_DETALLE", conn)
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
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA_DETALLE.SP_BUSCAR_ORDEN_COMPRA_DETALLE", conn)
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
