Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class DevolucionDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Devolucion) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_INSERTAR_DEVOLUCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", entidad.orden_venta_id)
                    cmd.Parameters.Add("P_CLI_ID", entidad.cli_id)
                    cmd.Parameters.Add("P_MOTIVO", entidad.motivo)
                    cmd.Parameters.Add("P_ESTADO_DEVOLUCION", entidad.estado_devolucion)
                    cmd.Parameters.Add("P_SOLICITUD_AT", entidad.solicitud_at)
                    cmd.Parameters.Add("P_RESOLUCION_AT", If(entidad.resolucion_at.HasValue, CType(entidad.resolucion_at.Value, Object), DBNull.Value))

                    cmd.Parameters.Add("P_DEVOLUCION_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        Return Convert.ToInt32(cmd.Parameters("P_DEVOLUCION_ID").Value)
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Devolucion)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_ACTUALIZAR_DEVOLUCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_DEVOLUCION_ID", entidad.devolucion_id)
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", entidad.orden_venta_id)
                    cmd.Parameters.Add("P_CLI_ID", entidad.cli_id)
                    cmd.Parameters.Add("P_MOTIVO", entidad.motivo)
                    cmd.Parameters.Add("P_ESTADO_DEVOLUCION", entidad.estado_devolucion)
                    cmd.Parameters.Add("P_SOLICITUD_AT", entidad.solicitud_at)
                    cmd.Parameters.Add("P_RESOLUCION_AT", If(entidad.resolucion_at.HasValue, CType(entidad.resolucion_at.Value, Object), DBNull.Value))

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
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_ELIMINAR_DEVOLUCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_DEVOLUCION_ID", id)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Devolucion
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_OBTENER_DEVOLUCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_DEVOLUCION_ID", id)
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

        Public Function Listar() As List(Of Devolucion)
            Dim lista As New List(Of Devolucion)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_LISTAR_DEVOLUCION", conn)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Devolucion)
            Dim lista As New List(Of Devolucion)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_BUSCAR_DEVOLUCION", conn)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Devolucion
            Dim entidad As New Devolucion()

            If TieneColumna(dr, "DEVOLUCION_ID") AndAlso Not IsDBNull(dr("DEVOLUCION_ID")) Then
                entidad.devolucion_id = Convert.ToInt32(dr("DEVOLUCION_ID"))
            End If

            If TieneColumna(dr, "ORDEN_VENTA_ID") AndAlso Not IsDBNull(dr("ORDEN_VENTA_ID")) Then
                entidad.orden_venta_id = Convert.ToInt32(dr("ORDEN_VENTA_ID"))
            End If

            If TieneColumna(dr, "CLI_ID") AndAlso Not IsDBNull(dr("CLI_ID")) Then
                entidad.cli_id = Convert.ToInt32(dr("CLI_ID"))
            End If

            If TieneColumna(dr, "MOTIVO") AndAlso Not IsDBNull(dr("MOTIVO")) Then
                entidad.motivo = dr("MOTIVO").ToString()
            End If

            If TieneColumna(dr, "ESTADO_DEVOLUCION") AndAlso Not IsDBNull(dr("ESTADO_DEVOLUCION")) Then
                entidad.estado_devolucion = dr("ESTADO_DEVOLUCION").ToString()
            End If

            If TieneColumna(dr, "SOLICITUD_AT") AndAlso Not IsDBNull(dr("SOLICITUD_AT")) Then
                entidad.solicitud_at = Convert.ToDateTime(dr("SOLICITUD_AT"))
            End If

            If TieneColumna(dr, "RESOLUCION_AT") AndAlso Not IsDBNull(dr("RESOLUCION_AT")) Then
                entidad.resolucion_at = Convert.ToDateTime(dr("RESOLUCION_AT"))
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