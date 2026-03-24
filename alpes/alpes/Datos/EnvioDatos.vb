Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class EnvioDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(envio As Envio) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ENVIO.SP_INSERTAR_ENVIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID",             OracleDbType.Int32).Value    = envio.OrdenVentaId
                    cmd.Parameters.Add("P_ESTADO_ENVIO_ID",            OracleDbType.Int32).Value    = envio.EstadoEnvioId
                    cmd.Parameters.Add("P_TIPO_ENTREGA_ID",            OracleDbType.Int32).Value    = envio.TipoEntregaId
                    cmd.Parameters.Add("P_ZONA_ENVIO_ID",              OracleDbType.Int32).Value    = envio.ZonaEnvioId
                    cmd.Parameters.Add("P_RUTA_ENTREGA_ID",            OracleDbType.Int32).Value    = If(envio.RutaEntregaId.HasValue, envio.RutaEntregaId.Value, DBNull.Value)
                    cmd.Parameters.Add("P_DIRECCION_ENTREGA_SNAPSHOT", OracleDbType.Varchar2).Value = envio.DireccionEntregaSnapshot
                    cmd.Parameters.Add("P_COSTO_ENVIO_SNAPSHOT",       OracleDbType.Decimal).Value  = envio.CostoEnvioSnapshot
                    cmd.Parameters.Add("P_FECHA_ENVIO",                OracleDbType.Date).Value     = If(envio.FechaEnvio.HasValue, envio.FechaEnvio.Value, DBNull.Value)
                    cmd.Parameters.Add("P_FECHA_ENTREGA_ESTIMADA",     OracleDbType.Date).Value     = If(envio.FechaEntregaEstimada.HasValue, envio.FechaEntregaEstimada.Value, DBNull.Value)
                    cmd.Parameters.Add("P_FECHA_ENTREGA_REAL",         OracleDbType.Date).Value     = If(envio.FechaEntregaReal.HasValue, envio.FechaEntregaReal.Value, DBNull.Value)
                    cmd.Parameters.Add("P_TRACKING_CODIGO",            OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(envio.TrackingCodigo), DBNull.Value, envio.TrackingCodigo)

                    Dim pOut As New OracleParameter("P_ENVIO_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(envio As Envio)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ENVIO.SP_ACTUALIZAR_ENVIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ENVIO_ID",                   OracleDbType.Int32).Value    = envio.EnvioId
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID",             OracleDbType.Int32).Value    = envio.OrdenVentaId
                    cmd.Parameters.Add("P_ESTADO_ENVIO_ID",            OracleDbType.Int32).Value    = envio.EstadoEnvioId
                    cmd.Parameters.Add("P_TIPO_ENTREGA_ID",            OracleDbType.Int32).Value    = envio.TipoEntregaId
                    cmd.Parameters.Add("P_ZONA_ENVIO_ID",              OracleDbType.Int32).Value    = envio.ZonaEnvioId
                    cmd.Parameters.Add("P_RUTA_ENTREGA_ID",            OracleDbType.Int32).Value    = If(envio.RutaEntregaId.HasValue, envio.RutaEntregaId.Value, DBNull.Value)
                    cmd.Parameters.Add("P_DIRECCION_ENTREGA_SNAPSHOT", OracleDbType.Varchar2).Value = envio.DireccionEntregaSnapshot
                    cmd.Parameters.Add("P_COSTO_ENVIO_SNAPSHOT",       OracleDbType.Decimal).Value  = envio.CostoEnvioSnapshot
                    cmd.Parameters.Add("P_FECHA_ENVIO",                OracleDbType.Date).Value     = If(envio.FechaEnvio.HasValue, envio.FechaEnvio.Value, DBNull.Value)
                    cmd.Parameters.Add("P_FECHA_ENTREGA_ESTIMADA",     OracleDbType.Date).Value     = If(envio.FechaEntregaEstimada.HasValue, envio.FechaEntregaEstimada.Value, DBNull.Value)
                    cmd.Parameters.Add("P_FECHA_ENTREGA_REAL",         OracleDbType.Date).Value     = If(envio.FechaEntregaReal.HasValue, envio.FechaEntregaReal.Value, DBNull.Value)
                    cmd.Parameters.Add("P_TRACKING_CODIGO",            OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(envio.TrackingCodigo), DBNull.Value, envio.TrackingCodigo)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(envioId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ENVIO.SP_ELIMINAR_ENVIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ENVIO_ID", OracleDbType.Int32).Value = envioId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ENVIO.SP_LISTAR_ENVIOS", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output
                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

        Public Function Buscar(criterio As String, valor As String) As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ENVIO.SP_BUSCAR_ENVIOS", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR",    OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR",   OracleDbType.RefCursor).Direction = ParameterDirection.Output
                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

    End Class

End Namespace
