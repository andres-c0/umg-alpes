Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class Factura_DetalleDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Factura_Detalle) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_INSERTAR_FACTURA_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_ID", entidad.factura_id)
                    cmd.Parameters.Add("P_PRODUCTO_ID", entidad.producto_id)
                    cmd.Parameters.Add("P_CANTIDAD", entidad.cantidad)
                    cmd.Parameters.Add("P_PRECIO_UNITARIO_SNAPSHOT", entidad.precio_unitario_snapshot)
                    cmd.Parameters.Add("P_TOTAL_LINEA", entidad.total_linea)

                    cmd.Parameters.Add("P_FACTURA_DET_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        Return Convert.ToInt32(cmd.Parameters("P_FACTURA_DET_ID").Value)
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Factura_Detalle)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_ACTUALIZAR_FACTURA_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_DET_ID", entidad.factura_det_id)
                    cmd.Parameters.Add("P_FACTURA_ID", entidad.factura_id)
                    cmd.Parameters.Add("P_PRODUCTO_ID", entidad.producto_id)
                    cmd.Parameters.Add("P_CANTIDAD", entidad.cantidad)
                    cmd.Parameters.Add("P_PRECIO_UNITARIO_SNAPSHOT", entidad.precio_unitario_snapshot)
                    cmd.Parameters.Add("P_TOTAL_LINEA", entidad.total_linea)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_ELIMINAR_FACTURA_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_DET_ID", id)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Factura_Detalle
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_OBTENER_FACTURA_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_DET_ID", id)
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

        Public Function Listar() As List(Of Factura_Detalle)
            Dim lista As New List(Of Factura_Detalle)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_LISTAR_FACTURA_DETALLE", conn)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Factura_Detalle)
            Dim lista As New List(Of Factura_Detalle)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_BUSCAR_FACTURA_DETALLE", conn)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Factura_Detalle
            Dim entidad As New Factura_Detalle()

            If TieneColumna(dr, "FACTURA_DET_ID") AndAlso Not IsDBNull(dr("FACTURA_DET_ID")) Then
                entidad.factura_det_id = Convert.ToInt32(dr("FACTURA_DET_ID"))
            End If

            If TieneColumna(dr, "FACTURA_ID") AndAlso Not IsDBNull(dr("FACTURA_ID")) Then
                entidad.factura_id = Convert.ToInt32(dr("FACTURA_ID"))
            End If

            If TieneColumna(dr, "PRODUCTO_ID") AndAlso Not IsDBNull(dr("PRODUCTO_ID")) Then
                entidad.producto_id = Convert.ToInt32(dr("PRODUCTO_ID"))
            End If

            If TieneColumna(dr, "CANTIDAD") AndAlso Not IsDBNull(dr("CANTIDAD")) Then
                entidad.cantidad = Convert.ToInt32(dr("CANTIDAD"))
            End If

            If TieneColumna(dr, "PRECIO_UNITARIO_SNAPSHOT") AndAlso Not IsDBNull(dr("PRECIO_UNITARIO_SNAPSHOT")) Then
                entidad.precio_unitario_snapshot = Convert.ToDecimal(dr("PRECIO_UNITARIO_SNAPSHOT"))
            End If

            If TieneColumna(dr, "TOTAL_LINEA") AndAlso Not IsDBNull(dr("TOTAL_LINEA")) Then
                entidad.total_linea = Convert.ToDecimal(dr("TOTAL_LINEA"))
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
