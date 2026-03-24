Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class EmpleadoDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(emp As Empleado) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_EMPLEADO.SP_INSERTAR_EMPLEADO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NOMBRES", OracleDbType.Varchar2).Value = emp.Nombres
                    cmd.Parameters.Add("P_APELLIDOS", OracleDbType.Varchar2).Value = emp.Apellidos
                    cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = emp.Email
                    cmd.Parameters.Add("P_TELEFONO", OracleDbType.Varchar2).Value = emp.Telefono
                    cmd.Parameters.Add("P_DIRECCION", OracleDbType.Varchar2).Value = emp.Direccion
                    cmd.Parameters.Add("P_DEPARTAMENTO_ID", OracleDbType.Int32).Value = emp.DepartamentoId
                    cmd.Parameters.Add("P_CARGO_ID", OracleDbType.Int32).Value = emp.CargoId
                    cmd.Parameters.Add("P_ROL_ID", OracleDbType.Int32).Value = emp.RolId

                    Dim pOut As New OracleParameter("P_EMPLEADO_ID", OracleDbType.Int32)
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