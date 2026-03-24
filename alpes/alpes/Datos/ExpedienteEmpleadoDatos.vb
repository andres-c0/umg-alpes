Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class ExpedienteEmpleadoDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub


        Public Function Insertar(exp As ExpedienteEmpleado) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_EXPEDIENTE_EMPLEADO.SP_INSERTAR_EXPEDIENTE", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_EMPLEADO_ID", OracleDbType.Int32).Value = exp.EmpleadoId
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = exp.Descripcion
                    cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = exp.Observaciones

                    Dim pOut As New OracleParameter("P_EXPEDIENTE_ID", OracleDbType.Int32)
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