Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.RecursosHumanos

Namespace Repositorios
    Public Class NominaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Nomina) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA.INSERTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_emp_id", entidad.emp_id)
                    cmd.Parameters.Add("p_periodo_inicio", If(entidad.periodo_inicio.HasValue, CType(entidad.periodo_inicio.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("p_periodo_fin", If(entidad.periodo_fin.HasValue, CType(entidad.periodo_fin.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("p_monto_bruto", entidad.monto_bruto)
                    cmd.Parameters.Add("p_monto_neto", entidad.monto_neto)
                    cmd.Parameters.Add("p_fecha_pago", If(entidad.fecha_pago.HasValue, CType(entidad.fecha_pago.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("p_estado", entidad.estado)
                    cmd.Parameters.Add("p_nomina_id", OracleDbType.Int32).Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(cmd.Parameters("p_nomina_id").Value)
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Nomina)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA.ACTUALIZAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_nomina_id", entidad.nomina_id)
                    cmd.Parameters.Add("p_emp_id", entidad.emp_id)
                    cmd.Parameters.Add("p_periodo_inicio", If(entidad.periodo_inicio.HasValue, CType(entidad.periodo_inicio.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("p_periodo_fin", If(entidad.periodo_fin.HasValue, CType(entidad.periodo_fin.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("p_monto_bruto", entidad.monto_bruto)
                    cmd.Parameters.Add("p_monto_neto", entidad.monto_neto)
                    cmd.Parameters.Add("p_fecha_pago", If(entidad.fecha_pago.HasValue, CType(entidad.fecha_pago.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("p_estado", entidad.estado)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA.ELIMINAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_nomina_id", id)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Nomina
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA.OBTENER_POR_ID", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_nomina_id", id)
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

        Public Function Listar() As List(Of Nomina)
            Dim lista As New List(Of Nomina)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_NOMINA.LISTAR", conn)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Nomina
            Dim entidad As New Nomina()

            entidad.nomina_id = Convert.ToInt32(dr("nomina_id"))
            entidad.emp_id = Convert.ToInt32(dr("emp_id"))
            entidad.periodo_inicio = If(IsDBNull(dr("periodo_inicio")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("periodo_inicio")))
            entidad.periodo_fin = If(IsDBNull(dr("periodo_fin")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("periodo_fin")))
            entidad.monto_bruto = Convert.ToDecimal(dr("monto_bruto"))
            entidad.monto_neto = Convert.ToDecimal(dr("monto_neto"))
            entidad.fecha_pago = If(IsDBNull(dr("fecha_pago")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("fecha_pago")))
            entidad.estado = dr("estado").ToString()
            entidad.created_at = If(IsDBNull(dr("created_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("created_at")))
            entidad.updated_at = If(IsDBNull(dr("updated_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("updated_at")))

            Return entidad
        End Function

    End Class
End Namespace