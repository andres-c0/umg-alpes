Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class HistorialLaboralDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub


        Public Function Insertar(hist As HistorialLaboral) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_HISTORIAL_LABORAL.SP_INSERTAR_HISTORIAL", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_EMPLEADO_ID", OracleDbType.Int32).Value = hist.EmpleadoId
                    cmd.Parameters.Add("P_CARGO_ID", OracleDbType.Int32).Value = hist.CargoId
                    cmd.Parameters.Add("P_DEPARTAMENTO_ID", OracleDbType.Int32).Value = hist.DepartamentoId
                    cmd.Parameters.Add("P_SALARIO", OracleDbType.Decimal).Value = hist.Salario
                    cmd.Parameters.Add("P_FECHA_INICIO", OracleDbType.Date).Value = hist.FechaInicio
                    cmd.Parameters.Add("P_FECHA_FIN", OracleDbType.Date).Value = hist.FechaFin

                    Dim pOut As New OracleParameter("P_HISTORIAL_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using
            End Using

        End Function



        Public Sub Actualizar(hist As HistorialLaboral)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_HISTORIAL_LABORAL.SP_ACTUALIZAR_HISTORIAL", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HISTORIAL_ID", OracleDbType.Int32).Value = hist.HistorialId
                    cmd.Parameters.Add("P_CARGO_ID", OracleDbType.Int32).Value = hist.CargoId
                    cmd.Parameters.Add("P_DEPARTAMENTO_ID", OracleDbType.Int32).Value = hist.DepartamentoId
                    cmd.Parameters.Add("P_SALARIO", OracleDbType.Decimal).Value = hist.Salario
                    cmd.Parameters.Add("P_FECHA_FIN", OracleDbType.Date).Value = hist.FechaFin

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub



        Public Sub Eliminar(historialId As Integer)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_HISTORIAL_LABORAL.SP_ELIMINAR_HISTORIAL", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HISTORIAL_ID", OracleDbType.Int32).Value = historialId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub



        Public Function Listar() As DataTable

            Dim tabla As New DataTable()

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_HISTORIAL_LABORAL.SP_LISTAR_HISTORIAL", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using reader = cmd.ExecuteReader()
                        tabla.Load(reader)
                    End Using

                End Using
            End Using

            Return tabla

        End Function

    End Class

End Namespace