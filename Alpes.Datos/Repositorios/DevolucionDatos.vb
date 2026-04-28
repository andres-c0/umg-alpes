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
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_INSERTAR_DEVOLUCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId

                    If String.IsNullOrWhiteSpace(entidad.Motivo) Then
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = entidad.Motivo
                    End If

                    cmd.Parameters.Add("P_ESTADO_DEVOLUCION", OracleDbType.Varchar2).Value = entidad.EstadoDevolucion
                    cmd.Parameters.Add("P_SOLICITUD_AT", OracleDbType.TimeStamp).Value = entidad.SolicitudAt

                    If entidad.ResolucionAt.HasValue Then
                        cmd.Parameters.Add("P_RESOLUCION_AT", OracleDbType.TimeStamp).Value = entidad.ResolucionAt.Value
                    Else
                        cmd.Parameters.Add("P_RESOLUCION_AT", OracleDbType.TimeStamp).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_DEVOLUCION_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_DEVOLUCION_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Devolucion)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_ACTUALIZAR_DEVOLUCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_DEVOLUCION_ID", OracleDbType.Int32).Value = entidad.DevolucionId
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId

                    If String.IsNullOrWhiteSpace(entidad.Motivo) Then
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = entidad.Motivo
                    End If

                    cmd.Parameters.Add("P_ESTADO_DEVOLUCION", OracleDbType.Varchar2).Value = entidad.EstadoDevolucion
                    cmd.Parameters.Add("P_SOLICITUD_AT", OracleDbType.TimeStamp).Value = entidad.SolicitudAt

                    If entidad.ResolucionAt.HasValue Then
                        cmd.Parameters.Add("P_RESOLUCION_AT", OracleDbType.TimeStamp).Value = entidad.ResolucionAt.Value
                    Else
                        cmd.Parameters.Add("P_RESOLUCION_AT", OracleDbType.TimeStamp).Value = DBNull.Value
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_ELIMINAR_DEVOLUCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_DEVOLUCION_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Devolucion
            Dim entidad As Devolucion = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_OBTENER_DEVOLUCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_DEVOLUCION_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = Mapear(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Devolucion)
            Dim lista As New List(Of Devolucion)

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_LISTAR_DEVOLUCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

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
            Dim lista As New List(Of Devolucion)

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_BUSCAR_DEVOLUCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

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
            Dim entidad As New Devolucion With {
                .DevolucionId = Convert.ToInt32(dr("DEVOLUCION_ID")),
                .OrdenVentaId = Convert.ToInt32(dr("ORDEN_VENTA_ID")),
                .CliId = Convert.ToInt32(dr("CLI_ID")),
                .Estado = dr("ESTADO").ToString()
            }

            If Not IsDBNull(dr("MOTIVO")) Then
                entidad.Motivo = dr("MOTIVO").ToString()
            Else
                entidad.Motivo = Nothing
            End If

            If Not IsDBNull(dr("ESTADO_DEVOLUCION")) Then
                entidad.EstadoDevolucion = dr("ESTADO_DEVOLUCION").ToString()
            Else
                entidad.EstadoDevolucion = Nothing
            End If

            If Not IsDBNull(dr("SOLICITUD_AT")) Then
                entidad.SolicitudAt = Convert.ToDateTime(dr("SOLICITUD_AT"))
            End If

            If TieneColumna(dr, "RESOLUCION_AT") AndAlso Not IsDBNull(dr("RESOLUCION_AT")) Then
                entidad.ResolucionAt = Convert.ToDateTime(dr("RESOLUCION_AT"))
            Else
                entidad.ResolucionAt = Nothing
            End If

            If TieneColumna(dr, "CREATED_AT") AndAlso Not IsDBNull(dr("CREATED_AT")) Then
                entidad.CreatedAt = Convert.ToDateTime(dr("CREATED_AT"))
            Else
                entidad.CreatedAt = Nothing
            End If

            If TieneColumna(dr, "UPDATED_AT") AndAlso Not IsDBNull(dr("UPDATED_AT")) Then
                entidad.UpdatedAt = Convert.ToDateTime(dr("UPDATED_AT"))
            Else
                entidad.UpdatedAt = Nothing
            End If

            Return entidad
        End Function

        Private Function TieneColumna(ByVal dr As OracleDataReader, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If dr.GetName(i).ToUpper() = nombreColumna.ToUpper() Then
                    Return True
                End If
            Next
            Return False
        End Function

    End Class
End Namespace