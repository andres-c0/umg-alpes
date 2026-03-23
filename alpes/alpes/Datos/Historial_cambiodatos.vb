Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class HistorialCambioContrasenaDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub


        Public Sub RegistrarCambio(usuarioId As Integer, passwordAnterior As String)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_HISTORIAL_PASSWORD.SP_REGISTRAR_CAMBIO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_USUARIO_ID", OracleDbType.Int32).Value = usuarioId
                    cmd.Parameters.Add("P_PASSWORD_ANTERIOR", OracleDbType.Varchar2).Value = passwordAnterior

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using

            End Using

        End Sub



        Public Function HistorialUsuario(usuarioId As Integer) As DataTable

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_HISTORIAL_PASSWORD.SP_HISTORIAL_USUARIO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_USUARIO_ID", OracleDbType.Int32).Value = usuarioId
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