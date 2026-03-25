Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client

Public Class PromocionDatos

    Private ReadOnly _conexion As Datos.ConexionOracle

    Public Sub New()
        _conexion = New Datos.ConexionOracle()
    End Sub

    Public Function Insertar(ByVal entidad As Promocion) As Integer
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_PROMOCION.SP_INSERTAR_PROMOCION", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_TIPO_PROMOCION_ID", OracleDbType.Int32).Value = entidad.TipoPromocionId
            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Nombre), CType(DBNull.Value, Object), entidad.Nombre)
            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Descripcion), CType(DBNull.Value, Object), entidad.Descripcion)
            cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio
            cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin
            cmd.Parameters.Add("P_PRIORIDAD", OracleDbType.Int32).Value = entidad.Prioridad
            cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                cn.Open()
                cmd.ExecuteNonQuery()
                Return Convert.ToInt32(cmd.Parameters("P_PROMOCION_ID").Value.ToString())
            End Using
        End Using
    End Function

    Public Sub Actualizar(ByVal entidad As Promocion)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_PROMOCION.SP_ACTUALIZAR_PROMOCION", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = entidad.PromocionId
            cmd.Parameters.Add("P_TIPO_PROMOCION_ID", OracleDbType.Int32).Value = entidad.TipoPromocionId
            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Nombre), CType(DBNull.Value, Object), entidad.Nombre)
            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Descripcion), CType(DBNull.Value, Object), entidad.Descripcion)
            cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio
            cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin
            cmd.Parameters.Add("P_PRIORIDAD", OracleDbType.Int32).Value = entidad.Prioridad

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Eliminar(ByVal id As Integer)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_PROMOCION.SP_ELIMINAR_PROMOCION", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = id

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ObtenerPorId(ByVal id As Integer) As DataTable
        Dim dt As New DataTable()

        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_PROMOCION.SP_OBTENER_PROMOCION", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = id
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
            Using cmd As New OracleCommand("PKG_PROMOCION.SP_LISTAR_PROMOCIONES", cn)
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
            Using cmd As New OracleCommand("PKG_PROMOCION.SP_BUSCAR_PROMOCIONES", cn)
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
