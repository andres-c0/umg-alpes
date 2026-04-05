Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.RecursosHumanos

Namespace Repositorios
    Public Class Expediente_EmpleadoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Expediente_Empleado) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EXPEDIENTE_EMPLEADO.INSERTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_emp_id", entidad.emp_id)
                    cmd.Parameters.Add("p_tipo_documento", entidad.tipo_documento)
                    cmd.Parameters.Add("p_url_documento", entidad.url_documento)
                    cmd.Parameters.Add("p_fecha_documento", If(entidad.fecha_documento.HasValue, CType(entidad.fecha_documento.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("p_estado", entidad.estado)
                    cmd.Parameters.Add("p_expediente_empleado_id", OracleDbType.Int32).Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(cmd.Parameters("p_expediente_empleado_id").Value)
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Expediente_Empleado)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EXPEDIENTE_EMPLEADO.ACTUALIZAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_expediente_empleado_id", entidad.expediente_empleado_id)
                    cmd.Parameters.Add("p_emp_id", entidad.emp_id)
                    cmd.Parameters.Add("p_tipo_documento", entidad.tipo_documento)
                    cmd.Parameters.Add("p_url_documento", entidad.url_documento)
                    cmd.Parameters.Add("p_fecha_documento", If(entidad.fecha_documento.HasValue, CType(entidad.fecha_documento.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("p_estado", entidad.estado)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EXPEDIENTE_EMPLEADO.ELIMINAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_expediente_empleado_id", id)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Expediente_Empleado
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EXPEDIENTE_EMPLEADO.OBTENER_POR_ID", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_expediente_empleado_id", id)
                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            Return Mapear(dr)
                        End If
                    End Using
                End Using
            End Using

            Return Nothing
        End Function

        Public Function Listar() As List(Of Expediente_Empleado)
            Dim lista As New List(Of Expediente_Empleado)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EXPEDIENTE_EMPLEADO.LISTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function Mapear(ByVal dr As OracleDataReader) As Expediente_Empleado
            Dim entidad As New Expediente_Empleado()

            entidad.expediente_empleado_id = Convert.ToInt32(dr("expediente_empleado_id"))
            entidad.emp_id = Convert.ToInt32(dr("emp_id"))
            entidad.tipo_documento = dr("tipo_documento").ToString()
            entidad.url_documento = dr("url_documento").ToString()
            entidad.fecha_documento = If(IsDBNull(dr("fecha_documento")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("fecha_documento")))
            entidad.estado = dr("estado").ToString()
            entidad.created_at = If(IsDBNull(dr("created_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("created_at")))
            entidad.updated_at = If(IsDBNull(dr("updated_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("updated_at")))

            Return entidad
        End Function

    End Class
End Namespace