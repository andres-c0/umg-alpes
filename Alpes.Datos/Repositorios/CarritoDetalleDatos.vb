Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class CarritoDetalleDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As CarritoDetalle) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CARRITO_DETALLE.SP_INSERTAR_CARRITO_DETALLE", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CARRITO_ID", OracleDbType.Int32).Value = entidad.CarritoId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = entidad.Cantidad

                    Dim pPrecio As New OracleParameter("P_PRECIO_UNITARIO_SNAPSHOT", OracleDbType.Decimal)
                    If entidad.PrecioUnitarioSnapshot.HasValue Then
                        pPrecio.Value = entidad.PrecioUnitarioSnapshot.Value
                    Else
                        pPrecio.Value = DBNull.Value
                    End If
                    cmd.Parameters.Add(pPrecio)

                    Dim pCarritoDetId As New OracleParameter("P_CARRITO_DET_ID", OracleDbType.Int32)
                    pCarritoDetId.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pCarritoDetId)

                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pCarritoDetId.Value.ToString())
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As CarritoDetalle) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CARRITO_DETALLE.SP_ACTUALIZAR_CARRITO_DETALLE", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CARRITO_DET_ID", OracleDbType.Int32).Value = entidad.CarritoDetId
                    cmd.Parameters.Add("P_CARRITO_ID", OracleDbType.Int32).Value = entidad.CarritoId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = entidad.Cantidad

                    Dim pPrecio As New OracleParameter("P_PRECIO_UNITARIO_SNAPSHOT", OracleDbType.Decimal)
                    If entidad.PrecioUnitarioSnapshot.HasValue Then
                        pPrecio.Value = entidad.PrecioUnitarioSnapshot.Value
                    Else
                        pPrecio.Value = DBNull.Value
                    End If
                    cmd.Parameters.Add(pPrecio)

                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal carritoDetId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CARRITO_DETALLE.SP_ELIMINAR_CARRITO_DETALLE", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CARRITO_DET_ID", OracleDbType.Int32).Value = carritoDetId
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal carritoDetId As Integer) As CarritoDetalle
            Dim entidad As CarritoDetalle = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CARRITO_DETALLE.SP_OBTENER_CARRITO_DETALLE", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CARRITO_DET_ID", OracleDbType.Int32).Value = carritoDetId
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = Mapear(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of CarritoDetalle)
            Dim lista As New List(Of CarritoDetalle)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CARRITO_DETALLE.SP_LISTAR_CARRITO_DETALLE", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of CarritoDetalle)
            Dim lista As New List(Of CarritoDetalle)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CARRITO_DETALLE.SP_BUSCAR_CARRITO_DETALLE", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function Mapear(ByVal dr As OracleDataReader) As CarritoDetalle
            Dim entidad As New CarritoDetalle()

            entidad.CarritoDetId = Convert.ToInt32(dr("CARRITO_DET_ID"))
            entidad.CarritoId = Convert.ToInt32(dr("CARRITO_ID"))
            entidad.ProductoId = Convert.ToInt32(dr("PRODUCTO_ID"))
            entidad.Cantidad = Convert.ToInt32(dr("CANTIDAD"))

            If dr("PRECIO_UNITARIO_SNAPSHOT") Is DBNull.Value Then
                entidad.PrecioUnitarioSnapshot = Nothing
            Else
                entidad.PrecioUnitarioSnapshot = Convert.ToDecimal(dr("PRECIO_UNITARIO_SNAPSHOT"))
            End If

            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace