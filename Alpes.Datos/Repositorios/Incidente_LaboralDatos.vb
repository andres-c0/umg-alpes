Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.RecursosHumanos

Namespace Repositorios
    Public Class Incidente_LaboralDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Incidente_Laboral) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INCIDENTE_LABORAL.INSERTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_emp_id", entidad.emp_id)
                    cmd.Parameters.Add("p_fecha_incidente", If(entidad.fecha_incidente.HasValue, CType(entidad.fecha_incidente.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("p_descripcion", entidad.descripcion)
                    cmd.Parameters.Add("p_gravedad", entidad.gravedad)
                    cmd.Parameters.Add("p_acciones_tomadas", entidad.acciones_tomadas)
                    cmd.Parameters.Add("p_estado", entidad.estado)
                    cmd.Parameters.Add("p_incidente_id", OracleDbType.Int32).Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(cmd.Parameters("p_incidente_id").Value)
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Incidente_Laboral)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INCIDENTE_LABORAL.ACTUALIZAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_incidente_id", entidad.incidente_id)
                    cmd.Parameters.Add("p_emp_id", entidad.emp_id)
                    cmd.Parameters.Add("p_fecha_incidente", If(entidad.fecha_incidente.HasValue, CType(entidad.fecha_incidente.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("p_descripcion", entidad.descripcion)
                    cmd.Parameters.Add("p_gravedad", entidad.gravedad)
                    cmd.Parameters.Add("p_acciones_tomadas", entidad.acciones_tomadas)
                    cmd.Parameters.Add("p_estado", entidad.estado)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INCIDENTE_LABORAL.ELIMINAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_incidente_id", id)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Incidente_Laboral
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INCIDENTE_LABORAL.OBTENER_POR_ID", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_incidente_id", id)
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

        Public Function Listar() As List(Of Incidente_Laboral)
            Dim lista As New List(Of Incidente_Laboral)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INCIDENTE_LABORAL.LISTAR", conn)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Incidente_Laboral
            Dim entidad As New Incidente_Laboral()

            entidad.incidente_id = Convert.ToInt32(dr("incidente_id"))
            entidad.emp_id = Convert.ToInt32(dr("emp_id"))
            entidad.fecha_incidente = If(IsDBNull(dr("fecha_incidente")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("fecha_incidente")))
            entidad.descripcion = dr("descripcion").ToString()
            entidad.gravedad = dr("gravedad").ToString()
            entidad.acciones_tomadas = dr("acciones_tomadas").ToString()
            entidad.estado = dr("estado").ToString()
            entidad.created_at = If(IsDBNull(dr("created_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("created_at")))
            entidad.updated_at = If(IsDBNull(dr("updated_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("updated_at")))

            Return entidad
        End Function

    End Class
End Namespace