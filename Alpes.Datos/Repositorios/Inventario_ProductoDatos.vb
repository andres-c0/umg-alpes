Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Inventario

Namespace Repositorios
    Public Class Inventario_ProductoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Inventario_Producto) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_PRODUCTO.SP_INSERTAR_INVENTARIO_PRODUCTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_STOCK", OracleDbType.Int32).Value = entidad.Stock
                    cmd.Parameters.Add("P_STOCK_RESERVADO", OracleDbType.Int32).Value = entidad.StockReservado

                    If entidad.StockMinimo.HasValue Then
                        cmd.Parameters.Add("P_STOCK_MINIMO", OracleDbType.Int32).Value = entidad.StockMinimo.Value
                    Else
                        cmd.Parameters.Add("P_STOCK_MINIMO", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_INV_PROD_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_INV_PROD_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Inventario_Producto)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_PRODUCTO.SP_ACTUALIZAR_INVENTARIO_PRODUCTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_INV_PROD_ID", OracleDbType.Int32).Value = entidad.InvProdId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_STOCK", OracleDbType.Int32).Value = entidad.Stock
                    cmd.Parameters.Add("P_STOCK_RESERVADO", OracleDbType.Int32).Value = entidad.StockReservado

                    If entidad.StockMinimo.HasValue Then
                        cmd.Parameters.Add("P_STOCK_MINIMO", OracleDbType.Int32).Value = entidad.StockMinimo.Value
                    Else
                        cmd.Parameters.Add("P_STOCK_MINIMO", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_PRODUCTO.SP_ELIMINAR_INVENTARIO_PRODUCTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_INV_PROD_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Inventario_Producto
            Dim entidad As Inventario_Producto = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_PRODUCTO.SP_OBTENER_INVENTARIO_PRODUCTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_INV_PROD_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearInventarioProducto(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Inventario_Producto)
            Dim lista As New List(Of Inventario_Producto)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_PRODUCTO.SP_LISTAR_INVENTARIO_PRODUCTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearInventarioProducto(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Inventario_Producto)
            Dim lista As New List(Of Inventario_Producto)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_PRODUCTO.SP_BUSCAR_INVENTARIO_PRODUCTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearInventarioProducto(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearInventarioProducto(ByVal dr As OracleDataReader) As Inventario_Producto
            Dim entidad As New Inventario_Producto()

            If TieneColumna(dr, "INV_PROD_ID") AndAlso Not IsDBNull(dr("INV_PROD_ID")) Then
                entidad.InvProdId = Convert.ToInt32(dr("INV_PROD_ID"))
            End If

            If TieneColumna(dr, "PRODUCTO_ID") AndAlso Not IsDBNull(dr("PRODUCTO_ID")) Then
                entidad.ProductoId = Convert.ToInt32(dr("PRODUCTO_ID"))
            End If

            If TieneColumna(dr, "NOMBRE_PRODUCTO") AndAlso Not IsDBNull(dr("NOMBRE_PRODUCTO")) Then
                entidad.NombreProducto = dr("NOMBRE_PRODUCTO").ToString()
            Else
                entidad.NombreProducto = String.Empty
            End If

            If TieneColumna(dr, "STOCK") AndAlso Not IsDBNull(dr("STOCK")) Then
                entidad.Stock = Convert.ToInt32(dr("STOCK"))
            End If

            If TieneColumna(dr, "STOCK_RESERVADO") AndAlso Not IsDBNull(dr("STOCK_RESERVADO")) Then
                entidad.StockReservado = Convert.ToInt32(dr("STOCK_RESERVADO"))
            End If

            If TieneColumna(dr, "STOCK_MINIMO") AndAlso Not IsDBNull(dr("STOCK_MINIMO")) Then
                entidad.StockMinimo = Convert.ToInt32(dr("STOCK_MINIMO"))
            Else
                entidad.StockMinimo = Nothing
            End If

            If TieneColumna(dr, "CREATED_AT") AndAlso Not IsDBNull(dr("CREATED_AT")) Then
                entidad.CreatedAt = Convert.ToDateTime(dr("CREATED_AT"))
            Else
                entidad.CreatedAt = Nothing
            End If

            If TieneColumna(dr, "UPDATED_AT") AndAlso Not IsDBNull(dr("UPDATED_AT")) Then
                entidad.UpdatedAt = Convert.ToDateTime(dr("UPDATED_AT"))
            Else
                entidad.UpdatedAt = Nothing
            End If

            If TieneColumna(dr, "ESTADO") AndAlso Not IsDBNull(dr("ESTADO")) Then
                entidad.Estado = dr("ESTADO").ToString()
            Else
                entidad.Estado = String.Empty
            End If

            Return entidad
        End Function

        Private Function TieneColumna(ByVal dr As IDataRecord, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If String.Equals(dr.GetName(i), nombreColumna, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            Next

            Return False
        End Function

    End Class
End Namespace