Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class EstadoOrdenCompraDatos
        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(estadoOc As EstadoOrdenCompra) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN_COMPRA.SP_INSERTAR_ESTADO_ORDEN_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CODIGO",      OracleDbType.Varchar2).Value = estadoOc.Codigo
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(estadoOc.Descripcion), DBNull.Value, estadoOc.Descripcion)
                    Dim pOut As New OracleParameter("P_ESTADO_OC_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(estadoOc As EstadoOrdenCompra)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN_COMPRA.SP_ACTUALIZAR_ESTADO_ORDEN_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ESTADO_OC_ID", OracleDbType.Int32).Value    = estadoOc.EstadoOcId
                    cmd.Parameters.Add("P_CODIGO",       OracleDbType.Varchar2).Value = estadoOc.Codigo
                    cmd.Parameters.Add("P_DESCRIPCION",  OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(estadoOc.Descripcion), DBNull.Value, estadoOc.Descripcion)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(estadoOcId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN_COMPRA.SP_ELIMINAR_ESTADO_ORDEN_COMPRA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ESTADO_OC_ID", OracleDbType.Int32).Value = estadoOcId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN_COMPRA.SP_LISTAR_ESTADOS_ORDEN_COMPRA", conn)
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
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN_COMPRA.SP_BUSCAR_ESTADOS_ORDEN_COMPRA", conn)
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
