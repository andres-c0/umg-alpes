Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class CondicionPagoDatos
        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(condicionPago As CondicionPago) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONDICION_PAGO.SP_INSERTAR_CONDICION_PAGO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_NOMBRE",       OracleDbType.Varchar2).Value = condicionPago.Nombre
                    cmd.Parameters.Add("P_DIAS_CREDITO", OracleDbType.Int32).Value    = condicionPago.DiasCredito
                    cmd.Parameters.Add("P_DESCRIPCION",  OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(condicionPago.Descripcion), DBNull.Value, condicionPago.Descripcion)
                    Dim pOut As New OracleParameter("P_CONDICION_PAGO_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(condicionPago As CondicionPago)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONDICION_PAGO.SP_ACTUALIZAR_CONDICION_PAGO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CONDICION_PAGO_ID", OracleDbType.Int32).Value    = condicionPago.CondicionPagoId
                    cmd.Parameters.Add("P_NOMBRE",            OracleDbType.Varchar2).Value = condicionPago.Nombre
                    cmd.Parameters.Add("P_DIAS_CREDITO",      OracleDbType.Int32).Value    = condicionPago.DiasCredito
                    cmd.Parameters.Add("P_DESCRIPCION",       OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(condicionPago.Descripcion), DBNull.Value, condicionPago.Descripcion)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(condicionPagoId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONDICION_PAGO.SP_ELIMINAR_CONDICION_PAGO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CONDICION_PAGO_ID", OracleDbType.Int32).Value = condicionPagoId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONDICION_PAGO.SP_LISTAR_CONDICIONES_PAGO", conn)
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
                Using cmd As New OracleCommand("PKG_CONDICION_PAGO.SP_BUSCAR_CONDICIONES_PAGO", conn)
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
