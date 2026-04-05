Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class Historial_PromocionDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Historial_Promocion) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_INSERTAR_HISTORIAL_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", entidad.orden_venta_id)
                    cmd.Parameters.Add("P_PROMOCION_ID", entidad.promocion_id)
                    cmd.Parameters.Add("P_MONTO_DESCUENTO_SNAPSHOT", entidad.monto_descuento_snapshot)

                    cmd.Parameters.Add("P_HISTORIAL_PROMOCION_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(cmd.Parameters("P_HISTORIAL_PROMOCION_ID").Value)
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Historial_Promocion)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_ACTUALIZAR_HISTORIAL_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HISTORIAL_PROMOCION_ID", entidad.historial_promocion_id)
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", entidad.orden_venta_id)
                    cmd.Parameters.Add("P_PROMOCION_ID", entidad.promocion_id)
                    cmd.Parameters.Add("P_MONTO_DESCUENTO_SNAPSHOT", entidad.monto_descuento_snapshot)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_ELIMINAR_HISTORIAL_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HISTORIAL_PROMOCION_ID", id)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Historial_Promocion
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_OBTENER_HISTORIAL_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HISTORIAL_PROMOCION_ID", id)
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

        Public Function Listar() As List(Of Historial_Promocion)
            Dim lista As New List(Of Historial_Promocion)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_LISTAR_HISTORIAL_PROMOCION", conn)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Historial_Promocion)
            Dim lista As New List(Of Historial_Promocion)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_BUSCAR_HISTORIAL_PROMOCION", conn)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Historial_Promocion
            Dim entidad As New Historial_Promocion()

            If TieneColumna(dr, "HISTORIAL_PROMOCION_ID") AndAlso Not IsDBNull(dr("HISTORIAL_PROMOCION_ID")) Then
                entidad.historial_promocion_id = Convert.ToInt32(dr("HISTORIAL_PROMOCION_ID"))
            End If

            If TieneColumna(dr, "ORDEN_VENTA_ID") AndAlso Not IsDBNull(dr("ORDEN_VENTA_ID")) Then
                entidad.orden_venta_id = Convert.ToInt32(dr("ORDEN_VENTA_ID"))
            End If

            If TieneColumna(dr, "PROMOCION_ID") AndAlso Not IsDBNull(dr("PROMOCION_ID")) Then
                entidad.promocion_id = Convert.ToInt32(dr("PROMOCION_ID"))
            End If

            If TieneColumna(dr, "MONTO_DESCUENTO_SNAPSHOT") AndAlso Not IsDBNull(dr("MONTO_DESCUENTO_SNAPSHOT")) Then
                entidad.monto_descuento_snapshot = Convert.ToDecimal(dr("MONTO_DESCUENTO_SNAPSHOT"))
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