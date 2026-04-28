Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.RecursosHumanos

Namespace Repositorios
    Public Class EmpleadoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Empleado) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EMPLEADO.INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("p_depto_id", OracleDbType.Int32).Value = entidad.DeptoId
                    cmd.Parameters.Add("p_cargo_id", OracleDbType.Int32).Value = entidad.CargoId

                    If entidad.RolEmpleadoId.HasValue Then
                        cmd.Parameters.Add("p_rol_empleado_id", OracleDbType.Int32).Value = entidad.RolEmpleadoId.Value
                    Else
                        cmd.Parameters.Add("p_rol_empleado_id", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("p_nombres", OracleDbType.Varchar2).Value = entidad.Nombres
                    cmd.Parameters.Add("p_apellidos", OracleDbType.Varchar2).Value = entidad.Apellidos

                    If String.IsNullOrWhiteSpace(entidad.Email) Then
                        cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = entidad.Email
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Telefono) Then
                        cmd.Parameters.Add("p_telefono", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_telefono", OracleDbType.Varchar2).Value = entidad.Telefono
                    End If

                    cmd.Parameters.Add("p_fecha_ingreso", OracleDbType.Date).Value = entidad.FechaIngreso

                    If entidad.SalarioBase.HasValue Then
                        cmd.Parameters.Add("p_salario_base", OracleDbType.Decimal).Value = entidad.SalarioBase.Value
                    Else
                        cmd.Parameters.Add("p_salario_base", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Estado) Then
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = entidad.Estado
                    End If

                    cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("p_emp_id").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Empleado)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EMPLEADO.ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = entidad.EmpId
                    cmd.Parameters.Add("p_depto_id", OracleDbType.Int32).Value = entidad.DeptoId
                    cmd.Parameters.Add("p_cargo_id", OracleDbType.Int32).Value = entidad.CargoId

                    If entidad.RolEmpleadoId.HasValue Then
                        cmd.Parameters.Add("p_rol_empleado_id", OracleDbType.Int32).Value = entidad.RolEmpleadoId.Value
                    Else
                        cmd.Parameters.Add("p_rol_empleado_id", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("p_nombres", OracleDbType.Varchar2).Value = entidad.Nombres
                    cmd.Parameters.Add("p_apellidos", OracleDbType.Varchar2).Value = entidad.Apellidos

                    If String.IsNullOrWhiteSpace(entidad.Email) Then
                        cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = entidad.Email
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Telefono) Then
                        cmd.Parameters.Add("p_telefono", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_telefono", OracleDbType.Varchar2).Value = entidad.Telefono
                    End If

                    cmd.Parameters.Add("p_fecha_ingreso", OracleDbType.Date).Value = entidad.FechaIngreso

                    If entidad.SalarioBase.HasValue Then
                        cmd.Parameters.Add("p_salario_base", OracleDbType.Decimal).Value = entidad.SalarioBase.Value
                    Else
                        cmd.Parameters.Add("p_salario_base", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Estado) Then
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = entidad.Estado
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EMPLEADO.ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Empleado
            Dim entidad As Empleado = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EMPLEADO.OBTENER_POR_ID", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearEmpleado(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Empleado)
            Dim lista As New List(Of Empleado)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EMPLEADO.LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearEmpleado(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearEmpleado(ByVal dr As OracleDataReader) As Empleado
            Dim entidad As New Empleado()

            entidad.EmpId = Convert.ToInt32(dr("emp_id"))
            entidad.DeptoId = Convert.ToInt32(dr("depto_id"))
            entidad.CargoId = Convert.ToInt32(dr("cargo_id"))

            If IsDBNull(dr("rol_empleado_id")) Then
                entidad.RolEmpleadoId = Nothing
            Else
                entidad.RolEmpleadoId = Convert.ToInt32(dr("rol_empleado_id"))
            End If

            entidad.Nombres = dr("nombres").ToString()
            entidad.Apellidos = dr("apellidos").ToString()
            entidad.Email = If(IsDBNull(dr("email")), String.Empty, dr("email").ToString())
            entidad.Telefono = If(IsDBNull(dr("telefono")), String.Empty, dr("telefono").ToString())
            entidad.FechaIngreso = Convert.ToDateTime(dr("fecha_ingreso"))

            If IsDBNull(dr("salario_base")) Then
                entidad.SalarioBase = Nothing
            Else
                entidad.SalarioBase = Convert.ToDecimal(dr("salario_base"))
            End If

            entidad.CreatedAt = Convert.ToDateTime(dr("created_at"))

            If IsDBNull(dr("updated_at")) Then
                entidad.UpdatedAt = Nothing
            Else
                entidad.UpdatedAt = Convert.ToDateTime(dr("updated_at"))
            End If

            entidad.Estado = If(IsDBNull(dr("estado")), String.Empty, dr("estado").ToString())

            Return entidad
        End Function

    End Class
End Namespace