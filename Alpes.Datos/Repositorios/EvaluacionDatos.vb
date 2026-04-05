Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.RecursosHumanos

Namespace Repositorios
    Public Class EvaluacionDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Evaluacion) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EVALUACION.INSERTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_emp_id", entidad.emp_id)
                    cmd.Parameters.Add("p_evaluador_emp_id", entidad.evaluador_emp_id)
                    cmd.Parameters.Add("p_fecha_eval", If(entidad.fecha_eval.HasValue, CType(entidad.fecha_eval.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("p_puntuacion", entidad.puntuacion)
                    cmd.Parameters.Add("p_comentarios", entidad.comentarios)
                    cmd.Parameters.Add("p_estado", entidad.estado)
                    cmd.Parameters.Add("p_evaluacion_id", OracleDbType.Int32).Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(cmd.Parameters("p_evaluacion_id").Value)
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Evaluacion)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EVALUACION.ACTUALIZAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_evaluacion_id", entidad.evaluacion_id)
                    cmd.Parameters.Add("p_emp_id", entidad.emp_id)
                    cmd.Parameters.Add("p_evaluador_emp_id", entidad.evaluador_emp_id)
                    cmd.Parameters.Add("p_fecha_eval", If(entidad.fecha_eval.HasValue, CType(entidad.fecha_eval.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("p_puntuacion", entidad.puntuacion)
                    cmd.Parameters.Add("p_comentarios", entidad.comentarios)
                    cmd.Parameters.Add("p_estado", entidad.estado)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EVALUACION.ELIMINAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_evaluacion_id", id)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Evaluacion
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EVALUACION.OBTENER_POR_ID", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_evaluacion_id", id)
                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

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

        Public Function Listar() As List(Of Evaluacion)
            Dim lista As New List(Of Evaluacion)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EVALUACION.LISTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

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

        Private Function Mapear(ByVal dr As OracleDataReader) As Evaluacion
            Dim entidad As New Evaluacion()

            entidad.evaluacion_id = Convert.ToInt32(dr("evaluacion_id"))
            entidad.emp_id = Convert.ToInt32(dr("emp_id"))
            entidad.evaluador_emp_id = Convert.ToInt32(dr("evaluador_emp_id"))
            entidad.fecha_eval = If(IsDBNull(dr("fecha_eval")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("fecha_eval")))
            entidad.puntuacion = Convert.ToDecimal(dr("puntuacion"))
            entidad.comentarios = dr("comentarios").ToString()
            entidad.estado = dr("estado").ToString()
            entidad.created_at = If(IsDBNull(dr("created_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("created_at")))
            entidad.updated_at = If(IsDBNull(dr("updated_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("updated_at")))

            Return entidad
        End Function

    End Class
End Namespace