Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client

Public Class ZonaEnvioDatos

    Private ReadOnly _conexion As Datos.ConexionOracle

    Public Sub New()
        _conexion = New Datos.ConexionOracle()
    End Sub

    Public Function Insertar(ByVal entidad As ZonaEnvio) As Integer
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_ZONA_ENVIO.SP_INSERTAR_ZONA_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Nombre), CType(DBNull.Value, Object), entidad.Nombre)
            cmd.Parameters.Add("P_PAIS", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Pais), CType(DBNull.Value, Object), entidad.Pais)
            cmd.Parameters.Add("P_DEPARTAMENTO", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Departamento), CType(DBNull.Value, Object), entidad.Departamento)
            cmd.Parameters.Add("P_CIUDAD", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Ciudad), CType(DBNull.Value, Object), entidad.Ciudad)
            cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                cn.Open()
                cmd.ExecuteNonQuery()
                Return Convert.ToInt32(cmd.Parameters("P_ZONA_ENVIO_ID").Value.ToString())
            End Using
        End Using
    End Function

    Public Sub Actualizar(ByVal entidad As ZonaEnvio)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_ZONA_ENVIO.SP_ACTUALIZAR_ZONA_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = entidad.ZonaEnvioId
            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Nombre), CType(DBNull.Value, Object), entidad.Nombre)
            cmd.Parameters.Add("P_PAIS", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Pais), CType(DBNull.Value, Object), entidad.Pais)
            cmd.Parameters.Add("P_DEPARTAMENTO", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Departamento), CType(DBNull.Value, Object), entidad.Departamento)
            cmd.Parameters.Add("P_CIUDAD", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Ciudad), CType(DBNull.Value, Object), entidad.Ciudad)

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Eliminar(ByVal id As Integer)
        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_ZONA_ENVIO.SP_ELIMINAR_ZONA_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = id

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ObtenerPorId(ByVal id As Integer) As DataTable
        Dim dt As New DataTable()

        Using cn = _conexion.ObtenerConexion()
            Using cmd As New OracleCommand("PKG_ZONA_ENVIO.SP_OBTENER_ZONA_ENVIO", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_ZONA_ENVIO_ID", OracleDbType.Int32).Value = id
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
            Using cmd As New OracleCommand("PKG_ZONA_ENVIO.SP_LISTAR_ZONAS_ENVIO", cn)
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
            Using cmd As New OracleCommand("PKG_ZONA_ENVIO.SP_BUSCAR_ZONAS_ENVIO", cn)
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
