Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class ResenaComentarioDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As ResenaComentario) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_INSERTAR_RESENA_COMENTARIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId

                    If entidad.Calificacion.HasValue Then
                        cmd.Parameters.Add("P_CALIFICACION", OracleDbType.Decimal).Value = entidad.Calificacion.Value
                    Else
                        cmd.Parameters.Add("P_CALIFICACION", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Comentario) Then
                        cmd.Parameters.Add("P_COMENTARIO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_COMENTARIO", OracleDbType.Varchar2).Value = entidad.Comentario
                    End If

                    cmd.Parameters.Add("P_RESENA_AT", OracleDbType.TimeStamp).Value = entidad.ResenaAt
                    cmd.Parameters.Add("P_RESENA_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_RESENA_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As ResenaComentario)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_ACTUALIZAR_RESENA_COMENTARIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_RESENA_ID", OracleDbType.Int32).Value = entidad.ResenaId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId

                    If entidad.Calificacion.HasValue Then
                        cmd.Parameters.Add("P_CALIFICACION", OracleDbType.Decimal).Value = entidad.Calificacion.Value
                    Else
                        cmd.Parameters.Add("P_CALIFICACION", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Comentario) Then
                        cmd.Parameters.Add("P_COMENTARIO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_COMENTARIO", OracleDbType.Varchar2).Value = entidad.Comentario
                    End If

                    cmd.Parameters.Add("P_RESENA_AT", OracleDbType.TimeStamp).Value = entidad.ResenaAt

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_ELIMINAR_RESENA_COMENTARIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_RESENA_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As ResenaComentario
            Dim entidad As ResenaComentario = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_OBTENER_RESENA_COMENTARIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_RESENA_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = Mapear(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of ResenaComentario)
            Dim lista As New List(Of ResenaComentario)

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_LISTAR_RESENA_COMENTARIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of ResenaComentario)
            Dim lista As New List(Of ResenaComentario)

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_BUSCAR_RESENA_COMENTARIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = "CLI_ID"
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function Mapear(ByVal dr As OracleDataReader) As ResenaComentario
            Dim entidad As New ResenaComentario With {
                .ResenaId = Convert.ToInt32(dr("RESENA_ID")),
                .CliId = Convert.ToInt32(dr("CLI_ID")),
                .ProductoId = Convert.ToInt32(dr("PRODUCTO_ID")),
                .ResenaAt = Convert.ToDateTime(dr("RESENA_AT")),
                .Estado = dr("ESTADO").ToString()
            }

            If Not IsDBNull(dr("CALIFICACION")) Then
                entidad.Calificacion = Convert.ToDecimal(dr("CALIFICACION"))
            Else
                entidad.Calificacion = Nothing
            End If

            If Not IsDBNull(dr("COMENTARIO")) Then
                entidad.Comentario = dr("COMENTARIO").ToString()
            Else
                entidad.Comentario = Nothing
            End If

            If Not IsDBNull(dr("CREATED_AT")) Then
                entidad.CreatedAt = Convert.ToDateTime(dr("CREATED_AT"))
            Else
                entidad.CreatedAt = Nothing
            End If

            If Not IsDBNull(dr("UPDATED_AT")) Then
                entidad.UpdatedAt = Convert.ToDateTime(dr("UPDATED_AT"))
            Else
                entidad.UpdatedAt = Nothing
            End If

            Return entidad
        End Function

    End Class
End Namespace