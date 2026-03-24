Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class NominaDetalleDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub


        Public Function Insertar(det As NominaDetalle) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_NOMINA_DETALLE.SP_INSERTAR_DETALLE", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NOMINA_ID", OracleDbType.Int32).Value = det.NominaId
                    cmd.Parameters.Add("P_CONCEPTO", OracleDbType.Varchar2).Value = det.Concepto
                    cmd.Parameters.Add("P_TIPO", OracleDbType.Varchar2).Value = det.Tipo
                    cmd.Parameters.Add("P_MONTO", OracleDbType.Decimal).Value = det.Monto

                    Dim pOut As New OracleParameter("P_DETALLE_ID", OracleDbType.Int32)
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