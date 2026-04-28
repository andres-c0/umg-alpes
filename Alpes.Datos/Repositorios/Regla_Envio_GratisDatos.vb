Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class ReglaEnvioGratisDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As ReglaEnvioGratis) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_INSERTAR_REGLA_ENVIO_GRATIS", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = entidad.ZonaEnvioId.Value

                    If entidad.MontoMinimo.HasValue Then
                        cmd.Parameters.Add("P_MONTO_MINIMO", OracleDbType.Decimal).Value = entidad.MontoMinimo.Value
                    Else
                        cmd.Parameters.Add("P_MONTO_MINIMO", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If entidad.PesoMaxKg.HasValue Then
                        cmd.Parameters.Add("P_PESO_MAX_KG", OracleDbType.Decimal).Value = entidad.PesoMaxKg.Value
                    Else
                        cmd.Parameters.Add("P_PESO_MAX_KG", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If entidad.VigenciaInicio.HasValue Then
                        cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio.Value
                    Else
                        cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If entidad.VigenciaFin.HasValue Then
                        cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin.Value
                    Else
                        cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_REGLA_ENVIO_GRATIS_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_REGLA_ENVIO_GRATIS_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As ReglaEnvioGratis)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_ACTUALIZAR_REGLA_ENVIO_GRATIS", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_REGLA_ENVIO_GRATIS_ID", OracleDbType.Int32).Value = entidad.ReglaEnvioGratisId
                    cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = entidad.ZonaEnvioId.Value

                    If entidad.MontoMinimo.HasValue Then
                        cmd.Parameters.Add("P_MONTO_MINIMO", OracleDbType.Decimal).Value = entidad.MontoMinimo.Value
                    Else
                        cmd.Parameters.Add("P_MONTO_MINIMO", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If entidad.PesoMaxKg.HasValue Then
                        cmd.Parameters.Add("P_PESO_MAX_KG", OracleDbType.Decimal).Value = entidad.PesoMaxKg.Value
                    Else
                        cmd.Parameters.Add("P_PESO_MAX_KG", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If entidad.VigenciaInicio.HasValue Then
                        cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio.Value
                    Else
                        cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If entidad.VigenciaFin.HasValue Then
                        cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin.Value
                    Else
                        cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = DBNull.Value
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_ELIMINAR_REGLA_ENVIO_GRATIS", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_REGLA_ENVIO_GRATIS_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As ReglaEnvioGratis
            Dim entidad As ReglaEnvioGratis = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_OBTENER_REGLA_ENVIO_GRATIS", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_REGLA_ENVIO_GRATIS_ID", OracleDbType.Int32).Value = id
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

        Public Function Listar() As List(Of ReglaEnvioGratis)
            Dim lista As New List(Of ReglaEnvioGratis)

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_LISTAR_REGLAS_ENVIO_GRATIS", cn)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ReglaEnvioGratis)
            Dim lista As New List(Of ReglaEnvioGratis)

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_ENVIO_GRATIS.SP_BUSCAR_REGLAS_ENVIO_GRATIS", cn)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As ReglaEnvioGratis
            Dim entidad As New ReglaEnvioGratis With {
                .ReglaEnvioGratisId = Convert.ToInt32(dr("REGLA_ENVIO_GRATIS_ID")),
                .Estado = dr("ESTADO").ToString()
            }

            If TieneColumna(dr, "ZONA_ENVIO_ID") AndAlso Not IsDBNull(dr("ZONA_ENVIO_ID")) Then
                entidad.ZonaEnvioId = Convert.ToInt32(dr("ZONA_ENVIO_ID"))
            Else
                entidad.ZonaEnvioId = Nothing
            End If

            If TieneColumna(dr, "ZONA_ENVIO") AndAlso Not IsDBNull(dr("ZONA_ENVIO")) Then
                entidad.ZonaEnvio = dr("ZONA_ENVIO").ToString()
            Else
                entidad.ZonaEnvio = Nothing
            End If

            If Not IsDBNull(dr("MONTO_MINIMO")) Then
                entidad.MontoMinimo = Convert.ToDecimal(dr("MONTO_MINIMO"))
            Else
                entidad.MontoMinimo = Nothing
            End If

            If Not IsDBNull(dr("PESO_MAX_KG")) Then
                entidad.PesoMaxKg = Convert.ToDecimal(dr("PESO_MAX_KG"))
            Else
                entidad.PesoMaxKg = Nothing
            End If

            If Not IsDBNull(dr("VIGENCIA_INICIO")) Then
                entidad.VigenciaInicio = Convert.ToDateTime(dr("VIGENCIA_INICIO"))
            Else
                entidad.VigenciaInicio = Nothing
            End If

            If Not IsDBNull(dr("VIGENCIA_FIN")) Then
                entidad.VigenciaFin = Convert.ToDateTime(dr("VIGENCIA_FIN"))
            Else
                entidad.VigenciaFin = Nothing
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