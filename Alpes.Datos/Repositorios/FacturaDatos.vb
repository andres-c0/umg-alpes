Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class FacturaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Factura) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_INSERTAR_FACTURA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
                    cmd.Parameters.Add("P_NUM_FACTURA", OracleDbType.Varchar2).Value = entidad.NumFactura
                    cmd.Parameters.Add("P_FECHA_EMISION", OracleDbType.Date).Value = entidad.FechaEmision

                    If String.IsNullOrWhiteSpace(entidad.NitFacturacion) Then
                        cmd.Parameters.Add("P_NIT_FACTURACION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_NIT_FACTURACION", OracleDbType.Varchar2).Value = entidad.NitFacturacion
                    End If

                    If String.IsNullOrWhiteSpace(entidad.DireccionFacturacionSnapshot) Then
                        cmd.Parameters.Add("P_DIRECCION_FACTURACION_SNAPSHOT", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DIRECCION_FACTURACION_SNAPSHOT", OracleDbType.Varchar2).Value = entidad.DireccionFacturacionSnapshot
                    End If

                    cmd.Parameters.Add("P_TOTAL_FACTURA_SNAPSHOT", OracleDbType.Decimal).Value = entidad.TotalFacturaSnapshot
                    cmd.Parameters.Add("P_FACTURA_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_FACTURA_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Factura)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_ACTUALIZAR_FACTURA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_ID", OracleDbType.Int32).Value = entidad.FacturaId
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
                    cmd.Parameters.Add("P_NUM_FACTURA", OracleDbType.Varchar2).Value = entidad.NumFactura
                    cmd.Parameters.Add("P_FECHA_EMISION", OracleDbType.Date).Value = entidad.FechaEmision

                    If String.IsNullOrWhiteSpace(entidad.NitFacturacion) Then
                        cmd.Parameters.Add("P_NIT_FACTURACION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_NIT_FACTURACION", OracleDbType.Varchar2).Value = entidad.NitFacturacion
                    End If

                    If String.IsNullOrWhiteSpace(entidad.DireccionFacturacionSnapshot) Then
                        cmd.Parameters.Add("P_DIRECCION_FACTURACION_SNAPSHOT", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DIRECCION_FACTURACION_SNAPSHOT", OracleDbType.Varchar2).Value = entidad.DireccionFacturacionSnapshot
                    End If

                    cmd.Parameters.Add("P_TOTAL_FACTURA_SNAPSHOT", OracleDbType.Decimal).Value = entidad.TotalFacturaSnapshot

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_ELIMINAR_FACTURA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Factura
            Dim entidad As Factura = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_OBTENER_FACTURA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearFactura(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Factura)
            Dim lista As New List(Of Factura)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_LISTAR_FACTURAS", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearFactura(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Factura)
            Dim lista As New List(Of Factura)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_BUSCAR_FACTURAS", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearFactura(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearFactura(ByVal dr As OracleDataReader) As Factura
            Dim entidad As New Factura()

            entidad.FacturaId = Convert.ToInt32(dr("FACTURA_ID"))
            entidad.NumFactura = dr("NUM_FACTURA").ToString()
            entidad.OrdenVentaId = Convert.ToInt32(dr("ORDEN_VENTA_ID"))
            entidad.FechaEmision = Convert.ToDateTime(dr("FECHA_EMISION"))
            entidad.TotalFacturaSnapshot = Convert.ToDecimal(dr("TOTAL_FACTURA_SNAPSHOT"))
            entidad.Estado = dr("ESTADO").ToString()

            If TieneColumna(dr, "NIT_FACTURACION") AndAlso Not IsDBNull(dr("NIT_FACTURACION")) Then
                entidad.NitFacturacion = dr("NIT_FACTURACION").ToString()
            Else
                entidad.NitFacturacion = String.Empty
            End If

            If TieneColumna(dr, "DIRECCION_FACTURACION_SNAPSHOT") AndAlso Not IsDBNull(dr("DIRECCION_FACTURACION_SNAPSHOT")) Then
                entidad.DireccionFacturacionSnapshot = dr("DIRECCION_FACTURACION_SNAPSHOT").ToString()
            Else
                entidad.DireccionFacturacionSnapshot = String.Empty
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
                If dr.GetName(i).Equals(nombreColumna, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            Next
            Return False
        End Function

    End Class
End Namespace