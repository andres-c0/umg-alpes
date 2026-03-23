Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class AbastecimientoDatos
        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(ab As Abastecimiento) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ABASTECIMIENTO.SP_INSERTAR_ABASTECIMIENTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_MP_ID",             OracleDbType.Int32).Value    = ab.MpId
                    cmd.Parameters.Add("P_CANTIDAD_SUGERIDA", OracleDbType.Decimal).Value  = ab.CantidadSugerida
                    cmd.Parameters.Add("P_MOTIVO",            OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(ab.Motivo), DBNull.Value, ab.Motivo)
                    Dim pOut As New OracleParameter("P_ABASTECIMIENTO_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(ab As Abastecimiento)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ABASTECIMIENTO.SP_ACTUALIZAR_ABASTECIMIENTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ABASTECIMIENTO_ID",  OracleDbType.Int32).Value    = ab.AbastecimientoId
                    cmd.Parameters.Add("P_MP_ID",              OracleDbType.Int32).Value    = ab.MpId
                    cmd.Parameters.Add("P_CANTIDAD_SUGERIDA",  OracleDbType.Decimal).Value  = ab.CantidadSugerida
                    cmd.Parameters.Add("P_MOTIVO",             OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(ab.Motivo), DBNull.Value, ab.Motivo)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(abastecimientoId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ABASTECIMIENTO.SP_ELIMINAR_ABASTECIMIENTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ABASTECIMIENTO_ID", OracleDbType.Int32).Value = abastecimientoId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ABASTECIMIENTO.SP_LISTAR_ABASTECIMIENTOS", conn)
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
                Using cmd As New OracleCommand("PKG_ABASTECIMIENTO.SP_BUSCAR_ABASTECIMIENTOS", conn)
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
