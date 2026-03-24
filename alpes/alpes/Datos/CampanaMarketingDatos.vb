Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client

Public Class CampanaMarketingDatos

    Private ReadOnly _conexion As alpes.Datos.ConexionOracle

    Public Sub New()
        _conexion = New alpes.Datos.ConexionOracle()
    End Sub

    Public Function Insertar(ByVal entidad As CampanaMarketing) As Integer
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_CAMPANA_MARKETING.SP_INSERTAR_CAMPANA_MARKETING", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Nombre), CType(DBNull.Value, Object), entidad.Nombre)
            cmd.Parameters.Add("P_CANAL", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Canal), CType(DBNull.Value, Object), entidad.Canal)
            cmd.Parameters.Add("P_PRESUPUESTO", OracleDbType.Decimal).Value = If(entidad.Presupuesto.HasValue, CType(entidad.Presupuesto.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_INICIO", OracleDbType.Date).Value = entidad.Inicio
            cmd.Parameters.Add("P_FIN", OracleDbType.Date).Value = entidad.Fin
            cmd.Parameters.Add("P_CAMPANA_MARKETING_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                cn.Open()
                cmd.ExecuteNonQuery()
                Return Convert.ToInt32(cmd.Parameters("P_CAMPANA_MARKETING_ID").Value.ToString())
            End Using
        End Using
    End Function

    Public Sub Actualizar(ByVal entidad As CampanaMarketing)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_CAMPANA_MARKETING.SP_ACTUALIZAR_CAMPANA_MARKETING", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_CAMPANA_MARKETING_ID", OracleDbType.Int32).Value = entidad.CampanaMarketingId
            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Nombre), CType(DBNull.Value, Object), entidad.Nombre)
            cmd.Parameters.Add("P_CANAL", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Canal), CType(DBNull.Value, Object), entidad.Canal)
            cmd.Parameters.Add("P_PRESUPUESTO", OracleDbType.Decimal).Value = If(entidad.Presupuesto.HasValue, CType(entidad.Presupuesto.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_INICIO", OracleDbType.Date).Value = entidad.Inicio
            cmd.Parameters.Add("P_FIN", OracleDbType.Date).Value = entidad.Fin

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Eliminar(ByVal id As Integer)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_CAMPANA_MARKETING.SP_ELIMINAR_CAMPANA_MARKETING", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_CAMPANA_MARKETING_ID", OracleDbType.Int32).Value = id

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ObtenerPorId(ByVal id As Integer) As DataTable
        Dim dt As New DataTable()

        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_CAMPANA_MARKETING.SP_OBTENER_CAMPANA_MARKETING", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_CAMPANA_MARKETING_ID", OracleDbType.Int32).Value = id
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
            Using cmd As New OracleCommand("PKG_CAMPANA_MARKETING.SP_LISTAR_CAMPANA_MARKETING", cn)
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
            Using cmd As New OracleCommand("PKG_CAMPANA_MARKETING.SP_BUSCAR_CAMPANA_MARKETING", cn)
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
