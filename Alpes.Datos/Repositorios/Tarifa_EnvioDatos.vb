Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class Tarifa_EnvioDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Tarifa_Envio) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_INSERTAR_TARIFA_ENVIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ZONA_ENVIO_ID", entidad.zona_envio_id)
                    cmd.Parameters.Add("P_TIPO_ENTREGA_ID", entidad.tipo_entrega_id)
                    cmd.Parameters.Add("P_PESO_DESDE_KG", entidad.peso_desde_kg)
                    cmd.Parameters.Add("P_PESO_HASTA_KG", entidad.peso_hasta_kg)
                    cmd.Parameters.Add("P_COSTO", entidad.costo)

                    cmd.Parameters.Add("P_TARIFA_ENVIO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        Return Convert.ToInt32(cmd.Parameters("P_TARIFA_ENVIO_ID").Value)
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Tarifa_Envio)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_ACTUALIZAR_TARIFA_ENVIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_TARIFA_ENVIO_ID", entidad.tarifa_envio_id)
                    cmd.Parameters.Add("P_ZONA_ENVIO_ID", entidad.zona_envio_id)
                    cmd.Parameters.Add("P_TIPO_ENTREGA_ID", entidad.tipo_entrega_id)
                    cmd.Parameters.Add("P_PESO_DESDE_KG", entidad.peso_desde_kg)
                    cmd.Parameters.Add("P_PESO_HASTA_KG", entidad.peso_hasta_kg)
                    cmd.Parameters.Add("P_COSTO", entidad.costo)

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
                Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_ELIMINAR_TARIFA_ENVIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_TARIFA_ENVIO_ID", id)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Tarifa_Envio
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_OBTENER_TARIFA_ENVIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_TARIFA_ENVIO_ID", id)
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

        Public Function Listar() As List(Of Tarifa_Envio)
            Dim lista As New List(Of Tarifa_Envio)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_LISTAR_TARIFAS_ENVIO", conn)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Tarifa_Envio)
            Dim lista As New List(Of Tarifa_Envio)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_BUSCAR_TARIFAS_ENVIO", conn)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Tarifa_Envio
            Dim entidad As New Tarifa_Envio()

            If TieneColumna(dr, "TARIFA_ENVIO_ID") AndAlso Not IsDBNull(dr("TARIFA_ENVIO_ID")) Then
                entidad.tarifa_envio_id = Convert.ToInt32(dr("TARIFA_ENVIO_ID"))
            End If

            If TieneColumna(dr, "ZONA_ENVIO_ID") AndAlso Not IsDBNull(dr("ZONA_ENVIO_ID")) Then
                entidad.zona_envio_id = Convert.ToInt32(dr("ZONA_ENVIO_ID"))
            End If

            If TieneColumna(dr, "ZONA_ENVIO") AndAlso Not IsDBNull(dr("ZONA_ENVIO")) Then
                entidad.zona_envio = dr("ZONA_ENVIO").ToString()
            End If

            If TieneColumna(dr, "TIPO_ENTREGA_ID") AndAlso Not IsDBNull(dr("TIPO_ENTREGA_ID")) Then
                entidad.tipo_entrega_id = Convert.ToInt32(dr("TIPO_ENTREGA_ID"))
            End If

            If TieneColumna(dr, "TIPO_ENTREGA") AndAlso Not IsDBNull(dr("TIPO_ENTREGA")) Then
                entidad.tipo_entrega = dr("TIPO_ENTREGA").ToString()
            End If

            If TieneColumna(dr, "PESO_DESDE_KG") AndAlso Not IsDBNull(dr("PESO_DESDE_KG")) Then
                entidad.peso_desde_kg = Convert.ToDecimal(dr("PESO_DESDE_KG"))
            End If

            If TieneColumna(dr, "PESO_HASTA_KG") AndAlso Not IsDBNull(dr("PESO_HASTA_KG")) Then
                entidad.peso_hasta_kg = Convert.ToDecimal(dr("PESO_HASTA_KG"))
            End If

            If TieneColumna(dr, "COSTO") AndAlso Not IsDBNull(dr("COSTO")) Then
                entidad.costo = Convert.ToDecimal(dr("COSTO"))
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