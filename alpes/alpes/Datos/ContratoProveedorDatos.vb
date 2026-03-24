Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class ContratoProveedorDatos
        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(cp As ContratoProveedor) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTRATO_PROVEEDOR.SP_INSERTAR_CONTRATO_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PROV_ID",         OracleDbType.Int32).Value    = cp.ProvId
                    cmd.Parameters.Add("P_TITULO",          OracleDbType.Varchar2).Value = cp.Titulo
                    cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value     = If(cp.VigenciaInicio = Nothing, DBNull.Value, cp.VigenciaInicio)
                    cmd.Parameters.Add("P_VIGENCIA_FIN",    OracleDbType.Date).Value     = If(cp.VigenciaFin = Nothing, DBNull.Value, cp.VigenciaFin)
                    cmd.Parameters.Add("P_URL_DOCUMENTO",   OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(cp.UrlDocumento), DBNull.Value, cp.UrlDocumento)
                    Dim pOut As New OracleParameter("P_CONTRATO_PROVEEDOR_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(cp As ContratoProveedor)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTRATO_PROVEEDOR.SP_ACTUALIZAR_CONTRATO_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CONTRATO_PROVEEDOR_ID", OracleDbType.Int32).Value    = cp.ContratoProveedorId
                    cmd.Parameters.Add("P_PROV_ID",               OracleDbType.Int32).Value    = cp.ProvId
                    cmd.Parameters.Add("P_TITULO",                OracleDbType.Varchar2).Value = cp.Titulo
                    cmd.Parameters.Add("P_VIGENCIA_INICIO",       OracleDbType.Date).Value     = If(cp.VigenciaInicio = Nothing, DBNull.Value, cp.VigenciaInicio)
                    cmd.Parameters.Add("P_VIGENCIA_FIN",          OracleDbType.Date).Value     = If(cp.VigenciaFin = Nothing, DBNull.Value, cp.VigenciaFin)
                    cmd.Parameters.Add("P_URL_DOCUMENTO",         OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(cp.UrlDocumento), DBNull.Value, cp.UrlDocumento)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(contratoProveedorId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTRATO_PROVEEDOR.SP_ELIMINAR_CONTRATO_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CONTRATO_PROVEEDOR_ID", OracleDbType.Int32).Value = contratoProveedorId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTRATO_PROVEEDOR.SP_LISTAR_CONTRATOS_PROVEEDOR", conn)
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
                Using cmd As New OracleCommand("PKG_CONTRATO_PROVEEDOR.SP_BUSCAR_CONTRATOS_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR",    OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR",   OracleDbType.RefCursor).Direction = ParameterDirection.Output
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
