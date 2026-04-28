Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.RRHH

Namespace Repositorios
    Public Class EvaluacionDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Evaluacion) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EVALUACION.INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    If entidad.EmpId.HasValue Then
                        cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = entidad.EmpId.Value
                    Else
                        cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.EvaluadorEmpId.HasValue Then
                        cmd.Parameters.Add("p_evaluador_emp_id", OracleDbType.Int32).Value = entidad.EvaluadorEmpId.Value
                    Else
                        cmd.Parameters.Add("p_evaluador_emp_id", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.FechaEval.HasValue Then
                        cmd.Parameters.Add("p_fecha_eval", OracleDbType.Date).Value = entidad.FechaEval.Value
                    Else
                        cmd.Parameters.Add("p_fecha_eval", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If entidad.Puntuacion.HasValue Then
                        cmd.Parameters.Add("p_puntuacion", OracleDbType.Decimal).Value = entidad.Puntuacion.Value
                    Else
                        cmd.Parameters.Add("p_puntuacion", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Comentarios) Then
                        cmd.Parameters.Add("p_comentarios", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_comentarios", OracleDbType.Varchar2).Value = entidad.Comentarios
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Estado) Then
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = entidad.Estado
                    End If

                    cmd.Parameters.Add("p_evaluacion_id", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("p_evaluacion_id").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Evaluacion)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EVALUACION.ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_evaluacion_id", OracleDbType.Int32).Value = entidad.EvaluacionId

                    If entidad.EmpId.HasValue Then
                        cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = entidad.EmpId.Value
                    Else
                        cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.EvaluadorEmpId.HasValue Then
                        cmd.Parameters.Add("p_evaluador_emp_id", OracleDbType.Int32).Value = entidad.EvaluadorEmpId.Value
                    Else
                        cmd.Parameters.Add("p_evaluador_emp_id", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.FechaEval.HasValue Then
                        cmd.Parameters.Add("p_fecha_eval", OracleDbType.Date).Value = entidad.FechaEval.Value
                    Else
                        cmd.Parameters.Add("p_fecha_eval", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If entidad.Puntuacion.HasValue Then
                        cmd.Parameters.Add("p_puntuacion", OracleDbType.Decimal).Value = entidad.Puntuacion.Value
                    Else
                        cmd.Parameters.Add("p_puntuacion", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Comentarios) Then
                        cmd.Parameters.Add("p_comentarios", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_comentarios", OracleDbType.Varchar2).Value = entidad.Comentarios
                    End If

                    cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = entidad.Estado

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EVALUACION.ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_evaluacion_id", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Evaluacion
            Dim entidad As Evaluacion = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EVALUACION.OBTENER_POR_ID", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_evaluacion_id", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearEvaluacion(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Evaluacion)
            Dim lista As New List(Of Evaluacion)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EVALUACION.LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearEvaluacion(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearEvaluacion(ByVal dr As OracleDataReader) As Evaluacion
            Dim entidad As New Evaluacion()

            entidad.EvaluacionId = LeerEntero(dr, "EVALUACION_ID")
            entidad.EmpId = LeerEnteroNullable(dr, "EMP_ID")
            entidad.EvaluadorEmpId = LeerEnteroNullable(dr, "EVALUADOR_EMP_ID")
            entidad.FechaEval = LeerFechaNullable(dr, "FECHA_EVAL")
            entidad.Puntuacion = LeerDecimalNullable(dr, "PUNTUACION")
            entidad.Comentarios = LeerTexto(dr, "COMENTARIOS")
            entidad.CreatedAt = LeerFechaNullable(dr, "CREATED_AT")
            entidad.UpdatedAt = LeerFechaNullable(dr, "UPDATED_AT")
            entidad.Estado = LeerTexto(dr, "ESTADO")

            Return entidad
        End Function

        Private Function LeerTexto(ByVal dr As OracleDataReader, ByVal columna As String) As String
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return dr(columna).ToString()
            End If

            Return String.Empty
        End Function

        Private Function LeerEntero(ByVal dr As OracleDataReader, ByVal columna As String) As Integer
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToInt32(dr(columna))
            End If

            Return 0
        End Function

        Private Function LeerEnteroNullable(ByVal dr As OracleDataReader, ByVal columna As String) As Integer?
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToInt32(dr(columna))
            End If

            Return Nothing
        End Function

        Private Function LeerDecimalNullable(ByVal dr As OracleDataReader, ByVal columna As String) As Decimal?
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToDecimal(dr(columna))
            End If

            Return Nothing
        End Function

        Private Function LeerFechaNullable(ByVal dr As OracleDataReader, ByVal columna As String) As DateTime?
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToDateTime(dr(columna))
            End If

            Return Nothing
        End Function

        Private Function TieneColumna(ByVal dr As OracleDataReader, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If dr.GetName(i).ToUpper() = nombreColumna.ToUpper() Then
                    Return True
                End If
            Next

            Return False
        End Function

    End Class
End Namespace