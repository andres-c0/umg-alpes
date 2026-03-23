Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class SesionDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub


        Public Function CrearSesion(usuarioId As Integer, ip As String) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_SESION.SP_CREAR_SESION", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_USUARIO_ID", OracleDbType.Int32).Value = usuarioId
                    cmd.Parameters.Add("P_IP", OracleDbType.Varchar2).Value = ip

                    Dim pOut As New OracleParameter("P_SESION_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using

            End Using

        End Function



        Public Sub CerrarSesion(sesionId As Integer)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_SESION.SP_CERRAR_SESION", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_SESION_ID", OracleDbType.Int32).Value = sesionId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using

            End Using

        End Sub



        Public Function SesionesPorUsuario(usuarioId As Integer) As DataTable

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_SESION.SP_SESIONES_POR_USUARIO", conn)

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