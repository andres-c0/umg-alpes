Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class MovimientoInventarioDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(movimiento As MovimientoInventario) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_INVENTARIO.SP_INSERTAR_MOVIMIENTO_INVENTARIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_INV_PROD_ID",   OracleDbType.Int32).Value     = movimiento.InvProdId
                    cmd.Parameters.Add("P_TIPO_MOV",      OracleDbType.Varchar2).Value  = movimiento.TipoMov
                    cmd.Parameters.Add("P_CANTIDAD",      OracleDbType.Int32).Value     = movimiento.Cantidad
                    cmd.Parameters.Add("P_MOTIVO",        OracleDbType.Varchar2).Value  = If(String.IsNullOrWhiteSpace(movimiento.Motivo), DBNull.Value, movimiento.Motivo)
                    cmd.Parameters.Add("P_REFERENCIA_ID", OracleDbType.Int32).Value     = If(movimiento.ReferenciaId.HasValue, movimiento.ReferenciaId.Value, DBNull.Value)
                    cmd.Parameters.Add("P_MOV_AT",        OracleDbType.TimeStamp).Value = movimiento.MovAt

                    Dim pOut As New OracleParameter("P_MOV_INV_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(movimiento As MovimientoInventario)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_INVENTARIO.SP_ACTUALIZAR_MOVIMIENTO_INVENTARIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_MOV_INV_ID",    OracleDbType.Int32).Value     = movimiento.MovInvId
                    cmd.Parameters.Add("P_INV_PROD_ID",   OracleDbType.Int32).Value     = movimiento.InvProdId
                    cmd.Parameters.Add("P_TIPO_MOV",      OracleDbType.Varchar2).Value  = movimiento.TipoMov
                    cmd.Parameters.Add("P_CANTIDAD",      OracleDbType.Int32).Value     = movimiento.Cantidad
                    cmd.Parameters.Add("P_MOTIVO",        OracleDbType.Varchar2).Value  = If(String.IsNullOrWhiteSpace(movimiento.Motivo), DBNull.Value, movimiento.Motivo)
                    cmd.Parameters.Add("P_REFERENCIA_ID", OracleDbType.Int32).Value     = If(movimiento.ReferenciaId.HasValue, movimiento.ReferenciaId.Value, DBNull.Value)
                    cmd.Parameters.Add("P_MOV_AT",        OracleDbType.TimeStamp).Value = movimiento.MovAt

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(movInvId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_INVENTARIO.SP_ELIMINAR_MOVIMIENTO_INVENTARIO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_MOV_INV_ID", OracleDbType.Int32).Value = movInvId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_INVENTARIO.SP_LISTAR_MOVIMIENTO_INVENTARIO", conn)
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
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_INVENTARIO.SP_BUSCAR_MOVIMIENTO_INVENTARIO", conn)
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
