Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class RolDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub


        Public Function Insertar(rol As Rol) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_ROL.SP_INSERTAR_ROL", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = rol.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = rol.Descripcion

                    Dim pOut As New OracleParameter("P_ROL_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using

            End Using

        End Function



        Public Sub Actualizar(rol As Rol)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_ROL.SP_ACTUALIZAR_ROL", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ROL_ID", OracleDbType.Int32).Value = rol.RolId
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = rol.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = rol.Descripcion

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using

            End Using

        End Sub



        Public Sub Eliminar(rolId As Integer)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_ROL.SP_ELIMINAR_ROL", conn)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ROL_ID", OracleDbType.Int32).Value = rolId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using

            End Using

        End Sub



        Public Function Listar() As DataTable

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_ROL.SP_LISTAR_ROLES", conn)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using da As New OracleDataAdapter(cmd)

                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt

                    End Using

                End Using

            End Using

        End Function



        Public Function Buscar(nombre As String) As DataTable

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_ROL.SP_BUSCAR_ROL", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = nombre
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