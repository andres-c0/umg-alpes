Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client

Public Class ReglaPromocionDatos

    Private ReadOnly _conexion As alpes.Datos.ConexionOracle

    Public Sub New()
        _conexion = New alpes.Datos.ConexionOracle()
    End Sub

    Public Function Insertar(ByVal entidad As ReglaPromocion) As Integer
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_INSERTAR_REGLA_PROMOCION", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = entidad.PromocionId
            cmd.Parameters.Add("P_MIN_COMPRA", OracleDbType.Decimal).Value = If(entidad.MinCompra.HasValue, CType(entidad.MinCompra.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_MIN_ITEMS", OracleDbType.Int32).Value = If(entidad.MinItems.HasValue, CType(entidad.MinItems.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_APLICA_TIPO_PRODUCTO", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.AplicaTipoProducto), CType(DBNull.Value, Object), entidad.AplicaTipoProducto)
            cmd.Parameters.Add("P_TOPE_DESCUENTO", OracleDbType.Decimal).Value = If(entidad.TopeDescuento.HasValue, CType(entidad.TopeDescuento.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_REGLA_PROMOCION_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                cn.Open()
                cmd.ExecuteNonQuery()
                Return Convert.ToInt32(cmd.Parameters("P_REGLA_PROMOCION_ID").Value.ToString())
            End Using
        End Using
    End Function

    Public Sub Actualizar(ByVal entidad As ReglaPromocion)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_ACTUALIZAR_REGLA_PROMOCION", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_REGLA_PROMOCION_ID", OracleDbType.Int32).Value = entidad.ReglaPromocionId
            cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = entidad.PromocionId
            cmd.Parameters.Add("P_MIN_COMPRA", OracleDbType.Decimal).Value = If(entidad.MinCompra.HasValue, CType(entidad.MinCompra.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_MIN_ITEMS", OracleDbType.Int32).Value = If(entidad.MinItems.HasValue, CType(entidad.MinItems.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_APLICA_TIPO_PRODUCTO", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.AplicaTipoProducto), CType(DBNull.Value, Object), entidad.AplicaTipoProducto)
            cmd.Parameters.Add("P_TOPE_DESCUENTO", OracleDbType.Decimal).Value = If(entidad.TopeDescuento.HasValue, CType(entidad.TopeDescuento.Value, Object), DBNull.Value)

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Eliminar(ByVal id As Integer)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_ELIMINAR_REGLA_PROMOCION", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_REGLA_PROMOCION_ID", OracleDbType.Int32).Value = id

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ObtenerPorId(ByVal id As Integer) As DataTable
        Dim dt As New DataTable()

        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_OBTENER_REGLA_PROMOCION", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_REGLA_PROMOCION_ID", OracleDbType.Int32).Value = id
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
            Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_LISTAR_REGLAS_PROMOCION", cn)
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
            Using cmd As New OracleCommand("PKG_REGLA_PROMOCION.SP_BUSCAR_REGLAS_PROMOCION", cn)
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
