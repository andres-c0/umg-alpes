Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client

Public Class TipoEntregaDatos

    Private ReadOnly _conexion As alpes.Datos.ConexionOracle

    Public Sub New()
        _conexion = New alpes.Datos.ConexionOracle()
    End Sub

    Public Function Insertar(ByVal entidad As TipoEntrega) As Integer
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_TIPO_ENTREGA.SP_INSERTAR_TIPO_ENTREGA", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Nombre), CType(DBNull.Value, Object), entidad.Nombre)
            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Descripcion), CType(DBNull.Value, Object), entidad.Descripcion)
            cmd.Parameters.Add("P_TIPO_ENTREGA_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                cn.Open()
                cmd.ExecuteNonQuery()
                Return Convert.ToInt32(cmd.Parameters("P_TIPO_ENTREGA_ID").Value.ToString())
            End Using
        End Using
    End Function

    Public Sub Actualizar(ByVal entidad As TipoEntrega)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_TIPO_ENTREGA.SP_ACTUALIZAR_TIPO_ENTREGA", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_TIPO_ENTREGA_ID", OracleDbType.Int32).Value = entidad.TipoEntregaId
            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Nombre), CType(DBNull.Value, Object), entidad.Nombre)
            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Descripcion), CType(DBNull.Value, Object), entidad.Descripcion)

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Eliminar(ByVal id As Integer)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_TIPO_ENTREGA.SP_ELIMINAR_TIPO_ENTREGA", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_TIPO_ENTREGA_ID", OracleDbType.Int32).Value = id

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ObtenerPorId(ByVal id As Integer) As DataTable
        Dim dt As New DataTable()

        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_TIPO_ENTREGA.SP_OBTENER_TIPO_ENTREGA", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_TIPO_ENTREGA_ID", OracleDbType.Int32).Value = id
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
            Using cmd As New OracleCommand("PKG_TIPO_ENTREGA.SP_LISTAR_TIPO_ENTREGA", cn)
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
            Using cmd As New OracleCommand("PKG_TIPO_ENTREGA.SP_BUSCAR_TIPO_ENTREGA", cn)
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
