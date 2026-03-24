Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class CarritoDetalleDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(carritoDetalle As CarritoDetalle) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CARRITO_DETALLE.SP_INSERTAR_CARRITO_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CARRITO_ID", OracleDbType.Int32).Value = carritoDetalle.CarritoId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = carritoDetalle.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = carritoDetalle.Cantidad
                    cmd.Parameters.Add("P_PRECIO_UNITARIO_SNAPSHOT", OracleDbType.Decimal).Value =
                        If(carritoDetalle.PrecioUnitarioSnapshot.HasValue, CType(carritoDetalle.PrecioUnitarioSnapshot.Value, Object), DBNull.Value)

                    Dim pOut As New OracleParameter("P_CARRITO_DET_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(carritoDetalle As CarritoDetalle)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CARRITO_DETALLE.SP_ACTUALIZAR_CARRITO_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CARRITO_DET_ID", OracleDbType.Int32).Value = carritoDetalle.CarritoDetId
                    cmd.Parameters.Add("P_CARRITO_ID", OracleDbType.Int32).Value = carritoDetalle.CarritoId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = carritoDetalle.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = carritoDetalle.Cantidad
                    cmd.Parameters.Add("P_PRECIO_UNITARIO_SNAPSHOT", OracleDbType.Decimal).Value =
                        If(carritoDetalle.PrecioUnitarioSnapshot.HasValue, CType(carritoDetalle.PrecioUnitarioSnapshot.Value, Object), DBNull.Value)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(carritoDetalleId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CARRITO_DETALLE.SP_ELIMINAR_CARRITO_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CARRITO_DET_ID", OracleDbType.Int32).Value = carritoDetalleId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CARRITO_DETALLE.SP_LISTAR_CARRITO_DETALLE", conn)
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
                Using cmd As New OracleCommand("PKG_CARRITO_DETALLE.SP_BUSCAR_CARRITO_DETALLE", conn)
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
