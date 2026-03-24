Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client

Public Class PromocionProductoDatos

    Private ReadOnly _conexion As alpes.Datos.ConexionOracle

    Public Sub New()
        _conexion = New alpes.Datos.ConexionOracle()
    End Sub

    Public Function Insertar(ByVal entidad As PromocionProducto) As Integer
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_PROMOCION_PRODUCTO.SP_INSERTAR_PROMOCION_PRODUCTO", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = entidad.PromocionId
            cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
            cmd.Parameters.Add("P_LIMITE_UNIDADES", OracleDbType.Int32).Value = If(entidad.LimiteUnidades.HasValue, CType(entidad.LimiteUnidades.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_PROMOCION_PRODUCTO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                cn.Open()
                cmd.ExecuteNonQuery()
                Return Convert.ToInt32(cmd.Parameters("P_PROMOCION_PRODUCTO_ID").Value.ToString())
            End Using
        End Using
    End Function

    Public Sub Actualizar(ByVal entidad As PromocionProducto)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_PROMOCION_PRODUCTO.SP_ACTUALIZAR_PROMOCION_PRODUCTO", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_PROMOCION_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.PromocionProductoId
            cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = entidad.PromocionId
            cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
            cmd.Parameters.Add("P_LIMITE_UNIDADES", OracleDbType.Int32).Value = If(entidad.LimiteUnidades.HasValue, CType(entidad.LimiteUnidades.Value, Object), DBNull.Value)

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Eliminar(ByVal id As Integer)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_PROMOCION_PRODUCTO.SP_ELIMINAR_PROMOCION_PRODUCTO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_PROMOCION_PRODUCTO_ID", OracleDbType.Int32).Value = id

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ObtenerPorId(ByVal id As Integer) As DataTable
        Dim dt As New DataTable()

        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_PROMOCION_PRODUCTO.SP_OBTENER_PROMOCION_PRODUCTO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_PROMOCION_PRODUCTO_ID", OracleDbType.Int32).Value = id
                cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                Using da As New OracleDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function

    Public Function Listar() As DataTable
        Dim dt As New DataTable()

        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_PROMOCION_PRODUCTO.SP_LISTAR_PROMOCION_PRODUCTO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                Using da As New OracleDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function

    Public Function Buscar(ByVal criterio As String, ByVal valor As String) As DataTable
        Dim dt As New DataTable()

        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_PROMOCION_PRODUCTO.SP_BUSCAR_PROMOCION_PRODUCTO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                Using da As New OracleDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function

End Class
