Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class EvaluacionDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub


        Public Function Insertar(ev As Evaluacion) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_EVALUACION.SP_INSERTAR_EVALUACION", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_EMPLEADO_ID", OracleDbType.Int32).Value = ev.EmpleadoId
                    cmd.Parameters.Add("P_FECHA_EVAL", OracleDbType.Date).Value = ev.FechaEvaluacion
                    cmd.Parameters.Add("P_CALIFICACION", OracleDbType.Decimal).Value = ev.Calificacion
                    cmd.Parameters.Add("P_COMENTARIOS", OracleDbType.Varchar2).Value = ev.Comentarios

                    Dim pOut As New OracleParameter("P_EVALUACION_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using
            End Using

        End Function

    End Class

End Namespace