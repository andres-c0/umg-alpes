Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class PermisoDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub


        Public Function Insertar(permiso As Permiso) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_PERMISO.SP_INSERTAR_PERMISO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = permiso.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = permiso.Descripcion

                    Dim pOut As New OracleParameter("P_PERMISO_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using

            End Using

        End Function



        Public Sub Actualizar(permiso As Permiso)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_PERMISO.SP_ACTUALIZAR_PERMISO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PERMISO_ID", OracleDbType.Int32).Value = permiso.PermisoId
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = permiso.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = permiso.Descripcion

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using

            End Using

        End Sub



        Public Sub Eliminar(permisoId As Integer)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_PERMISO.SP_ELIMINAR_PERMISO", conn)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PERMISO_ID", OracleDbType.Int32).Value = permisoId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using

            End Using

        End Sub



        Public Function Listar() As DataTable

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_PERMISO.SP_LISTAR_PERMISOS", conn)

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

                Using cmd As New OracleCommand("PKG_PERMISO.SP_BUSCAR_PERMISO", conn)

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

