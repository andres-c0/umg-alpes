Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class RolPermisoDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub


        Public Sub AsignarPermiso(rolId As Integer, permisoId As Integer)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_ROL_PERMISO.SP_ASIGNAR_PERMISO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ROL_ID", OracleDbType.Int32).Value = rolId
                    cmd.Parameters.Add("P_PERMISO_ID", OracleDbType.Int32).Value = permisoId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using

            End Using

        End Sub



        Public Sub EliminarPermiso(rolId As Integer, permisoId As Integer)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_ROL_PERMISO.SP_ELIMINAR_PERMISO_ROL", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ROL_ID", OracleDbType.Int32).Value = rolId
                    cmd.Parameters.Add("P_PERMISO_ID", OracleDbType.Int32).Value = permisoId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using

            End Using

        End Sub



        Public Function PermisosPorRol(rolId As Integer) As DataTable

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_ROL_PERMISO.SP_PERMISOS_POR_ROL", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ROL_ID", OracleDbType.Int32).Value = rolId
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using da As New OracleDataAdapter(cmd)

                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt

                    End Using

                End Using

            End Using

        End Function



        Public Function RolesPorPermiso(permisoId As Integer) As DataTable

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_ROL_PERMISO.SP_ROLES_POR_PERMISO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PERMISO_ID", OracleDbType.Int32).Value = permisoId
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
