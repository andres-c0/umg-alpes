Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class HistorialPromocionDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As HistorialPromocion) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_INSERTAR_HISTORIAL_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    If entidad.OrdenVentaId.HasValue Then
                        cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId.Value
                    Else
                        cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.PromocionId.HasValue Then
                        cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = entidad.PromocionId.Value
                    Else
                        cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.MontoDescuentoSnapshot.HasValue Then
                        cmd.Parameters.Add("P_MONTO_DESCUENTO_SNAPSHOT", OracleDbType.Decimal).Value = entidad.MontoDescuentoSnapshot.Value
                    Else
                        cmd.Parameters.Add("P_MONTO_DESCUENTO_SNAPSHOT", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_HISTORIAL_PROMOCION_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_HISTORIAL_PROMOCION_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As HistorialPromocion)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_ACTUALIZAR_HISTORIAL_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HISTORIAL_PROMOCION_ID", OracleDbType.Int32).Value = entidad.HistorialPromocionId

                    If entidad.OrdenVentaId.HasValue Then
                        cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId.Value
                    Else
                        cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.PromocionId.HasValue Then
                        cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = entidad.PromocionId.Value
                    Else
                        cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.MontoDescuentoSnapshot.HasValue Then
                        cmd.Parameters.Add("P_MONTO_DESCUENTO_SNAPSHOT", OracleDbType.Decimal).Value = entidad.MontoDescuentoSnapshot.Value
                    Else
                        cmd.Parameters.Add("P_MONTO_DESCUENTO_SNAPSHOT", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_ELIMINAR_HISTORIAL_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HISTORIAL_PROMOCION_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As HistorialPromocion
            Dim entidad As HistorialPromocion = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_OBTENER_HISTORIAL_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HISTORIAL_PROMOCION_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearHistorialPromocion(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of HistorialPromocion)
            Dim lista As New List(Of HistorialPromocion)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_LISTAR_HISTORIAL_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearHistorialPromocion(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of HistorialPromocion)
            Dim lista As New List(Of HistorialPromocion)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_PROMOCION.SP_BUSCAR_HISTORIAL_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearHistorialPromocion(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearHistorialPromocion(ByVal dr As OracleDataReader) As HistorialPromocion
            Dim entidad As New HistorialPromocion()

            entidad.HistorialPromocionId = LeerEntero(dr, "HISTORIAL_PROMOCION_ID")
            entidad.OrdenVentaId = LeerEnteroNullable(dr, "ORDEN_VENTA_ID")
            entidad.PromocionId = LeerEnteroNullable(dr, "PROMOCION_ID")
            entidad.MontoDescuentoSnapshot = LeerDecimalNullable(dr, "MONTO_DESCUENTO_SNAPSHOT")
            entidad.CreatedAt = LeerFechaNullable(dr, "CREATED_AT")
            entidad.UpdatedAt = LeerFechaNullable(dr, "UPDATED_AT")
            entidad.Estado = LeerTexto(dr, "ESTADO")

            Return entidad
        End Function

        Private Function LeerTexto(ByVal dr As OracleDataReader, ByVal columna As String) As String
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return dr(columna).ToString()
            End If

            Return String.Empty
        End Function

        Private Function LeerEntero(ByVal dr As OracleDataReader, ByVal columna As String) As Integer
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToInt32(dr(columna))
            End If

            Return 0
        End Function

        Private Function LeerEnteroNullable(ByVal dr As OracleDataReader, ByVal columna As String) As Integer?
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToInt32(dr(columna))
            End If

            Return Nothing
        End Function

        Private Function LeerDecimalNullable(ByVal dr As OracleDataReader, ByVal columna As String) As Decimal?
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToDecimal(dr(columna))
            End If

            Return Nothing
        End Function

        Private Function LeerFechaNullable(ByVal dr As OracleDataReader, ByVal columna As String) As DateTime?
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToDateTime(dr(columna))
            End If

            Return Nothing
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