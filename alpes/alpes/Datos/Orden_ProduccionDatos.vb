Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class OrdenProduccionDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(entidad As OrdenProduccion) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION.SP_INSERTAR_ORDEN_PRODUCCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CODIGO_ORDEN", OracleDbType.Varchar2).Value = entidad.CodigoOrden
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD_PROGRAMADA", OracleDbType.Decimal).Value = entidad.CantidadProgramada
                    cmd.Parameters.Add("P_FECHA_INICIO", OracleDbType.Date).Value = entidad.FechaInicio
                    cmd.Parameters.Add("P_FECHA_FIN", OracleDbType.Date).Value = entidad.FechaFin
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado
                    cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Observaciones), DBNull.Value, entidad.Observaciones)

                    Dim pOut As New OracleParameter("P_OP_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(entidad As OrdenProduccion)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION.SP_ACTUALIZAR_ORDEN_PRODUCCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_OP_ID", OracleDbType.Int32).Value = entidad.OpId
                    cmd.Parameters.Add("P_CODIGO_ORDEN", OracleDbType.Varchar2).Value = entidad.CodigoOrden
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD_PROGRAMADA", OracleDbType.Decimal).Value = entidad.CantidadProgramada
                    cmd.Parameters.Add("P_FECHA_INICIO", OracleDbType.Date).Value = entidad.FechaInicio
                    cmd.Parameters.Add("P_FECHA_FIN", OracleDbType.Date).Value = entidad.FechaFin
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado
                    cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Observaciones), DBNull.Value, entidad.Observaciones)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(opId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION.SP_ELIMINAR_ORDEN_PRODUCCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_OP_ID", OracleDbType.Int32).Value = opId

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION.SP_LISTAR_ORDENES_PRODUCCION", conn)
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
                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION.SP_BUSCAR_ORDENES_PRODUCCION", conn)
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

        Public Function ObtenerPorId(opId As Integer) As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION.SP_OBTENER_ORDEN_PRODUCCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_OP_ID", OracleDbType.Int32).Value = opId
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