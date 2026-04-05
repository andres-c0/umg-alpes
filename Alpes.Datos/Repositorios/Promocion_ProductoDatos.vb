Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class Promocion_ProductoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Promocion_Producto) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION_PRODUCTO.SP_INSERTAR_PROMOCION_PRODUCTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PROMOCION_ID", entidad.promocion_id)
                    cmd.Parameters.Add("P_PRODUCTO_ID", entidad.producto_id)
                    cmd.Parameters.Add("P_LIMITE_UNIDADES", entidad.limite_unidades)

                    cmd.Parameters.Add("P_PROMOCION_PRODUCTO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(cmd.Parameters("P_PROMOCION_PRODUCTO_ID").Value)
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Promocion_Producto)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION_PRODUCTO.SP_ACTUALIZAR_PROMOCION_PRODUCTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PROMOCION_PRODUCTO_ID", entidad.promocion_producto_id)
                    cmd.Parameters.Add("P_PROMOCION_ID", entidad.promocion_id)
                    cmd.Parameters.Add("P_PRODUCTO_ID", entidad.producto_id)
                    cmd.Parameters.Add("P_LIMITE_UNIDADES", entidad.limite_unidades)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION_PRODUCTO.SP_ELIMINAR_PROMOCION_PRODUCTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PROMOCION_PRODUCTO_ID", id)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Promocion_Producto
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION_PRODUCTO.SP_OBTENER_PROMOCION_PRODUCTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PROMOCION_PRODUCTO_ID", id)
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            Return Mapear(dr)
                        End If
                    End Using
                End Using
            End Using

            Return Nothing
        End Function

        Public Function Listar() As List(Of Promocion_Producto)
            Dim lista As New List(Of Promocion_Producto)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION_PRODUCTO.SP_LISTAR_PROMOCION_PRODUCTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Promocion_Producto)
            Dim lista As New List(Of Promocion_Producto)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION_PRODUCTO.SP_BUSCAR_PROMOCION_PRODUCTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", criterio)
                    cmd.Parameters.Add("P_VALOR", valor)
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function Mapear(ByVal dr As OracleDataReader) As Promocion_Producto
            Dim entidad As New Promocion_Producto()

            If TieneColumna(dr, "PROMOCION_PRODUCTO_ID") AndAlso Not IsDBNull(dr("PROMOCION_PRODUCTO_ID")) Then
                entidad.promocion_producto_id = Convert.ToInt32(dr("PROMOCION_PRODUCTO_ID"))
            End If

            If TieneColumna(dr, "PROMOCION_ID") AndAlso Not IsDBNull(dr("PROMOCION_ID")) Then
                entidad.promocion_id = Convert.ToInt32(dr("PROMOCION_ID"))
            End If

            If TieneColumna(dr, "PROMOCION") AndAlso Not IsDBNull(dr("PROMOCION")) Then
                entidad.promocion = dr("PROMOCION").ToString()
            End If

            If TieneColumna(dr, "PRODUCTO_ID") AndAlso Not IsDBNull(dr("PRODUCTO_ID")) Then
                entidad.producto_id = Convert.ToInt32(dr("PRODUCTO_ID"))
            End If

            If TieneColumna(dr, "REFERENCIA") AndAlso Not IsDBNull(dr("REFERENCIA")) Then
                entidad.referencia = dr("REFERENCIA").ToString()
            End If

            If TieneColumna(dr, "PRODUCTO") AndAlso Not IsDBNull(dr("PRODUCTO")) Then
                entidad.producto = dr("PRODUCTO").ToString()
            End If

            If TieneColumna(dr, "LIMITE_UNIDADES") AndAlso Not IsDBNull(dr("LIMITE_UNIDADES")) Then
                entidad.limite_unidades = Convert.ToInt32(dr("LIMITE_UNIDADES"))
            End If

            If TieneColumna(dr, "CREATED_AT") AndAlso Not IsDBNull(dr("CREATED_AT")) Then
                entidad.created_at = Convert.ToDateTime(dr("CREATED_AT"))
            End If

            If TieneColumna(dr, "UPDATED_AT") AndAlso Not IsDBNull(dr("UPDATED_AT")) Then
                entidad.updated_at = Convert.ToDateTime(dr("UPDATED_AT"))
            End If

            If TieneColumna(dr, "ESTADO") AndAlso Not IsDBNull(dr("ESTADO")) Then
                entidad.estado = dr("ESTADO").ToString()
            End If

            Return entidad
        End Function

        Private Function TieneColumna(ByVal dr As OracleDataReader, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If String.Equals(dr.GetName(i), nombreColumna, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            Next
            Return False
        End Function

    End Class
End Namespace