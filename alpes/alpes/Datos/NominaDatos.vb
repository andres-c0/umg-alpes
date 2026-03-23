Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class NominaDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub


        Public Function Insertar(nom As Nomina) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_NOMINA.SP_INSERTAR_NOMINA", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_EMPLEADO_ID", OracleDbType.Int32).Value = nom.EmpleadoId
                    cmd.Parameters.Add("P_FECHA_PAGO", OracleDbType.Date).Value = nom.FechaPago
                    cmd.Parameters.Add("P_SALARIO_BASE", OracleDbType.Decimal).Value = nom.SalarioBase
                    cmd.Parameters.Add("P_TOTAL_DEDUCCION", OracleDbType.Decimal).Value = nom.TotalDeduccion
                    cmd.Parameters.Add("P_TOTAL_PAGO", OracleDbType.Decimal).Value = nom.TotalPago

                    Dim pOut As New OracleParameter("P_NOMINA_ID", OracleDbType.Int32)
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