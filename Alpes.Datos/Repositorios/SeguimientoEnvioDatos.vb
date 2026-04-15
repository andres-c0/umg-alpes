Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Envios

Namespace Repositorios
    Public Class SeguimientoEnvioDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As SeguimientoEnvio) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_INSERTAR_SEGUIMIENTO_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ENVIO_ID", OracleDbType.Int32).Value = entidad.EnvioId
                    cmd.Parameters.Add("P_ESTADO_ENVIO_ID", OracleDbType.Int32).Value = entidad.EstadoEnvioId
                    cmd.Parameters.Add("P_EVENTO_AT", OracleDbType.TimeStamp).Value = entidad.EventoAt

                    If String.IsNullOrWhiteSpace(entidad.UbicacionTexto) Then
                        cmd.Parameters.Add("P_UBICACION_TEXTO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_UBICACION_TEXTO", OracleDbType.Varchar2).Value = entidad.UbicacionTexto
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Observacion) Then
                        cmd.Parameters.Add("P_OBSERVACION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_OBSERVACION", OracleDbType.Varchar2).Value = entidad.Observacion
                    End If

                    cmd.Parameters.Add("P_SEG_ENVIO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_SEG_ENVIO_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As SeguimientoEnvio)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_ACTUALIZAR_SEGUIMIENTO_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_SEG_ENVIO_ID", OracleDbType.Int32).Value = entidad.SegEnvioId
                    cmd.Parameters.Add("P_ENVIO_ID", OracleDbType.Int32).Value = entidad.EnvioId
                    cmd.Parameters.Add("P_ESTADO_ENVIO_ID", OracleDbType.Int32).Value = entidad.EstadoEnvioId
                    cmd.Parameters.Add("P_EVENTO_AT", OracleDbType.TimeStamp).Value = entidad.EventoAt

                    If String.IsNullOrWhiteSpace(entidad.UbicacionTexto) Then
                        cmd.Parameters.Add("P_UBICACION_TEXTO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_UBICACION_TEXTO", OracleDbType.Varchar2).Value = entidad.UbicacionTexto
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Observacion) Then
                        cmd.Parameters.Add("P_OBSERVACION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_OBSERVACION", OracleDbType.Varchar2).Value = entidad.Observacion
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_ELIMINAR_SEGUIMIENTO_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_SEG_ENVIO_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As SeguimientoEnvio
            Dim entidad As SeguimientoEnvio = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_OBTENER_SEGUIMIENTO_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_SEG_ENVIO_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearSeguimientoEnvio(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of SeguimientoEnvio)
            Dim lista As New List(Of SeguimientoEnvio)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_LISTAR_SEGUIMIENTO_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearSeguimientoEnvio(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of SeguimientoEnvio)
            Dim lista As New List(Of SeguimientoEnvio)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SEGUIMIENTO_ENVIO.SP_BUSCAR_SEGUIMIENTO_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearSeguimientoEnvio(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearSeguimientoEnvio(ByVal dr As OracleDataReader) As SeguimientoEnvio
            Dim entidad As New SeguimientoEnvio()

            entidad.SegEnvioId = Convert.ToInt32(dr("SEG_ENVIO_ID"))
            entidad.EnvioId = Convert.ToInt32(dr("ENVIO_ID"))
            entidad.EstadoEnvioId = Convert.ToInt32(dr("ESTADO_ENVIO_ID"))
            entidad.EventoAt = Convert.ToDateTime(dr("EVENTO_AT"))
            entidad.UbicacionTexto = If(IsDBNull(dr("UBICACION_TEXTO")), Nothing, dr("UBICACION_TEXTO").ToString())
            entidad.Observacion = If(IsDBNull(dr("OBSERVACION")), Nothing, dr("OBSERVACION").ToString())
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace