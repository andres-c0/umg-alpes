Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.RRHH

Namespace Repositorios
    Public Class Rol_EmpleadoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Rol_Empleado) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ROL_EMPLEADO.INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = entidad.Nombre

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("p_descripcion", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_descripcion", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Estado) Then
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = "ACTIVO"
                    Else
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = entidad.Estado
                    End If

                    cmd.Parameters.Add("p_rol_empleado_id", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("p_rol_empleado_id").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Rol_Empleado)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ROL_EMPLEADO.ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_rol_empleado_id", OracleDbType.Int32).Value = entidad.RolEmpleadoId
                    cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = entidad.Nombre

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("p_descripcion", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_descripcion", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Estado) Then
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = "ACTIVO"
                    Else
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = entidad.Estado
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ROL_EMPLEADO.ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_rol_empleado_id", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Rol_Empleado
            Dim entidad As Rol_Empleado = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ROL_EMPLEADO.OBTENER_POR_ID", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_rol_empleado_id", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearRolEmpleado(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Rol_Empleado)
            Dim lista As New List(Of Rol_Empleado)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ROL_EMPLEADO.LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearRolEmpleado(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearRolEmpleado(ByVal dr As OracleDataReader) As Rol_Empleado
            Dim entidad As New Rol_Empleado()

            entidad.RolEmpleadoId = Convert.ToInt32(dr("ROL_EMPLEADO_ID"))
            entidad.Nombre = dr("NOMBRE").ToString()
            entidad.Descripcion = If(IsDBNull(dr("DESCRIPCION")), Nothing, dr("DESCRIPCION").ToString())
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))

            Return entidad
        End Function

    End Class
End Namespace