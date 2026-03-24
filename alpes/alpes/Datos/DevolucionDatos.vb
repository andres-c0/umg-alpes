Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class DevolucionDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(devolucion As Devolucion) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_INSERTAR_DEVOLUCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = devolucion.OrdenVentaId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = devolucion.CliId
                    cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = devolucion.Motivo
                    cmd.Parameters.Add("P_ESTADO_DEVOLUCION", OracleDbType.Varchar2).Value = devolucion.EstadoDevolucion
                    cmd.Parameters.Add("P_SOLICITUD_AT", OracleDbType.TimeStamp).Value = devolucion.SolicitudAt
                    cmd.Parameters.Add("P_RESOLUCION_AT", OracleDbType.TimeStamp).Value =
                        If(devolucion.ResolucionAt Is Nothing, DBNull.Value, devolucion.ResolucionAt)

                    Dim pOut As New OracleParameter("P_DEVOLUCION_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(devolucion As Devolucion)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_ACTUALIZAR_DEVOLUCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_DEVOLUCION_ID", OracleDbType.Int32).Value = devolucion.DevolucionId
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = devolucion.OrdenVentaId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = devolucion.CliId
                    cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = devolucion.Motivo
                    cmd.Parameters.Add("P_ESTADO_DEVOLUCION", OracleDbType.Varchar2).Value = devolucion.EstadoDevolucion
                    cmd.Parameters.Add("P_SOLICITUD_AT", OracleDbType.TimeStamp).Value = devolucion.SolicitudAt
                    cmd.Parameters.Add("P_RESOLUCION_AT", OracleDbType.TimeStamp).Value =
                        If(devolucion.ResolucionAt Is Nothing, DBNull.Value, devolucion.ResolucionAt)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(devolucionId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_ELIMINAR_DEVOLUCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_DEVOLUCION_ID", OracleDbType.Int32).Value = devolucionId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_LISTAR_DEVOLUCION", conn)
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
                Using cmd As New OracleCommand("PKG_DEVOLUCION.SP_BUSCAR_DEVOLUCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output
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
