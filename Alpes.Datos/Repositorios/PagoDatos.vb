Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class PagoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Pago) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO.SP_INSERTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", entidad.orden_venta_id)
                    cmd.Parameters.Add("P_METODO_PAGO_ID", entidad.metodo_pago_id)
                    cmd.Parameters.Add("P_MONTO", entidad.monto)
                    cmd.Parameters.Add("P_ESTADO_PAGO", entidad.estado_pago)
                    cmd.Parameters.Add("P_REFERENCIA", entidad.referencia)
                    cmd.Parameters.Add("P_PAGO_AT", entidad.pago_at)
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

        Public Sub Actualizar(ByVal entidad As Pago)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO.SP_ACTUALIZAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PAGO_ID", entidad.pago_id)
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", entidad.orden_venta_id)
                    cmd.Parameters.Add("P_METODO_PAGO_ID", entidad.metodo_pago_id)
                    cmd.Parameters.Add("P_MONTO", entidad.monto)
                    cmd.Parameters.Add("P_ESTADO_PAGO", entidad.estado_pago)
                    cmd.Parameters.Add("P_REFERENCIA", entidad.referencia)
                    cmd.Parameters.Add("P_PAGO_AT", entidad.pago_at)
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
                Using cmd As New OracleCommand("PKG_PAGO.SP_ELIMINAR", conn)
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

        Public Function ObtenerPorId(ByVal id As Integer) As Pago
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO.SP_OBTENER", conn)
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

        Public Function Listar() As List(Of Pago)
            Dim lista As New List(Of Pago)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO.SP_LISTAR", conn)
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

        Public Function Buscar(ByVal valor As Decimal) As List(Of Pago)
            Dim lista As New List(Of Pago)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO.SP_BUSCAR", conn)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Pago
            Dim entidad As New Pago()

            entidad.pago_id = Convert.ToInt32(dr("pago_id"))
            entidad.orden_venta_id = Convert.ToInt32(dr("orden_venta_id"))
            entidad.metodo_pago_id = Convert.ToInt32(dr("metodo_pago_id"))
            entidad.monto = Convert.ToDecimal(dr("monto"))
            entidad.estado_pago = dr("estado_pago").ToString()
            entidad.referencia = dr("referencia").ToString()
            entidad.pago_at = Convert.ToDateTime(dr("pago_at"))
            entidad.created_at = Convert.ToDateTime(dr("created_at"))
            entidad.updated_at = If(IsDBNull(dr("updated_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("updated_at")))
            entidad.estado = dr("estado").ToString()

            Return entidad
        End Function

    End Class
End Namespace
