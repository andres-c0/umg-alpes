Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class UsuarioDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub


        Public Function Insertar(usuario As Usuario) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_USUARIO.SP_INSERTAR_USUARIO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = usuario.Nombre
                    cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = usuario.Email
                    cmd.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2).Value = usuario.Password
                    cmd.Parameters.Add("P_ROL_ID", OracleDbType.Int32).Value = usuario.RolId

                    Dim pOut As New OracleParameter("P_USUARIO_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using

            End Using

        End Function



        Public Function Login(email As String, password As String) As DataTable

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_USUARIO.SP_LOGIN", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = email
                    cmd.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2).Value = password
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