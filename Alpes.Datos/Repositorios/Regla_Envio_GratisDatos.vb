Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Envios

Namespace Repositorios
    Public Class Regla_Envio_GratisDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Regla_Envio_Gratis) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_INSERTAR_REGLA_ENVIO_GRATIS", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ZONA_ENVIO_ID", entidad.zona_envio_id)
                    cmd.Parameters.Add("P_MONTO_MINIMO", entidad.monto_minimo)
                    cmd.Parameters.Add("P_PESO_MAX_KG", entidad.peso_max_kg)
                    cmd.Parameters.Add("P_VIGENCIA_INICIO", entidad.vigencia_inicio)
                    cmd.Parameters.Add("P_VIGENCIA_FIN", If(entidad.vigencia_fin.HasValue, CType(entidad.vigencia_fin.Value, Object), DBNull.Value))

                    cmd.Parameters.Add("P_REGLA_ENVIO_GRATIS_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        Return Convert.ToInt32(cmd.Parameters("P_REGLA_ENVIO_GRATIS_ID").Value)
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Regla_Envio_Gratis)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_ACTUALIZAR_REGLA_ENVIO_GRATIS", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_REGLA_ENVIO_GRATIS_ID", entidad.regla_envio_gratis_id)
                    cmd.Parameters.Add("P_ZONA_ENVIO_ID", entidad.zona_envio_id)
                    cmd.Parameters.Add("P_MONTO_MINIMO", entidad.monto_minimo)
                    cmd.Parameters.Add("P_PESO_MAX_KG", entidad.peso_max_kg)
                    cmd.Parameters.Add("P_VIGENCIA_INICIO", entidad.vigencia_inicio)
                    cmd.Parameters.Add("P_VIGENCIA_FIN", If(entidad.vigencia_fin.HasValue, CType(entidad.vigencia_fin.Value, Object), DBNull.Value))

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
                Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_ELIMINAR_REGLA_ENVIO_GRATIS", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_REGLA_ENVIO_GRATIS_ID", id)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Regla_Envio_Gratis
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_OBTENER_REGLA_ENVIO_GRATIS", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_REGLA_ENVIO_GRATIS_ID", id)
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

        Public Function Listar() As List(Of Regla_Envio_Gratis)
            Dim lista As New List(Of Regla_Envio_Gratis)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_LISTAR_REGLAS_ENVIO_GRATIS", conn)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Regla_Envio_Gratis)
            Dim lista As New List(Of Regla_Envio_Gratis)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_BUSCAR_REGLAS_ENVIO_GRATIS", conn)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Regla_Envio_Gratis
            Dim entidad As New Regla_Envio_Gratis()

            If TieneColumna(dr, "REGLA_ENVIO_GRATIS_ID") AndAlso Not IsDBNull(dr("REGLA_ENVIO_GRATIS_ID")) Then
                entidad.regla_envio_gratis_id = Convert.ToInt32(dr("REGLA_ENVIO_GRATIS_ID"))
            End If

            If TieneColumna(dr, "ZONA_ENVIO_ID") AndAlso Not IsDBNull(dr("ZONA_ENVIO_ID")) Then
                entidad.zona_envio_id = Convert.ToInt32(dr("ZONA_ENVIO_ID"))
            End If

            If TieneColumna(dr, "ZONA_ENVIO") AndAlso Not IsDBNull(dr("ZONA_ENVIO")) Then
                entidad.zona_envio = dr("ZONA_ENVIO").ToString()
            End If

            If TieneColumna(dr, "MONTO_MINIMO") AndAlso Not IsDBNull(dr("MONTO_MINIMO")) Then
                entidad.monto_minimo = Convert.ToDecimal(dr("MONTO_MINIMO"))
            End If

            If TieneColumna(dr, "PESO_MAX_KG") AndAlso Not IsDBNull(dr("PESO_MAX_KG")) Then
                entidad.peso_max_kg = Convert.ToDecimal(dr("PESO_MAX_KG"))
            End If

            If TieneColumna(dr, "VIGENCIA_INICIO") AndAlso Not IsDBNull(dr("VIGENCIA_INICIO")) Then
                entidad.vigencia_inicio = Convert.ToDateTime(dr("VIGENCIA_INICIO"))
            End If

            If TieneColumna(dr, "VIGENCIA_FIN") AndAlso Not IsDBNull(dr("VIGENCIA_FIN")) Then
                entidad.vigencia_fin = Convert.ToDateTime(dr("VIGENCIA_FIN"))
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