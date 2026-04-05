Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class Historial_CompraDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Historial_Compra) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_INSERTAR_HISTORIAL_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", entidad.cli_id)
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", entidad.orden_venta_id)
                    cmd.Parameters.Add("P_COMPRA_AT", entidad.compra_at)
                    cmd.Parameters.Add("P_MONTO_TOTAL_SNAPSHOT", entidad.monto_total_snapshot)

                    cmd.Parameters.Add("P_HIST_COMPRA_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        Return Convert.ToInt32(cmd.Parameters("P_HIST_COMPRA_ID").Value)
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Historial_Compra)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_ACTUALIZAR_HISTORIAL_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HIST_COMPRA_ID", entidad.hist_compra_id)
                    cmd.Parameters.Add("P_CLI_ID", entidad.cli_id)
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", entidad.orden_venta_id)
                    cmd.Parameters.Add("P_COMPRA_AT", entidad.compra_at)
                    cmd.Parameters.Add("P_MONTO_TOTAL_SNAPSHOT", entidad.monto_total_snapshot)

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
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_ELIMINAR_HISTORIAL_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HIST_COMPRA_ID", id)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Historial_Compra
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_OBTENER_HISTORIAL_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HIST_COMPRA_ID", id)
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

        Public Function Listar() As List(Of Historial_Compra)
            Dim lista As New List(Of Historial_Compra)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_LISTAR_HISTORIAL_COMPRA", conn)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Historial_Compra)
            Dim lista As New List(Of Historial_Compra)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_COMPRA.SP_BUSCAR_HISTORIAL_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", criterio)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Historial_Compra
            Dim entidad As New Historial_Compra()

            If TieneColumna(dr, "HIST_COMPRA_ID") AndAlso Not IsDBNull(dr("HIST_COMPRA_ID")) Then
                entidad.hist_compra_id = Convert.ToInt32(dr("HIST_COMPRA_ID"))
            End If

            If TieneColumna(dr, "CLI_ID") AndAlso Not IsDBNull(dr("CLI_ID")) Then
                entidad.cli_id = Convert.ToInt32(dr("CLI_ID"))
            End If

            If TieneColumna(dr, "ORDEN_VENTA_ID") AndAlso Not IsDBNull(dr("ORDEN_VENTA_ID")) Then
                entidad.orden_venta_id = Convert.ToInt32(dr("ORDEN_VENTA_ID"))
            End If

            If TieneColumna(dr, "COMPRA_AT") AndAlso Not IsDBNull(dr("COMPRA_AT")) Then
                entidad.compra_at = Convert.ToDateTime(dr("COMPRA_AT"))
            End If

            If TieneColumna(dr, "MONTO_TOTAL_SNAPSHOT") AndAlso Not IsDBNull(dr("MONTO_TOTAL_SNAPSHOT")) Then
                entidad.monto_total_snapshot = Convert.ToDecimal(dr("MONTO_TOTAL_SNAPSHOT"))
            End If

            If TieneColumna(dr, "CREATED_AT") AndAlso Not IsDBNull(dr("CREATED_AT")) Then
                entidad.created_at = Convert.ToDateTime(dr("CREATED_AT"))
            End If

            If TieneColumna(dr, "UPDATED_AT") AndAlso Not IsDBNull(dr("UPDATED_AT")) Then
                entidad.updated_at = Convert.ToDateTime(dr("UPDATED_AT"))
            End If

            If TieneColumna(dr, "ESTADO") AndAlso Not IsDBNull(dr("ESTADO")) Then
                entidad.estado = dr("ESTADO").ToString()
            End If

            Return entidad
        End Function

        Private Function TieneColumna(ByVal dr As OracleDataReader, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If String.Equals(dr.GetName(i), nombreColumna, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            Next
            Return False
        End Function

    End Class
End Namespace
