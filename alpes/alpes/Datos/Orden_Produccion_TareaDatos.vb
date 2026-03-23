Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class OrdenProduccionTareaDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(entidad As OrdenProduccionTarea) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION_TAREA.SP_INSERTAR_ORDEN_PRODUCCION_TAREA", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_OP_ID", OracleDbType.Int32).Value = entidad.OpId
                    cmd.Parameters.Add("P_NOMBRE_TAREA", OracleDbType.Varchar2).Value = entidad.NombreTarea
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(entidad.Descripcion), DBNull.Value, entidad.Descripcion)
                    cmd.Parameters.Add("P_RESPONSABLE", OracleDbType.Varchar2).Value = entidad.Responsable
                    cmd.Parameters.Add("P_FECHA_INICIO", OracleDbType.Date).Value = entidad.FechaInicio
                    cmd.Parameters.Add("P_FECHA_FIN", OracleDbType.Date).Value = entidad.FechaFin
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    Dim pOut As New OracleParameter("P_OPT_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using
            End Using
        End Function


        Public Sub Actualizar(entidad As OrdenProduccionTarea)

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION_TAREA.SP_ACTUALIZAR_ORDEN_PRODUCCION_TAREA", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_OPT_ID", OracleDbType.Int32).Value = entidad.OptId
                    cmd.Parameters.Add("P_OP_ID", OracleDbType.Int32).Value = entidad.OpId
                    cmd.Parameters.Add("P_NOMBRE_TAREA", OracleDbType.Varchar2).Value = entidad.NombreTarea
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(entidad.Descripcion), DBNull.Value, entidad.Descripcion)
                    cmd.Parameters.Add("P_RESPONSABLE", OracleDbType.Varchar2).Value = entidad.Responsable
                    cmd.Parameters.Add("P_FECHA_INICIO", OracleDbType.Date).Value = entidad.FechaInicio
                    cmd.Parameters.Add("P_FECHA_FIN", OracleDbType.Date).Value = entidad.FechaFin
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Sub Eliminar(optId As Integer)

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION_TAREA.SP_ELIMINAR_ORDEN_PRODUCCION_TAREA", conn)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_OPT_ID", OracleDbType.Int32).Value = optId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Function Listar() As DataTable

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION_TAREA.SP_LISTAR_ORDENES_PRODUCCION_TAREA", conn)

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
                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION_TAREA.SP_BUSCAR_ORDENES_PRODUCCION_TAREA", conn)

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


        Public Function ObtenerPorId(optId As Integer) As DataTable

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION_TAREA.SP_OBTENER_ORDEN_PRODUCCION_TAREA", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_OPT_ID", OracleDbType.Int32).Value = optId
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