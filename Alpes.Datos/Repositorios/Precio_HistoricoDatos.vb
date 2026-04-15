Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Inventario

Namespace Repositorios
    Public Class Precio_HistoricoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Precio_Historico) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRECIO_HISTORICO.SP_INSERTAR_PRECIO_HISTORICO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_PRECIO", OracleDbType.Decimal).Value = entidad.Precio
                    cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio

                    If entidad.VigenciaFin.HasValue Then
                        cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin.Value
                    Else
                        cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Motivo) Then
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = entidad.Motivo
                    End If

                    cmd.Parameters.Add("P_PRECIO_HIST_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_PRECIO_HIST_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Precio_Historico)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRECIO_HISTORICO.SP_ACTUALIZAR_PRECIO_HISTORICO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_PRECIO_HIST_ID", OracleDbType.Int32).Value = entidad.PrecioHistId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_PRECIO", OracleDbType.Decimal).Value = entidad.Precio
                    cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio

                    If entidad.VigenciaFin.HasValue Then
                        cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin.Value
                    Else
                        cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Motivo) Then
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = entidad.Motivo
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRECIO_HISTORICO.SP_ELIMINAR_PRECIO_HISTORICO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_PRECIO_HIST_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Precio_Historico
            Dim entidad As Precio_Historico = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRECIO_HISTORICO.SP_OBTENER_PRECIO_HISTORICO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_PRECIO_HIST_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearPrecioHistorico(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Precio_Historico)
            Dim lista As New List(Of Precio_Historico)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRECIO_HISTORICO.SP_LISTAR_PRECIO_HISTORICO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearPrecioHistorico(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Precio_Historico)
            Dim lista As New List(Of Precio_Historico)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRECIO_HISTORICO.SP_BUSCAR_PRECIO_HISTORICO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearPrecioHistorico(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearPrecioHistorico(ByVal dr As OracleDataReader) As Precio_Historico
            Dim entidad As New Precio_Historico()

            If TieneColumna(dr, "PRECIO_HIST_ID") AndAlso Not IsDBNull(dr("PRECIO_HIST_ID")) Then
                entidad.PrecioHistId = Convert.ToInt32(dr("PRECIO_HIST_ID"))
            End If

            If TieneColumna(dr, "PRODUCTO_ID") AndAlso Not IsDBNull(dr("PRODUCTO_ID")) Then
                entidad.ProductoId = Convert.ToInt32(dr("PRODUCTO_ID"))
            End If

            If TieneColumna(dr, "NOMBRE_PRODUCTO") AndAlso Not IsDBNull(dr("NOMBRE_PRODUCTO")) Then
                entidad.NombreProducto = dr("NOMBRE_PRODUCTO").ToString()
            Else
                entidad.NombreProducto = String.Empty
            End If

            If TieneColumna(dr, "PRECIO") AndAlso Not IsDBNull(dr("PRECIO")) Then
                entidad.Precio = Convert.ToDecimal(dr("PRECIO"))
            End If

            If TieneColumna(dr, "VIGENCIA_INICIO") AndAlso Not IsDBNull(dr("VIGENCIA_INICIO")) Then
                entidad.VigenciaInicio = Convert.ToDateTime(dr("VIGENCIA_INICIO"))
            End If

            If TieneColumna(dr, "VIGENCIA_FIN") AndAlso Not IsDBNull(dr("VIGENCIA_FIN")) Then
                entidad.VigenciaFin = Convert.ToDateTime(dr("VIGENCIA_FIN"))
            Else
                entidad.VigenciaFin = Nothing
            End If

            If TieneColumna(dr, "MOTIVO") AndAlso Not IsDBNull(dr("MOTIVO")) Then
                entidad.Motivo = dr("MOTIVO").ToString()
            Else
                entidad.Motivo = String.Empty
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