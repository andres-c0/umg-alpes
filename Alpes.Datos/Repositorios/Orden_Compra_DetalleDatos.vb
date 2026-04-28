Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Compras

Namespace Repositorios
    Public Class Orden_Compra_DetalleDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Orden_Compra_Detalle) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA_DETALLE.SP_INSERTAR_ORDEN_COMPRA_DETALLE", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID", OracleDbType.Int32).Value = entidad.OrdenCompraId
                    cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Decimal).Value = entidad.Cantidad
                    cmd.Parameters.Add("P_COSTO_UNITARIO", OracleDbType.Decimal).Value = entidad.CostoUnitario
                    cmd.Parameters.Add("P_SUBTOTAL_LINEA", OracleDbType.Decimal).Value = entidad.SubtotalLinea
                    cmd.Parameters.Add("P_ORDEN_COMPRA_DET_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_ORDEN_COMPRA_DET_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Orden_Compra_Detalle)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA_DETALLE.SP_ACTUALIZAR_ORDEN_COMPRA_DETALLE", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ORDEN_COMPRA_DET_ID", OracleDbType.Int32).Value = entidad.OrdenCompraDetId
                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID", OracleDbType.Int32).Value = entidad.OrdenCompraId
                    cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Decimal).Value = entidad.Cantidad
                    cmd.Parameters.Add("P_COSTO_UNITARIO", OracleDbType.Decimal).Value = entidad.CostoUnitario
                    cmd.Parameters.Add("P_SUBTOTAL_LINEA", OracleDbType.Decimal).Value = entidad.SubtotalLinea

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA_DETALLE.SP_ELIMINAR_ORDEN_COMPRA_DETALLE", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ORDEN_COMPRA_DET_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Orden_Compra_Detalle
            Dim entidad As Orden_Compra_Detalle = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA_DETALLE.SP_OBTENER_ORDEN_COMPRA_DETALLE", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ORDEN_COMPRA_DET_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearOrdenCompraDetalle(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Orden_Compra_Detalle)
            Dim lista As New List(Of Orden_Compra_Detalle)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA_DETALLE.SP_LISTAR_ORDEN_COMPRA_DETALLE", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearOrdenCompraDetalle(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Orden_Compra_Detalle)
            Dim lista As New List(Of Orden_Compra_Detalle)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA_DETALLE.SP_BUSCAR_ORDEN_COMPRA_DETALLE", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearOrdenCompraDetalle(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearOrdenCompraDetalle(ByVal dr As OracleDataReader) As Orden_Compra_Detalle
            Dim entidad As New Orden_Compra_Detalle()

            If TieneColumna(dr, "ORDEN_COMPRA_DET_ID") AndAlso Not IsDBNull(dr("ORDEN_COMPRA_DET_ID")) Then
                entidad.OrdenCompraDetId = Convert.ToInt32(dr("ORDEN_COMPRA_DET_ID"))
            End If

            If TieneColumna(dr, "ORDEN_COMPRA_ID") AndAlso Not IsDBNull(dr("ORDEN_COMPRA_ID")) Then
                entidad.OrdenCompraId = Convert.ToInt32(dr("ORDEN_COMPRA_ID"))
            End If

            If TieneColumna(dr, "MP_ID") AndAlso Not IsDBNull(dr("MP_ID")) Then
                entidad.MpId = Convert.ToInt32(dr("MP_ID"))
            End If

            If TieneColumna(dr, "MATERIA_PRIMA_NOMBRE") AndAlso Not IsDBNull(dr("MATERIA_PRIMA_NOMBRE")) Then
                entidad.MateriaPrimaNombre = dr("MATERIA_PRIMA_NOMBRE").ToString()
            Else
                entidad.MateriaPrimaNombre = String.Empty
            End If

            If TieneColumna(dr, "CANTIDAD") AndAlso Not IsDBNull(dr("CANTIDAD")) Then
                entidad.Cantidad = Convert.ToDecimal(dr("CANTIDAD"))
            End If

            If TieneColumna(dr, "COSTO_UNITARIO") AndAlso Not IsDBNull(dr("COSTO_UNITARIO")) Then
                entidad.CostoUnitario = Convert.ToDecimal(dr("COSTO_UNITARIO"))
            End If

            If TieneColumna(dr, "SUBTOTAL_LINEA") AndAlso Not IsDBNull(dr("SUBTOTAL_LINEA")) Then
                entidad.SubtotalLinea = Convert.ToDecimal(dr("SUBTOTAL_LINEA"))
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