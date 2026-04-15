Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Compras

Namespace Repositorios
    Public Class Orden_CompraDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Orden_Compra) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA.SP_INSERTAR_ORDEN_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    If String.IsNullOrWhiteSpace(entidad.NumOc) Then
                        cmd.Parameters.Add("P_NUM_OC", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_NUM_OC", OracleDbType.Varchar2).Value = entidad.NumOc
                    End If

                    cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = entidad.ProvId
                    cmd.Parameters.Add("P_ESTADO_OC_ID", OracleDbType.Int32).Value = entidad.EstadoOcId
                    cmd.Parameters.Add("P_CONDICION_PAGO_ID", OracleDbType.Int32).Value = entidad.CondicionPagoId
                    cmd.Parameters.Add("P_FECHA_OC", OracleDbType.Date).Value = entidad.FechaOc
                    cmd.Parameters.Add("P_SUBTOTAL", OracleDbType.Decimal).Value = entidad.Subtotal
                    cmd.Parameters.Add("P_IMPUESTO", OracleDbType.Decimal).Value = entidad.Impuesto
                    cmd.Parameters.Add("P_TOTAL", OracleDbType.Decimal).Value = entidad.Total

                    If String.IsNullOrWhiteSpace(entidad.Observaciones) Then
                        cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = entidad.Observaciones
                    End If

                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_ORDEN_COMPRA_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Orden_Compra)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA.SP_ACTUALIZAR_ORDEN_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID", OracleDbType.Int32).Value = entidad.OrdenCompraId

                    If String.IsNullOrWhiteSpace(entidad.NumOc) Then
                        cmd.Parameters.Add("P_NUM_OC", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_NUM_OC", OracleDbType.Varchar2).Value = entidad.NumOc
                    End If

                    cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = entidad.ProvId
                    cmd.Parameters.Add("P_ESTADO_OC_ID", OracleDbType.Int32).Value = entidad.EstadoOcId
                    cmd.Parameters.Add("P_CONDICION_PAGO_ID", OracleDbType.Int32).Value = entidad.CondicionPagoId
                    cmd.Parameters.Add("P_FECHA_OC", OracleDbType.Date).Value = entidad.FechaOc
                    cmd.Parameters.Add("P_SUBTOTAL", OracleDbType.Decimal).Value = entidad.Subtotal
                    cmd.Parameters.Add("P_IMPUESTO", OracleDbType.Decimal).Value = entidad.Impuesto
                    cmd.Parameters.Add("P_TOTAL", OracleDbType.Decimal).Value = entidad.Total

                    If String.IsNullOrWhiteSpace(entidad.Observaciones) Then
                        cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = entidad.Observaciones
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA.SP_ELIMINAR_ORDEN_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Orden_Compra
            Dim entidad As Orden_Compra = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA.SP_OBTENER_ORDEN_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID", OracleDbType.Int32).Value = id
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

        Public Function Listar() As List(Of Orden_Compra)
            Dim lista As New List(Of Orden_Compra)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA.SP_LISTAR_ORDENES_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            Dim itemBase As Orden_Compra = MapearOrdenCompraLista(dr)
                            Dim itemDetalle As Orden_Compra = ObtenerPorId(itemBase.OrdenCompraId)

                            If itemDetalle IsNot Nothing Then
                                If String.IsNullOrWhiteSpace(itemDetalle.NumOc) Then
                                    itemDetalle.NumOc = itemBase.NumOc
                                End If

                                If String.IsNullOrWhiteSpace(itemDetalle.RazonSocial) Then
                                    itemDetalle.RazonSocial = itemBase.RazonSocial
                                End If

                                If String.IsNullOrWhiteSpace(itemDetalle.EstadoOcCodigo) Then
                                    itemDetalle.EstadoOcCodigo = itemBase.EstadoOcCodigo
                                End If

                                If itemDetalle.Total <= 0D Then
                                    itemDetalle.Total = itemBase.Total
                                End If

                                If itemDetalle.FechaOc = DateTime.MinValue Then
                                    itemDetalle.FechaOc = itemBase.FechaOc
                                End If

                                If String.IsNullOrWhiteSpace(itemDetalle.Estado) Then
                                    itemDetalle.Estado = itemBase.Estado
                                End If

                                lista.Add(itemDetalle)
                            Else
                                lista.Add(itemBase)
                            End If
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Orden_Compra)
            Dim lista As New List(Of Orden_Compra)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_COMPRA.SP_BUSCAR_ORDENES_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            Dim itemBase As Orden_Compra = MapearOrdenCompraLista(dr)
                            Dim itemDetalle As Orden_Compra = ObtenerPorId(itemBase.OrdenCompraId)

                            If itemDetalle IsNot Nothing Then
                                If String.IsNullOrWhiteSpace(itemDetalle.NumOc) Then
                                    itemDetalle.NumOc = itemBase.NumOc
                                End If

                                If String.IsNullOrWhiteSpace(itemDetalle.RazonSocial) Then
                                    itemDetalle.RazonSocial = itemBase.RazonSocial
                                End If

                                If String.IsNullOrWhiteSpace(itemDetalle.EstadoOcCodigo) Then
                                    itemDetalle.EstadoOcCodigo = itemBase.EstadoOcCodigo
                                End If

                                If itemDetalle.Total <= 0D Then
                                    itemDetalle.Total = itemBase.Total
                                End If

                                If itemDetalle.FechaOc = DateTime.MinValue Then
                                    itemDetalle.FechaOc = itemBase.FechaOc
                                End If

                                If String.IsNullOrWhiteSpace(itemDetalle.Estado) Then
                                    itemDetalle.Estado = itemBase.Estado
                                End If

                                lista.Add(itemDetalle)
                            Else
                                lista.Add(itemBase)
                            End If
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearOrdenCompraLista(ByVal dr As OracleDataReader) As Orden_Compra
            Dim entidad As New Orden_Compra()

            If Not IsDBNull(dr("ORDEN_COMPRA_ID")) Then
                entidad.OrdenCompraId = Convert.ToInt32(dr("ORDEN_COMPRA_ID"))
            End If

            entidad.NumOc = If(IsDBNull(dr("NUM_OC")), String.Empty, dr("NUM_OC").ToString())
            entidad.RazonSocial = If(IsDBNull(dr("RAZON_SOCIAL")), String.Empty, dr("RAZON_SOCIAL").ToString())

            If Not IsDBNull(dr("FECHA_OC")) Then
                entidad.FechaOc = Convert.ToDateTime(dr("FECHA_OC"))
            End If

            If Not IsDBNull(dr("TOTAL")) Then
                entidad.Total = Convert.ToDecimal(dr("TOTAL"))
            End If

            entidad.Estado = If(IsDBNull(dr("ESTADO")), String.Empty, dr("ESTADO").ToString())

            If TieneColumna(dr, "ESTADO_OC") AndAlso Not IsDBNull(dr("ESTADO_OC")) Then
                entidad.EstadoOcCodigo = dr("ESTADO_OC").ToString()
            Else
                entidad.EstadoOcCodigo = String.Empty
            End If

            Return entidad
        End Function

        Private Function MapearOrdenCompraDetalle(ByVal dr As OracleDataReader) As Orden_Compra
            Dim entidad As New Orden_Compra()

            If Not IsDBNull(dr("ORDEN_COMPRA_ID")) Then
                entidad.OrdenCompraId = Convert.ToInt32(dr("ORDEN_COMPRA_ID"))
            End If

            entidad.NumOc = If(IsDBNull(dr("NUM_OC")), String.Empty, dr("NUM_OC").ToString())

            If Not IsDBNull(dr("PROV_ID")) Then
                entidad.ProvId = Convert.ToInt32(dr("PROV_ID"))
            End If

            entidad.RazonSocial = If(IsDBNull(dr("RAZON_SOCIAL")), String.Empty, dr("RAZON_SOCIAL").ToString())

            If Not IsDBNull(dr("ESTADO_OC_ID")) Then
                entidad.EstadoOcId = Convert.ToInt32(dr("ESTADO_OC_ID"))
            End If

            entidad.EstadoOcCodigo = If(IsDBNull(dr("ESTADO_OC_CODIGO")), String.Empty, dr("ESTADO_OC_CODIGO").ToString())

            If Not IsDBNull(dr("CONDICION_PAGO_ID")) Then
                entidad.CondicionPagoId = Convert.ToInt32(dr("CONDICION_PAGO_ID"))
            End If

            entidad.CondicionPagoNombre = If(IsDBNull(dr("CONDICION_PAGO_NOMBRE")), String.Empty, dr("CONDICION_PAGO_NOMBRE").ToString())

            If Not IsDBNull(dr("FECHA_OC")) Then
                entidad.FechaOc = Convert.ToDateTime(dr("FECHA_OC"))
            End If

            If Not IsDBNull(dr("SUBTOTAL")) Then
                entidad.Subtotal = Convert.ToDecimal(dr("SUBTOTAL"))
            End If

            If Not IsDBNull(dr("IMPUESTO")) Then
                entidad.Impuesto = Convert.ToDecimal(dr("IMPUESTO"))
            End If

            If Not IsDBNull(dr("TOTAL")) Then
                entidad.Total = Convert.ToDecimal(dr("TOTAL"))
            End If

            entidad.Observaciones = If(IsDBNull(dr("OBSERVACIONES")), String.Empty, dr("OBSERVACIONES").ToString())
            entidad.Estado = If(IsDBNull(dr("ESTADO")), String.Empty, dr("ESTADO").ToString())

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