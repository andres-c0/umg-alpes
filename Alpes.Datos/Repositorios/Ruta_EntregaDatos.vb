Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Logistica

Namespace Repositorios
    Public Class Ruta_EntregaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Ruta_Entrega) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RUTA_ENTREGA.SP_INSERTAR_RUTA_ENTREGA", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_VEHICULO_ID", OracleDbType.Int32).Value = entidad.VehiculoId
                    cmd.Parameters.Add("P_FECHA_RUTA", OracleDbType.Date).Value = entidad.FechaRuta

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    cmd.Parameters.Add("P_ESTADO_RUTA", OracleDbType.Varchar2).Value = entidad.EstadoRuta
                    cmd.Parameters.Add("P_RUTA_ENTREGA_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_RUTA_ENTREGA_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Ruta_Entrega)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RUTA_ENTREGA.SP_ACTUALIZAR_RUTA_ENTREGA", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_RUTA_ENTREGA_ID", OracleDbType.Int32).Value = entidad.RutaEntregaId
                    cmd.Parameters.Add("P_VEHICULO_ID", OracleDbType.Int32).Value = entidad.VehiculoId
                    cmd.Parameters.Add("P_FECHA_RUTA", OracleDbType.Date).Value = entidad.FechaRuta

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    cmd.Parameters.Add("P_ESTADO_RUTA", OracleDbType.Varchar2).Value = entidad.EstadoRuta

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RUTA_ENTREGA.SP_ELIMINAR_RUTA_ENTREGA", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_RUTA_ENTREGA_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Ruta_Entrega
            Dim entidad As Ruta_Entrega = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RUTA_ENTREGA.SP_OBTENER_RUTA_ENTREGA", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_RUTA_ENTREGA_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearRutaEntrega(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Ruta_Entrega)
            Dim lista As New List(Of Ruta_Entrega)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RUTA_ENTREGA.SP_LISTAR_RUTA_ENTREGA", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearRutaEntrega(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Ruta_Entrega)
            Dim lista As New List(Of Ruta_Entrega)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RUTA_ENTREGA.SP_BUSCAR_RUTA_ENTREGA", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearRutaEntrega(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearRutaEntrega(ByVal dr As OracleDataReader) As Ruta_Entrega
            Dim entidad As New Ruta_Entrega()

            If TieneColumna(dr, "RUTA_ENTREGA_ID") AndAlso Not IsDBNull(dr("RUTA_ENTREGA_ID")) Then
                entidad.RutaEntregaId = Convert.ToInt32(dr("RUTA_ENTREGA_ID"))
            End If

            If TieneColumna(dr, "VEHICULO_ID") AndAlso Not IsDBNull(dr("VEHICULO_ID")) Then
                entidad.VehiculoId = Convert.ToInt32(dr("VEHICULO_ID"))
            End If

            If TieneColumna(dr, "PLACA_VEHICULO") AndAlso Not IsDBNull(dr("PLACA_VEHICULO")) Then
                entidad.PlacaVehiculo = dr("PLACA_VEHICULO").ToString()
            Else
                entidad.PlacaVehiculo = String.Empty
            End If

            If TieneColumna(dr, "FECHA_RUTA") AndAlso Not IsDBNull(dr("FECHA_RUTA")) Then
                entidad.FechaRuta = Convert.ToDateTime(dr("FECHA_RUTA"))
            End If

            If TieneColumna(dr, "DESCRIPCION") AndAlso Not IsDBNull(dr("DESCRIPCION")) Then
                entidad.Descripcion = dr("DESCRIPCION").ToString()
            Else
                entidad.Descripcion = String.Empty
            End If

            If TieneColumna(dr, "ESTADO_RUTA") AndAlso Not IsDBNull(dr("ESTADO_RUTA")) Then
                entidad.EstadoRuta = dr("ESTADO_RUTA").ToString()
            Else
                entidad.EstadoRuta = String.Empty
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

            If TieneColumna(dr, "ESTADO") AndAlso Not IsDBNull(dr("ESTADO")) Then
                entidad.Estado = dr("ESTADO").ToString()
            Else
                entidad.Estado = String.Empty
            End If

            Return entidad
        End Function

        Private Function TieneColumna(ByVal dr As IDataRecord, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If String.Equals(dr.GetName(i), nombreColumna, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            Next

            Return False
        End Function

    End Class
End Namespace