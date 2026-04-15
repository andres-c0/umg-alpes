Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class Orden_VentaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Orden_Venta) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NUM_ORDEN", OracleDbType.Varchar2).Value = entidad.NumOrden
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                    cmd.Parameters.Add("P_ESTADO_ORDEN_ID", OracleDbType.Int32).Value = entidad.EstadoOrdenId
                    cmd.Parameters.Add("P_FECHA_ORDEN", OracleDbType.TimeStamp).Value = entidad.FechaOrden
                    cmd.Parameters.Add("P_SUBTOTAL", OracleDbType.Decimal).Value = entidad.Subtotal
                    cmd.Parameters.Add("P_DESCUENTO", OracleDbType.Decimal).Value = entidad.Descuento
                    cmd.Parameters.Add("P_IMPUESTO", OracleDbType.Decimal).Value = entidad.Impuesto
                    cmd.Parameters.Add("P_TOTAL", OracleDbType.Decimal).Value = entidad.Total

                    If String.IsNullOrWhiteSpace(entidad.Moneda) Then
                        cmd.Parameters.Add("P_MONEDA", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MONEDA", OracleDbType.Varchar2).Value = entidad.Moneda
                    End If

                    If String.IsNullOrWhiteSpace(entidad.DireccionEnvioSnapshot) Then
                        cmd.Parameters.Add("P_DIRECCION_ENVIO_SNAPSHOT", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DIRECCION_ENVIO_SNAPSHOT", OracleDbType.Varchar2).Value = entidad.DireccionEnvioSnapshot
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Observaciones) Then
                        cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = entidad.Observaciones
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Estado) Then
                        cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado
                    End If

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Orden_Venta)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
                    cmd.Parameters.Add("P_NUM_ORDEN", OracleDbType.Varchar2).Value = entidad.NumOrden
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                    cmd.Parameters.Add("P_ESTADO_ORDEN_ID", OracleDbType.Int32).Value = entidad.EstadoOrdenId
                    cmd.Parameters.Add("P_FECHA_ORDEN", OracleDbType.TimeStamp).Value = entidad.FechaOrden
                    cmd.Parameters.Add("P_SUBTOTAL", OracleDbType.Decimal).Value = entidad.Subtotal
                    cmd.Parameters.Add("P_DESCUENTO", OracleDbType.Decimal).Value = entidad.Descuento
                    cmd.Parameters.Add("P_IMPUESTO", OracleDbType.Decimal).Value = entidad.Impuesto
                    cmd.Parameters.Add("P_TOTAL", OracleDbType.Decimal).Value = entidad.Total

                    If String.IsNullOrWhiteSpace(entidad.Moneda) Then
                        cmd.Parameters.Add("P_MONEDA", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MONEDA", OracleDbType.Varchar2).Value = entidad.Moneda
                    End If

                    If String.IsNullOrWhiteSpace(entidad.DireccionEnvioSnapshot) Then
                        cmd.Parameters.Add("P_DIRECCION_ENVIO_SNAPSHOT", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DIRECCION_ENVIO_SNAPSHOT", OracleDbType.Varchar2).Value = entidad.DireccionEnvioSnapshot
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Observaciones) Then
                        cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = entidad.Observaciones
                    End If

                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Orden_Venta
            Dim entidad As Orden_Venta = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_OBTENER", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearOrdenVenta(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Orden_Venta)
            Dim lista As New List(Of Orden_Venta)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearOrdenVenta(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Orden_Venta)
            Dim lista As New List(Of Orden_Venta)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_BUSCAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearOrdenVenta(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearOrdenVenta(ByVal dr As OracleDataReader) As Orden_Venta
            Dim entidad As New Orden_Venta()

            entidad.OrdenVentaId = Convert.ToInt32(dr("orden_venta_id"))
            entidad.NumOrden = dr("num_orden").ToString()
            entidad.CliId = Convert.ToInt32(dr("cli_id"))
            entidad.EstadoOrdenId = Convert.ToInt32(dr("estado_orden_id"))
            entidad.FechaOrden = Convert.ToDateTime(dr("fecha_orden"))
            entidad.Subtotal = Convert.ToDecimal(dr("subtotal"))
            entidad.Descuento = Convert.ToDecimal(dr("descuento"))
            entidad.Impuesto = Convert.ToDecimal(dr("impuesto"))
            entidad.Total = Convert.ToDecimal(dr("total"))
            entidad.Moneda = If(IsDBNull(dr("moneda")), String.Empty, dr("moneda").ToString())
            entidad.DireccionEnvioSnapshot = If(IsDBNull(dr("direccion_envio_snapshot")), String.Empty, dr("direccion_envio_snapshot").ToString())
            entidad.Observaciones = If(IsDBNull(dr("observaciones")), String.Empty, dr("observaciones").ToString())
            entidad.CreatedAt = Convert.ToDateTime(dr("created_at"))

            If IsDBNull(dr("updated_at")) Then
                entidad.UpdatedAt = Nothing
            Else
                entidad.UpdatedAt = Convert.ToDateTime(dr("updated_at"))
            End If

            entidad.Estado = dr("estado").ToString()

            Return entidad
        End Function

    End Class
End Namespace