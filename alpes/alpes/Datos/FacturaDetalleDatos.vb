Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class FacturaDetalleDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(facturaDetalle As FacturaDetalle) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_INSERTAR_FACTURA_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_ID", OracleDbType.Int32).Value = facturaDetalle.FacturaId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = facturaDetalle.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = facturaDetalle.Cantidad
                    cmd.Parameters.Add("P_PRECIO_UNITARIO_SNAPSHOT", OracleDbType.Decimal).Value = facturaDetalle.PrecioUnitarioSnapshot
                    cmd.Parameters.Add("P_TOTAL_LINEA", OracleDbType.Decimal).Value = facturaDetalle.TotalLinea

                    Dim pOut As New OracleParameter("P_FACTURA_DET_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(facturaDetalle As FacturaDetalle)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_ACTUALIZAR_FACTURA_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_DET_ID", OracleDbType.Int32).Value = facturaDetalle.FacturaDetId
                    cmd.Parameters.Add("P_FACTURA_ID", OracleDbType.Int32).Value = facturaDetalle.FacturaId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = facturaDetalle.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = facturaDetalle.Cantidad
                    cmd.Parameters.Add("P_PRECIO_UNITARIO_SNAPSHOT", OracleDbType.Decimal).Value = facturaDetalle.PrecioUnitarioSnapshot
                    cmd.Parameters.Add("P_TOTAL_LINEA", OracleDbType.Decimal).Value = facturaDetalle.TotalLinea

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(facturaDetalleId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_ELIMINAR_FACTURA_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_FACTURA_DET_ID", OracleDbType.Int32).Value = facturaDetalleId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_LISTAR_FACTURA_DETALLE", conn)
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
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_BUSCAR_FACTURA_DETALLE", conn)
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
