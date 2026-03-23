Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class ResenaComentarioDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(resenaComentario As ResenaComentario) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_INSERTAR_RESENA_COMENTARIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = resenaComentario.CliId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = resenaComentario.ProductoId
                    cmd.Parameters.Add("P_CALIFICACION", OracleDbType.Int32).Value = resenaComentario.Calificacion
                    cmd.Parameters.Add("P_COMENTARIO", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(resenaComentario.Comentario), DBNull.Value, CType(resenaComentario.Comentario, Object))
                    cmd.Parameters.Add("P_RESENA_AT", OracleDbType.TimeStamp).Value = resenaComentario.ResenaAt

                    Dim pOut As New OracleParameter("P_RESENA_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(resenaComentario As ResenaComentario)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_ACTUALIZAR_RESENA_COMENTARIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_RESENA_ID", OracleDbType.Int32).Value = resenaComentario.ResenaId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = resenaComentario.CliId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = resenaComentario.ProductoId
                    cmd.Parameters.Add("P_CALIFICACION", OracleDbType.Int32).Value = resenaComentario.Calificacion
                    cmd.Parameters.Add("P_COMENTARIO", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(resenaComentario.Comentario), DBNull.Value, CType(resenaComentario.Comentario, Object))
                    cmd.Parameters.Add("P_RESENA_AT", OracleDbType.TimeStamp).Value = resenaComentario.ResenaAt

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(resenaComentarioId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_ELIMINAR_RESENA_COMENTARIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_RESENA_ID", OracleDbType.Int32).Value = resenaComentarioId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_LISTAR_RESENA_COMENTARIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output
                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

        Public Function Buscar(criterio As String, valor As String) As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RESENA_COMENTARIO.SP_BUSCAR_RESENA_COMENTARIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output
                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

    End Class

End Namespace
