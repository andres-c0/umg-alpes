Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class InventarioProductoDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(inventario As InventarioProducto) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_PRODUCTO.SP_INSERTAR_INVENTARIO_PRODUCTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PRODUCTO_ID",     OracleDbType.Int32).Value = inventario.ProductoId
                    cmd.Parameters.Add("P_STOCK",           OracleDbType.Int32).Value = inventario.Stock
                    cmd.Parameters.Add("P_STOCK_RESERVADO", OracleDbType.Int32).Value = inventario.StockReservado
                    cmd.Parameters.Add("P_STOCK_MINIMO",    OracleDbType.Int32).Value = If(inventario.StockMinimo.HasValue, inventario.StockMinimo.Value, DBNull.Value)

                    Dim pOut As New OracleParameter("P_INV_PROD_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(inventario As InventarioProducto)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_PRODUCTO.SP_ACTUALIZAR_INVENTARIO_PRODUCTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_INV_PROD_ID",     OracleDbType.Int32).Value = inventario.InvProdId
                    cmd.Parameters.Add("P_PRODUCTO_ID",     OracleDbType.Int32).Value = inventario.ProductoId
                    cmd.Parameters.Add("P_STOCK",           OracleDbType.Int32).Value = inventario.Stock
                    cmd.Parameters.Add("P_STOCK_RESERVADO", OracleDbType.Int32).Value = inventario.StockReservado
                    cmd.Parameters.Add("P_STOCK_MINIMO",    OracleDbType.Int32).Value = If(inventario.StockMinimo.HasValue, inventario.StockMinimo.Value, DBNull.Value)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(invProdId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_PRODUCTO.SP_ELIMINAR_INVENTARIO_PRODUCTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_INV_PROD_ID", OracleDbType.Int32).Value = invProdId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_PRODUCTO.SP_LISTAR_INVENTARIO_PRODUCTO", conn)
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
                Using cmd As New OracleCommand("PKG_INVENTARIO_PRODUCTO.SP_BUSCAR_INVENTARIO_PRODUCTO", conn)
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
