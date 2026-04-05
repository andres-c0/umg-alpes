Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class Regla_PromocionDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Regla_Promocion) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_INSERTAR_REGLA_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PROMOCION_ID", entidad.promocion_id)
                    cmd.Parameters.Add("P_MIN_COMPRA", entidad.min_compra)
                    cmd.Parameters.Add("P_MIN_ITEMS", entidad.min_items)
                    cmd.Parameters.Add("P_APLICA_TIPO_PRODUCTO", entidad.aplica_tipo_producto)
                    cmd.Parameters.Add("P_TOPE_DESCUENTO", entidad.tope_descuento)

                    cmd.Parameters.Add("P_REGLA_PROMOCION_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(cmd.Parameters("P_REGLA_PROMOCION_ID").Value)
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Regla_Promocion)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_ACTUALIZAR_REGLA_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_REGLA_PROMOCION_ID", entidad.regla_promocion_id)
                    cmd.Parameters.Add("P_PROMOCION_ID", entidad.promocion_id)
                    cmd.Parameters.Add("P_MIN_COMPRA", entidad.min_compra)
                    cmd.Parameters.Add("P_MIN_ITEMS", entidad.min_items)
                    cmd.Parameters.Add("P_APLICA_TIPO_PRODUCTO", entidad.aplica_tipo_producto)
                    cmd.Parameters.Add("P_TOPE_DESCUENTO", entidad.tope_descuento)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_ELIMINAR_REGLA_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_REGLA_PROMOCION_ID", id)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Regla_Promocion
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_OBTENER_REGLA_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_REGLA_PROMOCION_ID", id)
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

        Public Function Listar() As List(Of Regla_Promocion)
            Dim lista As New List(Of Regla_Promocion)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_LISTAR_REGLAS_PROMOCION", conn)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Regla_Promocion)
            Dim lista As New List(Of Regla_Promocion)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_BUSCAR_REGLAS_PROMOCION", conn)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Regla_Promocion
            Dim entidad As New Regla_Promocion()

            If TieneColumna(dr, "REGLA_PROMOCION_ID") AndAlso Not IsDBNull(dr("REGLA_PROMOCION_ID")) Then
                entidad.regla_promocion_id = Convert.ToInt32(dr("REGLA_PROMOCION_ID"))
            End If

            If TieneColumna(dr, "PROMOCION_ID") AndAlso Not IsDBNull(dr("PROMOCION_ID")) Then
                entidad.promocion_id = Convert.ToInt32(dr("PROMOCION_ID"))
            End If

            If TieneColumna(dr, "PROMOCION") AndAlso Not IsDBNull(dr("PROMOCION")) Then
                entidad.promocion = dr("PROMOCION").ToString()
            End If

            If TieneColumna(dr, "MIN_COMPRA") AndAlso Not IsDBNull(dr("MIN_COMPRA")) Then
                entidad.min_compra = Convert.ToDecimal(dr("MIN_COMPRA"))
            End If

            If TieneColumna(dr, "MIN_ITEMS") AndAlso Not IsDBNull(dr("MIN_ITEMS")) Then
                entidad.min_items = Convert.ToInt32(dr("MIN_ITEMS"))
            End If

            If TieneColumna(dr, "APLICA_TIPO_PRODUCTO") AndAlso Not IsDBNull(dr("APLICA_TIPO_PRODUCTO")) Then
                entidad.aplica_tipo_producto = dr("APLICA_TIPO_PRODUCTO").ToString()
            End If

            If TieneColumna(dr, "TOPE_DESCUENTO") AndAlso Not IsDBNull(dr("TOPE_DESCUENTO")) Then
                entidad.tope_descuento = Convert.ToDecimal(dr("TOPE_DESCUENTO"))
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