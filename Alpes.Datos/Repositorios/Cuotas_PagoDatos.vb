Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class Cuotas_PagoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Cuotas_Pago) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_INSERTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PAGO_ID", entidad.pago_id)
                    cmd.Parameters.Add("P_NUM_CUOTA", entidad.num_cuota)
                    cmd.Parameters.Add("P_MONTO_CUOTA", entidad.monto_cuota)
                    cmd.Parameters.Add("P_FECHA_VENCIMIENTO", If(entidad.fecha_vencimiento.HasValue, CType(entidad.fecha_vencimiento.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("P_FECHA_PAGO", If(entidad.fecha_pago.HasValue, CType(entidad.fecha_pago.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("P_ESTADO", entidad.estado)

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        Return Convert.ToInt32(cmd.Parameters("P_ID").Value)
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Cuotas_Pago)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_ACTUALIZAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CUOTA_ID", entidad.cuota_id)
                    cmd.Parameters.Add("P_PAGO_ID", entidad.pago_id)
                    cmd.Parameters.Add("P_NUM_CUOTA", entidad.num_cuota)
                    cmd.Parameters.Add("P_MONTO_CUOTA", entidad.monto_cuota)
                    cmd.Parameters.Add("P_FECHA_VENCIMIENTO", If(entidad.fecha_vencimiento.HasValue, CType(entidad.fecha_vencimiento.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("P_FECHA_PAGO", If(entidad.fecha_pago.HasValue, CType(entidad.fecha_pago.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("P_ESTADO", entidad.estado)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_ELIMINAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", id)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Cuotas_Pago
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_OBTENER", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", id)
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

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

        Public Function Listar() As List(Of Cuotas_Pago)
            Dim lista As New List(Of Cuotas_Pago)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_LISTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

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

        Public Function Buscar(ByVal valor As Integer) As List(Of Cuotas_Pago)
            Dim lista As New List(Of Cuotas_Pago)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_BUSCAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_VALOR", valor)
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

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

        Private Function Mapear(ByVal dr As OracleDataReader) As Cuotas_Pago
            Dim entidad As New Cuotas_Pago()

            entidad.cuota_id = Convert.ToInt32(dr("cuota_id"))
            entidad.pago_id = Convert.ToInt32(dr("pago_id"))
            entidad.num_cuota = Convert.ToInt32(dr("num_cuota"))
            entidad.monto_cuota = Convert.ToDecimal(dr("monto_cuota"))
            entidad.fecha_vencimiento = If(IsDBNull(dr("fecha_vencimiento")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("fecha_vencimiento")))
            entidad.fecha_pago = If(IsDBNull(dr("fecha_pago")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("fecha_pago")))
            entidad.created_at = Convert.ToDateTime(dr("created_at"))
            entidad.updated_at = If(IsDBNull(dr("updated_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("updated_at")))
            entidad.estado = dr("estado").ToString()

            Return entidad
        End Function

    End Class
End Namespace
