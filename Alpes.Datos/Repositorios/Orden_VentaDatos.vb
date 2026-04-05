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

        Public Function Insertar(entidad As Orden_Venta) As Integer
            Using conn = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_INSERTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NUM_ORDEN", entidad.num_orden)
                    cmd.Parameters.Add("P_CLI_ID", entidad.cli_id)
                    cmd.Parameters.Add("P_ESTADO_ORDEN_ID", entidad.estado_orden_id)
                    cmd.Parameters.Add("P_FECHA_ORDEN", entidad.fecha_orden)
                    cmd.Parameters.Add("P_SUBTOTAL", entidad.subtotal)
                    cmd.Parameters.Add("P_DESCUENTO", entidad.descuento)
                    cmd.Parameters.Add("P_IMPUESTO", entidad.impuesto)
                    cmd.Parameters.Add("P_TOTAL", entidad.total)
                    cmd.Parameters.Add("P_MONEDA", entidad.moneda)
                    cmd.Parameters.Add("P_DIRECCION_ENVIO_SNAPSHOT", entidad.direccion_envio_snapshot)
                    cmd.Parameters.Add("P_OBSERVACIONES", entidad.observaciones)
                    cmd.Parameters.Add("P_ESTADO", entidad.estado)

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(cmd.Parameters("P_ID").Value)
                End Using
            End Using
        End Function

        Public Sub Actualizar(entidad As Orden_Venta)
            Using conn = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_ACTUALIZAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", entidad.orden_venta_id)
                    cmd.Parameters.Add("P_NUM_ORDEN", entidad.num_orden)
                    cmd.Parameters.Add("P_CLI_ID", entidad.cli_id)
                    cmd.Parameters.Add("P_ESTADO_ORDEN_ID", entidad.estado_orden_id)
                    cmd.Parameters.Add("P_FECHA_ORDEN", entidad.fecha_orden)
                    cmd.Parameters.Add("P_SUBTOTAL", entidad.subtotal)
                    cmd.Parameters.Add("P_DESCUENTO", entidad.descuento)
                    cmd.Parameters.Add("P_IMPUESTO", entidad.impuesto)
                    cmd.Parameters.Add("P_TOTAL", entidad.total)
                    cmd.Parameters.Add("P_MONEDA", entidad.moneda)
                    cmd.Parameters.Add("P_DIRECCION_ENVIO_SNAPSHOT", entidad.direccion_envio_snapshot)
                    cmd.Parameters.Add("P_OBSERVACIONES", entidad.observaciones)
                    cmd.Parameters.Add("P_ESTADO", entidad.estado)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(id As Integer)
            Using conn = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_ELIMINAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", id)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(id As Integer) As Orden_Venta
            Using conn = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_OBTENER", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", id)
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr = cmd.ExecuteReader()
                        If dr.Read() Then
                            Return Mapear(dr)
                        End If
                    End Using
                End Using
            End Using

            Return Nothing
        End Function

        Public Function Listar() As List(Of Orden_Venta)
            Dim lista As New List(Of Orden_Venta)

            Using conn = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_LISTAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(valor As String) As List(Of Orden_Venta)
            Dim lista As New List(Of Orden_Venta)

            Using conn = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_VENTA.SP_BUSCAR", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_VALOR", valor)
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function Mapear(dr As OracleDataReader) As Orden_Venta
            Return New Orden_Venta With {
                .orden_venta_id = Convert.ToInt32(dr("orden_venta_id")),
                .num_orden = dr("num_orden").ToString(),
                .cli_id = Convert.ToInt32(dr("cli_id")),
                .estado_orden_id = Convert.ToInt32(dr("estado_orden_id")),
                .fecha_orden = Convert.ToDateTime(dr("fecha_orden")),
                .subtotal = Convert.ToDecimal(dr("subtotal")),
                .descuento = Convert.ToDecimal(dr("descuento")),
                .impuesto = Convert.ToDecimal(dr("impuesto")),
                .total = Convert.ToDecimal(dr("total")),
                .moneda = dr("moneda").ToString(),
                .direccion_envio_snapshot = dr("direccion_envio_snapshot").ToString(),
                .observaciones = dr("observaciones").ToString(),
                .created_at = Convert.ToDateTime(dr("created_at")),
                .updated_at = If(IsDBNull(dr("updated_at")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("updated_at"))),
                .estado = dr("estado").ToString()
            }
        End Function

    End Class
End Namespace