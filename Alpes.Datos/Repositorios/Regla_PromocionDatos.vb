Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class ReglaPromocionDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As ReglaPromocion) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_INSERTAR_REGLA_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    If entidad.PromocionId.HasValue Then
                        cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = entidad.PromocionId.Value
                    Else
                        cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.MinCompra.HasValue Then
                        cmd.Parameters.Add("P_MIN_COMPRA", OracleDbType.Decimal).Value = entidad.MinCompra.Value
                    Else
                        cmd.Parameters.Add("P_MIN_COMPRA", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If entidad.MinItems.HasValue Then
                        cmd.Parameters.Add("P_MIN_ITEMS", OracleDbType.Int32).Value = entidad.MinItems.Value
                    Else
                        cmd.Parameters.Add("P_MIN_ITEMS", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.AplicaTipoProducto) Then
                        cmd.Parameters.Add("P_APLICA_TIPO_PRODUCTO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_APLICA_TIPO_PRODUCTO", OracleDbType.Varchar2).Value = entidad.AplicaTipoProducto
                    End If

                    If entidad.TopeDescuento.HasValue Then
                        cmd.Parameters.Add("P_TOPE_DESCUENTO", OracleDbType.Decimal).Value = entidad.TopeDescuento.Value
                    Else
                        cmd.Parameters.Add("P_TOPE_DESCUENTO", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_REGLA_PROMOCION_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_REGLA_PROMOCION_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As ReglaPromocion)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_ACTUALIZAR_REGLA_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_REGLA_PROMOCION_ID", OracleDbType.Int32).Value = entidad.ReglaPromocionId

                    If entidad.PromocionId.HasValue Then
                        cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = entidad.PromocionId.Value
                    Else
                        cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.MinCompra.HasValue Then
                        cmd.Parameters.Add("P_MIN_COMPRA", OracleDbType.Decimal).Value = entidad.MinCompra.Value
                    Else
                        cmd.Parameters.Add("P_MIN_COMPRA", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If entidad.MinItems.HasValue Then
                        cmd.Parameters.Add("P_MIN_ITEMS", OracleDbType.Int32).Value = entidad.MinItems.Value
                    Else
                        cmd.Parameters.Add("P_MIN_ITEMS", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.AplicaTipoProducto) Then
                        cmd.Parameters.Add("P_APLICA_TIPO_PRODUCTO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_APLICA_TIPO_PRODUCTO", OracleDbType.Varchar2).Value = entidad.AplicaTipoProducto
                    End If

                    If entidad.TopeDescuento.HasValue Then
                        cmd.Parameters.Add("P_TOPE_DESCUENTO", OracleDbType.Decimal).Value = entidad.TopeDescuento.Value
                    Else
                        cmd.Parameters.Add("P_TOPE_DESCUENTO", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_ELIMINAR_REGLA_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_REGLA_PROMOCION_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As ReglaPromocion
            Dim entidad As ReglaPromocion = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_OBTENER_REGLA_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_REGLA_PROMOCION_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearReglaPromocion(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of ReglaPromocion)
            Dim lista As New List(Of ReglaPromocion)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_LISTAR_REGLAS_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearReglaPromocion(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ReglaPromocion)
            Dim lista As New List(Of ReglaPromocion)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_BUSCAR_REGLAS_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearReglaPromocion(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearReglaPromocion(ByVal dr As OracleDataReader) As ReglaPromocion
            Dim entidad As New ReglaPromocion()

            entidad.ReglaPromocionId = LeerEntero(dr, "REGLA_PROMOCION_ID")
            entidad.PromocionId = LeerEnteroNullable(dr, "PROMOCION_ID")
            entidad.Promocion = LeerTexto(dr, "PROMOCION")
            entidad.MinCompra = LeerDecimalNullable(dr, "MIN_COMPRA")
            entidad.MinItems = LeerEnteroNullable(dr, "MIN_ITEMS")
            entidad.AplicaTipoProducto = LeerTexto(dr, "APLICA_TIPO_PRODUCTO")
            entidad.TopeDescuento = LeerDecimalNullable(dr, "TOPE_DESCUENTO")
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