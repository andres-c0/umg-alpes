Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class Resena_ComentarioDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Resena_Comentario) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_INSERTAR_RESENA_COMENTARIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", entidad.cli_id)
                    cmd.Parameters.Add("P_PRODUCTO_ID", entidad.producto_id)
                    cmd.Parameters.Add("P_CALIFICACION", entidad.calificacion)
                    cmd.Parameters.Add("P_COMENTARIO", entidad.comentario)
                    cmd.Parameters.Add("P_RESENA_AT", entidad.resena_at)

                    cmd.Parameters.Add("P_RESENA_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        Return Convert.ToInt32(cmd.Parameters("P_RESENA_ID").Value)
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Resena_Comentario)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_ACTUALIZAR_RESENA_COMENTARIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_RESENA_ID", entidad.resena_id)
                    cmd.Parameters.Add("P_CLI_ID", entidad.cli_id)
                    cmd.Parameters.Add("P_PRODUCTO_ID", entidad.producto_id)
                    cmd.Parameters.Add("P_CALIFICACION", entidad.calificacion)
                    cmd.Parameters.Add("P_COMENTARIO", entidad.comentario)
                    cmd.Parameters.Add("P_RESENA_AT", entidad.resena_at)

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
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_ELIMINAR_RESENA_COMENTARIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_RESENA_ID", id)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Resena_Comentario
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_OBTENER_RESENA_COMENTARIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_RESENA_ID", id)
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

        Public Function Listar() As List(Of Resena_Comentario)
            Dim lista As New List(Of Resena_Comentario)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_LISTAR_RESENA_COMENTARIO", conn)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Resena_Comentario)
            Dim lista As New List(Of Resena_Comentario)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_BUSCAR_RESENA_COMENTARIO", conn)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Resena_Comentario
            Dim entidad As New Resena_Comentario()

            If TieneColumna(dr, "RESENA_ID") AndAlso Not IsDBNull(dr("RESENA_ID")) Then
                entidad.resena_id = Convert.ToInt32(dr("RESENA_ID"))
            End If

            If TieneColumna(dr, "CLI_ID") AndAlso Not IsDBNull(dr("CLI_ID")) Then
                entidad.cli_id = Convert.ToInt32(dr("CLI_ID"))
            End If

            If TieneColumna(dr, "PRODUCTO_ID") AndAlso Not IsDBNull(dr("PRODUCTO_ID")) Then
                entidad.producto_id = Convert.ToInt32(dr("PRODUCTO_ID"))
            End If

            If TieneColumna(dr, "CALIFICACION") AndAlso Not IsDBNull(dr("CALIFICACION")) Then
                entidad.calificacion = Convert.ToInt32(dr("CALIFICACION"))
            End If

            If TieneColumna(dr, "COMENTARIO") AndAlso Not IsDBNull(dr("COMENTARIO")) Then
                entidad.comentario = dr("COMENTARIO").ToString()
            End If

            If TieneColumna(dr, "RESENA_AT") AndAlso Not IsDBNull(dr("RESENA_AT")) Then
                entidad.resena_at = Convert.ToDateTime(dr("RESENA_AT"))
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
