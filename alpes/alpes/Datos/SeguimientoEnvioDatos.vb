Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class SeguimientoEnvioDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(seguimientoEnvio As SeguimientoEnvio) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_INSERTAR_SEGUIMIENTO_ENVIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ENVIO_ID",        OracleDbType.Int32).Value   = seguimientoEnvio.EnvioId
                    cmd.Parameters.Add("P_ESTADO_ENVIO_ID", OracleDbType.Int32).Value   = seguimientoEnvio.EstadoEnvioId
                    cmd.Parameters.Add("P_EVENTO_AT",       OracleDbType.TimeStamp).Value = seguimientoEnvio.EventoAt
                    cmd.Parameters.Add("P_UBICACION_TEXTO", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(seguimientoEnvio.UbicacionTexto), DBNull.Value, seguimientoEnvio.UbicacionTexto)
                    cmd.Parameters.Add("P_OBSERVACION",     OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(seguimientoEnvio.Observacion), DBNull.Value, seguimientoEnvio.Observacion)

                    Dim pOut As New OracleParameter("P_SEG_ENVIO_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(seguimientoEnvio As SeguimientoEnvio)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_ACTUALIZAR_SEGUIMIENTO_ENVIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_SEG_ENVIO_ID",    OracleDbType.Int32).Value     = seguimientoEnvio.SegEnvioId
                    cmd.Parameters.Add("P_ENVIO_ID",        OracleDbType.Int32).Value     = seguimientoEnvio.EnvioId
                    cmd.Parameters.Add("P_ESTADO_ENVIO_ID", OracleDbType.Int32).Value     = seguimientoEnvio.EstadoEnvioId
                    cmd.Parameters.Add("P_EVENTO_AT",       OracleDbType.TimeStamp).Value = seguimientoEnvio.EventoAt
                    cmd.Parameters.Add("P_UBICACION_TEXTO", OracleDbType.Varchar2).Value  = If(String.IsNullOrWhiteSpace(seguimientoEnvio.UbicacionTexto), DBNull.Value, seguimientoEnvio.UbicacionTexto)
                    cmd.Parameters.Add("P_OBSERVACION",     OracleDbType.Varchar2).Value  = If(String.IsNullOrWhiteSpace(seguimientoEnvio.Observacion), DBNull.Value, seguimientoEnvio.Observacion)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(segEnvioId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_ELIMINAR_SEGUIMIENTO_ENVIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_SEG_ENVIO_ID", OracleDbType.Int32).Value = segEnvioId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_LISTAR_SEGUIMIENTO_ENVIO", conn)
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
                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_BUSCAR_SEGUIMIENTO_ENVIO", conn)
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
