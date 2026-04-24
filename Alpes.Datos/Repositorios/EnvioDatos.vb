Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class EnvioDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Envio) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_ENVIO.SP_INSERTAR_ENVIO", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
                        cmd.Parameters.Add("P_ESTADO_ENVIO_ID", OracleDbType.Int32).Value = entidad.EstadoEnvioId
                        cmd.Parameters.Add("P_TIPO_ENTREGA_ID", OracleDbType.Int32).Value = entidad.TipoEntregaId
                        cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = entidad.ZonaEnvioId

                        Dim pRuta As New OracleParameter("P_RUTA_ENTREGA_ID", OracleDbType.Int32)
                        If entidad.RutaEntregaId.HasValue Then
                            pRuta.Value = entidad.RutaEntregaId.Value
                        Else
                            pRuta.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pRuta)

                        cmd.Parameters.Add("P_DIRECCION_ENTREGA_SNAPSHOT", OracleDbType.Varchar2).Value =
                            If(String.IsNullOrWhiteSpace(entidad.DireccionEntregaSnapshot), CType(DBNull.Value, Object), entidad.DireccionEntregaSnapshot)

                        cmd.Parameters.Add("P_COSTO_ENVIO_SNAPSHOT", OracleDbType.Decimal).Value = entidad.CostoEnvioSnapshot

                        Dim pFechaEnvio As New OracleParameter("P_FECHA_ENVIO", OracleDbType.Date)
                        pFechaEnvio.Value = If(entidad.FechaEnvio.HasValue, CType(entidad.FechaEnvio.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pFechaEnvio)

                        Dim pFechaEstimada As New OracleParameter("P_FECHA_ENTREGA_ESTIMADA", OracleDbType.Date)
                        pFechaEstimada.Value = If(entidad.FechaEntregaEstimada.HasValue, CType(entidad.FechaEntregaEstimada.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pFechaEstimada)

                        Dim pFechaReal As New OracleParameter("P_FECHA_ENTREGA_REAL", OracleDbType.Date)
                        pFechaReal.Value = If(entidad.FechaEntregaReal.HasValue, CType(entidad.FechaEntregaReal.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pFechaReal)

                        Dim pTracking As New OracleParameter("P_TRACKING_CODIGO", OracleDbType.Varchar2)
                        pTracking.Value = If(String.IsNullOrWhiteSpace(entidad.TrackingCodigo), CType(DBNull.Value, Object), entidad.TrackingCodigo)
                        cmd.Parameters.Add(pTracking)

                        Dim pId As New OracleParameter("P_ENVIO_ID", OracleDbType.Int32)
                        pId.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pId)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pId.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As Envio) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_ENVIO.SP_ACTUALIZAR_ENVIO", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ENVIO_ID", OracleDbType.Int32).Value = entidad.EnvioId
                        cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
                        cmd.Parameters.Add("P_ESTADO_ENVIO_ID", OracleDbType.Int32).Value = entidad.EstadoEnvioId
                        cmd.Parameters.Add("P_TIPO_ENTREGA_ID", OracleDbType.Int32).Value = entidad.TipoEntregaId
                        cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = entidad.ZonaEnvioId

                        Dim pRuta As New OracleParameter("P_RUTA_ENTREGA_ID", OracleDbType.Int32)
                        pRuta.Value = If(entidad.RutaEntregaId.HasValue, CType(entidad.RutaEntregaId.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pRuta)

                        cmd.Parameters.Add("P_DIRECCION_ENTREGA_SNAPSHOT", OracleDbType.Varchar2).Value =
                            If(String.IsNullOrWhiteSpace(entidad.DireccionEntregaSnapshot), CType(DBNull.Value, Object), entidad.DireccionEntregaSnapshot)

                        cmd.Parameters.Add("P_COSTO_ENVIO_SNAPSHOT", OracleDbType.Decimal).Value = entidad.CostoEnvioSnapshot

                        Dim pFechaEnvio As New OracleParameter("P_FECHA_ENVIO", OracleDbType.Date)
                        pFechaEnvio.Value = If(entidad.FechaEnvio.HasValue, CType(entidad.FechaEnvio.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pFechaEnvio)

                        Dim pFechaEstimada As New OracleParameter("P_FECHA_ENTREGA_ESTIMADA", OracleDbType.Date)
                        pFechaEstimada.Value = If(entidad.FechaEntregaEstimada.HasValue, CType(entidad.FechaEntregaEstimada.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pFechaEstimada)

                        Dim pFechaReal As New OracleParameter("P_FECHA_ENTREGA_REAL", OracleDbType.Date)
                        pFechaReal.Value = If(entidad.FechaEntregaReal.HasValue, CType(entidad.FechaEntregaReal.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pFechaReal)

                        Dim pTracking As New OracleParameter("P_TRACKING_CODIGO", OracleDbType.Varchar2)
                        pTracking.Value = If(String.IsNullOrWhiteSpace(entidad.TrackingCodigo), CType(DBNull.Value, Object), entidad.TrackingCodigo)
                        cmd.Parameters.Add(pTracking)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal envioId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_ENVIO.SP_ELIMINAR_ENVIO", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ENVIO_ID", OracleDbType.Int32).Value = envioId
                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal envioId As Integer) As Envio
            Dim entidad As Envio = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_ENVIO.SP_OBTENER_ENVIO", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ENVIO_ID", OracleDbType.Int32).Value = envioId
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

        Public Function Listar() As List(Of Envio)
            Dim lista As New List(Of Envio)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_ENVIO.SP_LISTAR_ENVIOS", conexion)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Envio)
            Dim lista As New List(Of Envio)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_ENVIO.SP_BUSCAR_ENVIOS", conexion)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Envio
            Dim entidad As New Envio()

            entidad.EnvioId = Convert.ToInt32(dr("ENVIO_ID"))
            entidad.OrdenVentaId = Convert.ToInt32(dr("ORDEN_VENTA_ID"))

            If TieneColumna(dr, "ESTADO_ENVIO_ID") AndAlso Not dr("ESTADO_ENVIO_ID") Is DBNull.Value Then
                entidad.EstadoEnvioId = Convert.ToInt32(dr("ESTADO_ENVIO_ID"))
            End If

            If TieneColumna(dr, "TIPO_ENTREGA_ID") AndAlso Not dr("TIPO_ENTREGA_ID") Is DBNull.Value Then
                entidad.TipoEntregaId = Convert.ToInt32(dr("TIPO_ENTREGA_ID"))
            End If

            If TieneColumna(dr, "ZONA_ENVIO_ID") AndAlso Not dr("ZONA_ENVIO_ID") Is DBNull.Value Then
                entidad.ZonaEnvioId = Convert.ToInt32(dr("ZONA_ENVIO_ID"))
            End If

            If TieneColumna(dr, "RUTA_ENTREGA_ID") AndAlso Not dr("RUTA_ENTREGA_ID") Is DBNull.Value Then
                entidad.RutaEntregaId = Convert.ToInt32(dr("RUTA_ENTREGA_ID"))
            End If

            If TieneColumna(dr, "DIRECCION_ENTREGA_SNAPSHOT") Then
                entidad.DireccionEntregaSnapshot = If(dr("DIRECCION_ENTREGA_SNAPSHOT") Is DBNull.Value, String.Empty, dr("DIRECCION_ENTREGA_SNAPSHOT").ToString())
            End If

            If TieneColumna(dr, "COSTO_ENVIO_SNAPSHOT") AndAlso Not dr("COSTO_ENVIO_SNAPSHOT") Is DBNull.Value Then
                entidad.CostoEnvioSnapshot = Convert.ToDecimal(dr("COSTO_ENVIO_SNAPSHOT"))
            End If

            If TieneColumna(dr, "FECHA_ENVIO") AndAlso Not dr("FECHA_ENVIO") Is DBNull.Value Then
                entidad.FechaEnvio = Convert.ToDateTime(dr("FECHA_ENVIO"))
            End If

            If TieneColumna(dr, "FECHA_ENTREGA_ESTIMADA") AndAlso Not dr("FECHA_ENTREGA_ESTIMADA") Is DBNull.Value Then
                entidad.FechaEntregaEstimada = Convert.ToDateTime(dr("FECHA_ENTREGA_ESTIMADA"))
            End If

            If TieneColumna(dr, "FECHA_ENTREGA_REAL") AndAlso Not dr("FECHA_ENTREGA_REAL") Is DBNull.Value Then
                entidad.FechaEntregaReal = Convert.ToDateTime(dr("FECHA_ENTREGA_REAL"))
            End If

            entidad.TrackingCodigo = If(TieneColumna(dr, "TRACKING_CODIGO") AndAlso Not dr("TRACKING_CODIGO") Is DBNull.Value, dr("TRACKING_CODIGO").ToString(), String.Empty)
            entidad.Estado = If(TieneColumna(dr, "ESTADO") AndAlso Not dr("ESTADO") Is DBNull.Value, dr("ESTADO").ToString(), String.Empty)

            entidad.EstadoEnvioDescripcion = If(TieneColumna(dr, "ESTADO_ENVIO") AndAlso Not dr("ESTADO_ENVIO") Is DBNull.Value, dr("ESTADO_ENVIO").ToString(), String.Empty)
            entidad.TipoEntregaNombre = If(TieneColumna(dr, "TIPO_ENTREGA") AndAlso Not dr("TIPO_ENTREGA") Is DBNull.Value, dr("TIPO_ENTREGA").ToString(), String.Empty)
            entidad.ZonaEnvioNombre = If(TieneColumna(dr, "ZONA_ENVIO") AndAlso Not dr("ZONA_ENVIO") Is DBNull.Value, dr("ZONA_ENVIO").ToString(), String.Empty)

            Return entidad
        End Function

        Private Function TieneColumna(ByVal dr As OracleDataReader, ByVal nombre As String) As Boolean
            Dim i As Integer
            For i = 0 To dr.FieldCount - 1
                If String.Equals(dr.GetName(i), nombre, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            Next
            Return False
        End Function

    End Class
End Namespace