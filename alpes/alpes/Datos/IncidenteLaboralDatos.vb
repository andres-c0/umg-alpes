Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class IncidenteLaboralDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub


        Public Function Insertar(inc As IncidenteLaboral) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_INCIDENTE_LABORAL.SP_INSERTAR_INCIDENTE", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_EMPLEADO_ID", OracleDbType.Int32).Value = inc.EmpleadoId
                    cmd.Parameters.Add("P_TIPO_INCIDENTE", OracleDbType.Varchar2).Value = inc.TipoIncidente
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = inc.Descripcion
                    cmd.Parameters.Add("P_FECHA_INCIDENTE", OracleDbType.Date).Value = inc.FechaIncidente

                    Dim pOut As New OracleParameter("P_INCIDENTE_ID", OracleDbType.Int32)
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