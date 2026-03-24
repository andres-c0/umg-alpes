Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class FacturaDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(factura As Factura) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_INSERTAR_FACTURA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = factura.OrdenVentaId
                    cmd.Parameters.Add("P_NUM_FACTURA", OracleDbType.Varchar2).Value = factura.NumFactura
                    cmd.Parameters.Add("P_FECHA_EMISION", OracleDbType.Date).Value = factura.FechaEmision
                    cmd.Parameters.Add("P_NIT_FACTURACION", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(factura.NitFacturacion), DBNull.Value, factura.NitFacturacion)
                    cmd.Parameters.Add("P_DIRECCION_FACTURACION_SNAPSHOT", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(factura.DireccionFacturacionSnapshot), DBNull.Value, factura.DireccionFacturacionSnapshot)
                    cmd.Parameters.Add("P_TOTAL_FACTURA_SNAPSHOT", OracleDbType.Decimal).Value = factura.TotalFacturaSnapshot

                    Dim pOut As New OracleParameter("P_FACTURA_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(factura As Factura)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_ACTUALIZAR_FACTURA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_ID", OracleDbType.Int32).Value = factura.FacturaId
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = factura.OrdenVentaId
                    cmd.Parameters.Add("P_NUM_FACTURA", OracleDbType.Varchar2).Value = factura.NumFactura
                    cmd.Parameters.Add("P_FECHA_EMISION", OracleDbType.Date).Value = factura.FechaEmision
                    cmd.Parameters.Add("P_NIT_FACTURACION", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(factura.NitFacturacion), DBNull.Value, factura.NitFacturacion)
                    cmd.Parameters.Add("P_DIRECCION_FACTURACION_SNAPSHOT", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(factura.DireccionFacturacionSnapshot), DBNull.Value, factura.DireccionFacturacionSnapshot)
                    cmd.Parameters.Add("P_TOTAL_FACTURA_SNAPSHOT", OracleDbType.Decimal).Value = factura.TotalFacturaSnapshot

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(facturaId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_ELIMINAR_FACTURA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_FACTURA_ID", OracleDbType.Int32).Value = facturaId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_LISTAR_FACTURAS", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output
                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

        Public Function Buscar(criterio As String, valor As String) As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_BUSCAR_FACTURAS", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output
                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

    End Class

End Namespace
