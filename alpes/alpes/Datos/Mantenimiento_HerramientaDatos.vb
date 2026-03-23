Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class MantenimientoHerramientaDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(entidad As MantenimientoHerramienta) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MANTENIMIENTO_HERRAMIENTA.SP_INSERTAR_MANTENIMIENTO_HERRAMIENTA", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HERR_ID", OracleDbType.Int32).Value = entidad.HerrId
                    cmd.Parameters.Add("P_FECHA_MANTENIMIENTO", OracleDbType.Date).Value = entidad.FechaMantenimiento
                    cmd.Parameters.Add("P_TIPO_MANTENIMIENTO", OracleDbType.Varchar2).Value = entidad.TipoMantenimiento
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(entidad.Descripcion), DBNull.Value, entidad.Descripcion)
                    cmd.Parameters.Add("P_RESPONSABLE", OracleDbType.Varchar2).Value = entidad.Responsable
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    Dim pOut As New OracleParameter("P_MANT_HERR_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using
            End Using
        End Function


        Public Sub Actualizar(entidad As MantenimientoHerramienta)

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MANTENIMIENTO_HERRAMIENTA.SP_ACTUALIZAR_MANTENIMIENTO_HERRAMIENTA", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_MANT_HERR_ID", OracleDbType.Int32).Value = entidad.MantHerrId
                    cmd.Parameters.Add("P_HERR_ID", OracleDbType.Int32).Value = entidad.HerrId
                    cmd.Parameters.Add("P_FECHA_MANTENIMIENTO", OracleDbType.Date).Value = entidad.FechaMantenimiento
                    cmd.Parameters.Add("P_TIPO_MANTENIMIENTO", OracleDbType.Varchar2).Value = entidad.TipoMantenimiento
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(entidad.Descripcion), DBNull.Value, entidad.Descripcion)
                    cmd.Parameters.Add("P_RESPONSABLE", OracleDbType.Varchar2).Value = entidad.Responsable
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Sub Eliminar(mantHerrId As Integer)

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MANTENIMIENTO_HERRAMIENTA.SP_ELIMINAR_MANTENIMIENTO_HERRAMIENTA", conn)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_MANT_HERR_ID", OracleDbType.Int32).Value = mantHerrId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Function Listar() As DataTable

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MANTENIMIENTO_HERRAMIENTA.SP_LISTAR_MANTENIMIENTOS_HERRAMIENTA", conn)

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
                Using cmd As New OracleCommand("PKG_MANTENIMIENTO_HERRAMIENTA.SP_BUSCAR_MANTENIMIENTOS_HERRAMIENTA", conn)

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


        Public Function ObtenerPorId(mantHerrId As Integer) As DataTable

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MANTENIMIENTO_HERRAMIENTA.SP_OBTENER_MANTENIMIENTO_HERRAMIENTA", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_MANT_HERR_ID", OracleDbType.Int32).Value = mantHerrId
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