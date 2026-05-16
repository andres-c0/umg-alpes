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
                        cmd.Parameters.Add("P_COMENTARIO", OracleDbType.Varchar2).Value = entidad.Comentario.Trim()
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
                        cmd.Parameters.Add("P_COMENTARIO", OracleDbType.Varchar2).Value = entidad.Comentario.Trim()
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
            Dim lista As New List(Of ResenaComentario)()

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
            Return Buscar("CLI_ID", valor)
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ResenaComentario)
            Dim lista As New List(Of ResenaComentario)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_BUSCAR_RESENA_COMENTARIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(criterio), "CLI_ID", criterio.Trim().ToUpperInvariant())
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = If(valor, String.Empty).Trim()
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
            Dim entidad As New ResenaComentario()

            entidad.ResenaId = LeerEntero(dr, "RESENA_ID")
            entidad.CliId = LeerEntero(dr, "CLI_ID")
            entidad.ProductoId = LeerEntero(dr, "PRODUCTO_ID")
            entidad.Calificacion = LeerDecimalNullable(dr, "CALIFICACION")
            entidad.ResenaAt = LeerFecha(dr, "RESENA_AT", DateTime.MinValue)
            entidad.Estado = LeerTexto(dr, "ESTADO")

            entidad.Comentario = LeerTexto(dr, "COMENTARIO")
            entidad.CreatedAt = LeerFechaNullable(dr, "CREATED_AT")
            entidad.UpdatedAt = LeerFechaNullable(dr, "UPDATED_AT")

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

        Private Function LeerTexto(ByVal dr As OracleDataReader, ByVal columna As String) As String
            If Not TieneColumna(dr, columna) OrElse IsDBNull(dr(columna)) Then
                Return String.Empty
            End If

            Return dr(columna).ToString()
        End Function

        Private Function LeerEntero(ByVal dr As OracleDataReader, ByVal columna As String) As Integer
            If Not TieneColumna(dr, columna) OrElse IsDBNull(dr(columna)) Then
                Return 0
            End If

            Return Convert.ToInt32(dr(columna))
        End Function

        Private Function LeerDecimalNullable(ByVal dr As OracleDataReader, ByVal columna As String) As Decimal?
            If Not TieneColumna(dr, columna) OrElse IsDBNull(dr(columna)) Then
                Return Nothing
            End If

            Return Convert.ToDecimal(dr(columna))
        End Function

        Private Function LeerFecha(ByVal dr As OracleDataReader, ByVal columna As String, ByVal valorDefecto As DateTime) As DateTime
            If Not TieneColumna(dr, columna) OrElse IsDBNull(dr(columna)) Then
                Return valorDefecto
            End If

            Return Convert.ToDateTime(dr(columna))
        End Function

        Private Function LeerFechaNullable(ByVal dr As OracleDataReader, ByVal columna As String) As DateTime?
            If Not TieneColumna(dr, columna) OrElse IsDBNull(dr(columna)) Then
                Return Nothing
            End If

            Return Convert.ToDateTime(dr(columna))
        End Function

    End Class
End Namespace