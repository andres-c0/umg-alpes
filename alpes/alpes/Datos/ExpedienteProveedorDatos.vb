Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class ExpedienteProveedorDatos
        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(ep As ExpedienteProveedor) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EXPEDIENTE_PROVEEDOR.SP_INSERTAR_EXPEDIENTE_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PROV_ID",         OracleDbType.Int32).Value    = ep.ProvId
                    cmd.Parameters.Add("P_TIPO_DOCUMENTO",  OracleDbType.Varchar2).Value = ep.TipoDocumento
                    cmd.Parameters.Add("P_URL_DOCUMENTO",   OracleDbType.Varchar2).Value = ep.UrlDocumento
                    cmd.Parameters.Add("P_FECHA_DOCUMENTO", OracleDbType.Date).Value     = If(ep.FechaDocumento = Nothing, DBNull.Value, ep.FechaDocumento)
                    Dim pOut As New OracleParameter("P_EXPEDIENTE_PROVEEDOR_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(ep As ExpedienteProveedor)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EXPEDIENTE_PROVEEDOR.SP_ACTUALIZAR_EXPEDIENTE_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_EXPEDIENTE_PROVEEDOR_ID", OracleDbType.Int32).Value    = ep.ExpedienteProveedorId
                    cmd.Parameters.Add("P_PROV_ID",                 OracleDbType.Int32).Value    = ep.ProvId
                    cmd.Parameters.Add("P_TIPO_DOCUMENTO",          OracleDbType.Varchar2).Value = ep.TipoDocumento
                    cmd.Parameters.Add("P_URL_DOCUMENTO",           OracleDbType.Varchar2).Value = ep.UrlDocumento
                    cmd.Parameters.Add("P_FECHA_DOCUMENTO",         OracleDbType.Date).Value     = If(ep.FechaDocumento = Nothing, DBNull.Value, ep.FechaDocumento)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(expedienteProveedorId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EXPEDIENTE_PROVEEDOR.SP_ELIMINAR_EXPEDIENTE_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_EXPEDIENTE_PROVEEDOR_ID", OracleDbType.Int32).Value = expedienteProveedorId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_EXPEDIENTE_PROVEEDOR.SP_LISTAR_EXPEDIENTES_PROVEEDOR", conn)
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
                Using cmd As New OracleCommand("PKG_EXPEDIENTE_PROVEEDOR.SP_BUSCAR_EXPEDIENTES_PROVEEDOR", conn)
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
