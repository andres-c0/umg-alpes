Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.RecursosHumanos

Namespace Repositorios
    Public Class Nomina_DetalleDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Nomina_Detalle) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA_DETALLE.INSERTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_nomina_id", entidad.nomina_id)
                    cmd.Parameters.Add("p_tipo", entidad.tipo)
                    cmd.Parameters.Add("p_concepto", entidad.concepto)
                    cmd.Parameters.Add("p_monto", entidad.monto)
                    cmd.Parameters.Add("p_estado", entidad.estado)
                    cmd.Parameters.Add("p_nomina_detalle_id", OracleDbType.Int32).Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(cmd.Parameters("p_nomina_detalle_id").Value)
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Nomina_Detalle)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA_DETALLE.ACTUALIZAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_nomina_detalle_id", entidad.nomina_detalle_id)
                    cmd.Parameters.Add("p_nomina_id", entidad.nomina_id)
                    cmd.Parameters.Add("p_tipo", entidad.tipo)
                    cmd.Parameters.Add("p_concepto", entidad.concepto)
                    cmd.Parameters.Add("p_monto", entidad.monto)
                    cmd.Parameters.Add("p_estado", entidad.estado)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA_DETALLE.ELIMINAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_nomina_detalle_id", id)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Nomina_Detalle
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA_DETALLE.OBTENER_POR_ID", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_nomina_detalle_id", id)
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

        Public Function Listar() As List(Of Nomina_Detalle)
            Dim lista As New List(Of Nomina_Detalle)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA_DETALLE.LISTAR", conn)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Nomina_Detalle
            Dim entidad As New Nomina_Detalle()

            entidad.nomina_detalle_id = Convert.ToInt32(dr("nomina_detalle_id"))
            entidad.nomina_id = Convert.ToInt32(dr("nomina_id"))
            entidad.tipo = dr("tipo").ToString()
            entidad.concepto = dr("concepto").ToString()
            entidad.monto = Convert.ToDecimal(dr("monto"))
            entidad.estado = dr("estado").ToString()
            entidad.created_at = If(IsDBNull(dr("created_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("created_at")))
            entidad.updated_at = If(IsDBNull(dr("updated_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("updated_at")))

            Return entidad
        End Function

    End Class
End Namespace