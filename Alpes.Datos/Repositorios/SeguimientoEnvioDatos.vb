Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class SeguimientoEnvioDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As SeguimientoEnvio) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_INSERTAR_SEGUIMIENTO_ENVIO", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ENVIO_ID", OracleDbType.Int32).Value = entidad.EnvioId
                        cmd.Parameters.Add("P_ESTADO_ENVIO_ID", OracleDbType.Int32).Value = entidad.EstadoEnvioId

                        Dim pEventoAt As New OracleParameter("P_EVENTO_AT", OracleDbType.TimeStamp)
                        pEventoAt.Value = If(entidad.EventoAt.HasValue, CType(entidad.EventoAt.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pEventoAt)

                        Dim pUbicacion As New OracleParameter("P_UBICACION_TEXTO", OracleDbType.Varchar2)
                        pUbicacion.Value = If(String.IsNullOrWhiteSpace(entidad.UbicacionTexto), CType(DBNull.Value, Object), entidad.UbicacionTexto)
                        cmd.Parameters.Add(pUbicacion)

                        Dim pObservacion As New OracleParameter("P_OBSERVACION", OracleDbType.Varchar2)
                        pObservacion.Value = If(String.IsNullOrWhiteSpace(entidad.Observacion), CType(DBNull.Value, Object), entidad.Observacion)
                        cmd.Parameters.Add(pObservacion)

                        Dim pId As New OracleParameter("P_SEG_ENVIO_ID", OracleDbType.Int32)
                        pId.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pId)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pId.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As SeguimientoEnvio) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_ACTUALIZAR_SEGUIMIENTO_ENVIO", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_SEG_ENVIO_ID", OracleDbType.Int32).Value = entidad.SegEnvioId
                        cmd.Parameters.Add("P_ENVIO_ID", OracleDbType.Int32).Value = entidad.EnvioId
                        cmd.Parameters.Add("P_ESTADO_ENVIO_ID", OracleDbType.Int32).Value = entidad.EstadoEnvioId

                        Dim pEventoAt As New OracleParameter("P_EVENTO_AT", OracleDbType.TimeStamp)
                        pEventoAt.Value = If(entidad.EventoAt.HasValue, CType(entidad.EventoAt.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pEventoAt)

                        Dim pUbicacion As New OracleParameter("P_UBICACION_TEXTO", OracleDbType.Varchar2)
                        pUbicacion.Value = If(String.IsNullOrWhiteSpace(entidad.UbicacionTexto), CType(DBNull.Value, Object), entidad.UbicacionTexto)
                        cmd.Parameters.Add(pUbicacion)

                        Dim pObservacion As New OracleParameter("P_OBSERVACION", OracleDbType.Varchar2)
                        pObservacion.Value = If(String.IsNullOrWhiteSpace(entidad.Observacion), CType(DBNull.Value, Object), entidad.Observacion)
                        cmd.Parameters.Add(pObservacion)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal segEnvioId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_ELIMINAR_SEGUIMIENTO_ENVIO", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_SEG_ENVIO_ID", OracleDbType.Int32).Value = segEnvioId
                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal segEnvioId As Integer) As SeguimientoEnvio
            Dim entidad As SeguimientoEnvio = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_OBTENER_SEGUIMIENTO_ENVIO", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_SEG_ENVIO_ID", OracleDbType.Int32).Value = segEnvioId
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

        Public Function Listar() As List(Of SeguimientoEnvio)
            Dim lista As New List(Of SeguimientoEnvio)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_LISTAR_SEGUIMIENTO_ENVIO", conexion)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of SeguimientoEnvio)
            Dim lista As New List(Of SeguimientoEnvio)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_BUSCAR_SEGUIMIENTO_ENVIO", conexion)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As SeguimientoEnvio
            Dim entidad As New SeguimientoEnvio()

            entidad.SegEnvioId = Convert.ToInt32(dr("SEG_ENVIO_ID"))
            entidad.EnvioId = Convert.ToInt32(dr("ENVIO_ID"))
            entidad.EstadoEnvioId = Convert.ToInt32(dr("ESTADO_ENVIO_ID"))

            If Not dr("EVENTO_AT") Is DBNull.Value Then
                entidad.EventoAt = Convert.ToDateTime(dr("EVENTO_AT"))
            End If

            entidad.UbicacionTexto = If(dr("UBICACION_TEXTO") Is DBNull.Value, String.Empty, dr("UBICACION_TEXTO").ToString())
            entidad.Observacion = If(dr("OBSERVACION") Is DBNull.Value, String.Empty, dr("OBSERVACION").ToString())

            If Not dr("CREATED_AT") Is DBNull.Value Then
                entidad.CreatedAt = Convert.ToDateTime(dr("CREATED_AT"))
            End If

            If Not dr("UPDATED_AT") Is DBNull.Value Then
                entidad.UpdatedAt = Convert.ToDateTime(dr("UPDATED_AT"))
            End If

            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace