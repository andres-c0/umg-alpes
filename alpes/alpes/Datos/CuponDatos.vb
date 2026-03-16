Option Strict On
Option Explicit On

Imports System.Configuration
Imports System.Data
Imports Oracle.ManagedDataAccess.Client

Public Class CuponDatos

    Private ReadOnly _connectionString As String

    Public Sub New()
        _connectionString = ConfigurationManager.ConnectionStrings("OracleConnection").ConnectionString
    End Sub

    Public Function Insertar(ByVal entidad As Cupon) As Integer
        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_CUPON.SP_INSERTAR_CUPON", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Codigo), CType(DBNull.Value, Object), entidad.Codigo)
            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Descripcion), CType(DBNull.Value, Object), entidad.Descripcion)
            cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio
            cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin
            cmd.Parameters.Add("P_LIMITE_USO_TOTAL", OracleDbType.Int32).Value = If(entidad.LimiteUsoTotal.HasValue, CType(entidad.LimiteUsoTotal.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_LIMITE_USO_POR_CLIENTE", OracleDbType.Int32).Value = If(entidad.LimiteUsoPorCliente.HasValue, CType(entidad.LimiteUsoPorCliente.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_USOS_ACTUALES", OracleDbType.Int32).Value = entidad.UsosActuales
            cmd.Parameters.Add("P_CUPON_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                cn.Open()
                cmd.ExecuteNonQuery()
                Return Convert.ToInt32(cmd.Parameters("P_CUPON_ID").Value.ToString())
            End Using
        End Using
    End Function

    Public Sub Actualizar(ByVal entidad As Cupon)
        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_CUPON.SP_ACTUALIZAR_CUPON", cn)
                cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("P_CUPON_ID", OracleDbType.Int32).Value = entidad.CuponId
            cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Codigo), CType(DBNull.Value, Object), entidad.Codigo)
            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Descripcion), CType(DBNull.Value, Object), entidad.Descripcion)
            cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio
            cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin
            cmd.Parameters.Add("P_LIMITE_USO_TOTAL", OracleDbType.Int32).Value = If(entidad.LimiteUsoTotal.HasValue, CType(entidad.LimiteUsoTotal.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_LIMITE_USO_POR_CLIENTE", OracleDbType.Int32).Value = If(entidad.LimiteUsoPorCliente.HasValue, CType(entidad.LimiteUsoPorCliente.Value, Object), DBNull.Value)
            cmd.Parameters.Add("P_USOS_ACTUALES", OracleDbType.Int32).Value = entidad.UsosActuales

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Eliminar(ByVal id As Integer)
        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_CUPON.SP_ELIMINAR_CUPON", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_CUPON_ID", OracleDbType.Int32).Value = id

                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ObtenerPorId(ByVal id As Integer) As DataTable
        Dim dt As New DataTable()

        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_CUPON.SP_OBTENER_CUPON", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("P_CUPON_ID", OracleDbType.Int32).Value = id
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

        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_CUPON.SP_LISTAR_CUPONES", cn)
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

        Using cn As New OracleConnection(_connectionString)
            Using cmd As New OracleCommand("PKG_CUPON.SP_BUSCAR_CUPONES", cn)
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
