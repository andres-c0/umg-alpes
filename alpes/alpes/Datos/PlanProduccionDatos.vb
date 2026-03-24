Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class PlanProduccionDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(planProduccion As PlanProduccion) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PLAN_PRODUCCION.SP_INSERTAR_PLAN_PRODUCCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PRODUCTO_ID",           OracleDbType.Int32).Value   = planProduccion.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD",              OracleDbType.Int32).Value   = planProduccion.Cantidad
                    cmd.Parameters.Add("P_PERIODO_INICIO",        OracleDbType.Date).Value    = planProduccion.PeriodoInicio
                    cmd.Parameters.Add("P_PERIODO_FIN",           OracleDbType.Date).Value    = planProduccion.PeriodoFin
                    cmd.Parameters.Add("P_TIEMPO_ESTIMADO_HORAS", OracleDbType.Decimal).Value = If(planProduccion.TiempoEstimadoHoras.HasValue, planProduccion.TiempoEstimadoHoras.Value, DBNull.Value)

                    Dim pOut As New OracleParameter("P_PLAN_PRODUCCION_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(planProduccion As PlanProduccion)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PLAN_PRODUCCION.SP_ACTUALIZAR_PLAN_PRODUCCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PLAN_PRODUCCION_ID",    OracleDbType.Int32).Value   = planProduccion.PlanProduccionId
                    cmd.Parameters.Add("P_PRODUCTO_ID",           OracleDbType.Int32).Value   = planProduccion.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD",              OracleDbType.Int32).Value   = planProduccion.Cantidad
                    cmd.Parameters.Add("P_PERIODO_INICIO",        OracleDbType.Date).Value    = planProduccion.PeriodoInicio
                    cmd.Parameters.Add("P_PERIODO_FIN",           OracleDbType.Date).Value    = planProduccion.PeriodoFin
                    cmd.Parameters.Add("P_TIEMPO_ESTIMADO_HORAS", OracleDbType.Decimal).Value = If(planProduccion.TiempoEstimadoHoras.HasValue, planProduccion.TiempoEstimadoHoras.Value, DBNull.Value)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(planProduccionId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PLAN_PRODUCCION.SP_ELIMINAR_PLAN_PRODUCCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PLAN_PRODUCCION_ID", OracleDbType.Int32).Value = planProduccionId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PLAN_PRODUCCION.SP_LISTAR_PLAN_PRODUCCION", conn)
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
                Using cmd As New OracleCommand("PKG_PLAN_PRODUCCION.SP_BUSCAR_PLAN_PRODUCCION", conn)
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
