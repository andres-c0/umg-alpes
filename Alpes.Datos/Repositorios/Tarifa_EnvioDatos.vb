Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class TarifaEnvioDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As TarifaEnvio) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_INSERTAR_TARIFA_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = entidad.ZonaEnvioId.Value
                    cmd.Parameters.Add("P_TIPO_ENTREGA_ID", OracleDbType.Int32).Value = entidad.TipoEntregaId.Value
                    cmd.Parameters.Add("P_PESO_DESDE_KG", OracleDbType.Decimal).Value = entidad.PesoDesdeKg.Value
                    cmd.Parameters.Add("P_PESO_HASTA_KG", OracleDbType.Decimal).Value = entidad.PesoHastaKg.Value
                    cmd.Parameters.Add("P_COSTO", OracleDbType.Decimal).Value = entidad.Costo.Value
                    cmd.Parameters.Add("P_TARIFA_ENVIO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_TARIFA_ENVIO_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As TarifaEnvio)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_ACTUALIZAR_TARIFA_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_TARIFA_ENVIO_ID", OracleDbType.Int32).Value = entidad.TarifaEnvioId
                    cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = entidad.ZonaEnvioId.Value
                    cmd.Parameters.Add("P_TIPO_ENTREGA_ID", OracleDbType.Int32).Value = entidad.TipoEntregaId.Value
                    cmd.Parameters.Add("P_PESO_DESDE_KG", OracleDbType.Decimal).Value = entidad.PesoDesdeKg.Value
                    cmd.Parameters.Add("P_PESO_HASTA_KG", OracleDbType.Decimal).Value = entidad.PesoHastaKg.Value
                    cmd.Parameters.Add("P_COSTO", OracleDbType.Decimal).Value = entidad.Costo.Value

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_ELIMINAR_TARIFA_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_TARIFA_ENVIO_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As TarifaEnvio
            Dim entidad As TarifaEnvio = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_OBTENER_TARIFA_ENVIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_TARIFA_ENVIO_ID", OracleDbType.Int32).Value = id
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

        Public Function Listar() As List(Of TarifaEnvio)
            Dim lista As New List(Of TarifaEnvio)

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_LISTAR_TARIFAS_ENVIO", cn)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of TarifaEnvio)
            Dim lista As New List(Of TarifaEnvio)

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_TARIFA_ENVIO.SP_BUSCAR_TARIFAS_ENVIO", cn)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As TarifaEnvio
            Dim entidad As New TarifaEnvio With {
                .TarifaEnvioId = Convert.ToInt32(dr("TARIFA_ENVIO_ID")),
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

            If TieneColumna(dr, "TIPO_ENTREGA_ID") AndAlso Not IsDBNull(dr("TIPO_ENTREGA_ID")) Then
                entidad.TipoEntregaId = Convert.ToInt32(dr("TIPO_ENTREGA_ID"))
            Else
                entidad.TipoEntregaId = Nothing
            End If

            If TieneColumna(dr, "TIPO_ENTREGA") AndAlso Not IsDBNull(dr("TIPO_ENTREGA")) Then
                entidad.TipoEntrega = dr("TIPO_ENTREGA").ToString()
            Else
                entidad.TipoEntrega = Nothing
            End If

            If Not IsDBNull(dr("PESO_DESDE_KG")) Then
                entidad.PesoDesdeKg = Convert.ToDecimal(dr("PESO_DESDE_KG"))
            Else
                entidad.PesoDesdeKg = Nothing
            End If

            If Not IsDBNull(dr("PESO_HASTA_KG")) Then
                entidad.PesoHastaKg = Convert.ToDecimal(dr("PESO_HASTA_KG"))
            Else
                entidad.PesoHastaKg = Nothing
            End If

            If Not IsDBNull(dr("COSTO")) Then
                entidad.Costo = Convert.ToDecimal(dr("COSTO"))
            Else
                entidad.Costo = Nothing
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
