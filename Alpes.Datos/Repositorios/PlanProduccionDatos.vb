Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Produccion

Namespace Repositorios
    Public Class PlanProduccionDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As PlanProduccion) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                conexion.Open()

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_PLAN_PRODUCCION.SP_INSERTAR_PLAN_PRODUCCION", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                        cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = entidad.Cantidad
                        cmd.Parameters.Add("P_PERIODO_INICIO", OracleDbType.Date).Value = entidad.PeriodoInicio
                        cmd.Parameters.Add("P_PERIODO_FIN", OracleDbType.Date).Value = entidad.PeriodoFin

                        Dim pTiempo As New OracleParameter("P_TIEMPO_ESTIMADO_HORAS", OracleDbType.Decimal)
                        If entidad.TiempoEstimadoHoras.HasValue Then
                            pTiempo.Value = entidad.TiempoEstimadoHoras.Value
                        Else
                            pTiempo.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pTiempo)

                        Dim pIdOut As New OracleParameter("P_PLAN_PRODUCCION_ID", OracleDbType.Int32)
                        pIdOut.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pIdOut)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pIdOut.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As PlanProduccion) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                conexion.Open()

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_PLAN_PRODUCCION.SP_ACTUALIZAR_PLAN_PRODUCCION", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_PLAN_PRODUCCION_ID", OracleDbType.Int32).Value = entidad.PlanProduccionId
                        cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                        cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = entidad.Cantidad
                        cmd.Parameters.Add("P_PERIODO_INICIO", OracleDbType.Date).Value = entidad.PeriodoInicio
                        cmd.Parameters.Add("P_PERIODO_FIN", OracleDbType.Date).Value = entidad.PeriodoFin

                        Dim pTiempo As New OracleParameter("P_TIEMPO_ESTIMADO_HORAS", OracleDbType.Decimal)
                        If entidad.TiempoEstimadoHoras.HasValue Then
                            pTiempo.Value = entidad.TiempoEstimadoHoras.Value
                        Else
                            pTiempo.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pTiempo)

                        Dim filas As Integer = cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return filas > 0
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal planProduccionId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                conexion.Open()

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_PLAN_PRODUCCION.SP_ELIMINAR_PLAN_PRODUCCION", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_PLAN_PRODUCCION_ID", OracleDbType.Int32).Value = planProduccionId

                        Dim filas As Integer = cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return filas > 0
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal planProduccionId As Integer) As PlanProduccion
            Dim entidad As PlanProduccion = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                conexion.Open()

                Using cmd As New OracleCommand("PKG_PLAN_PRODUCCION.SP_OBTENER_PLAN_PRODUCCION", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PLAN_PRODUCCION_ID", OracleDbType.Int32).Value = planProduccionId
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearPlanProduccion(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of PlanProduccion)
            Dim lista As New List(Of PlanProduccion)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                conexion.Open()

                Using cmd As New OracleCommand("PKG_PLAN_PRODUCCION.SP_LISTAR_PLAN_PRODUCCION", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearPlanProduccion(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of PlanProduccion)
            Dim lista As New List(Of PlanProduccion)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                conexion.Open()

                Using cmd As New OracleCommand("PKG_PLAN_PRODUCCION.SP_BUSCAR_PLAN_PRODUCCION", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearPlanProduccion(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearPlanProduccion(ByVal dr As OracleDataReader) As PlanProduccion
            Dim entidad As New PlanProduccion()

            entidad.PlanProduccionId = Convert.ToInt32(dr("PLAN_PRODUCCION_ID"))
            entidad.ProductoId = Convert.ToInt32(dr("PRODUCTO_ID"))
            entidad.Cantidad = Convert.ToInt32(dr("CANTIDAD"))
            entidad.PeriodoInicio = Convert.ToDateTime(dr("PERIODO_INICIO"))
            entidad.PeriodoFin = Convert.ToDateTime(dr("PERIODO_FIN"))

            If dr("TIEMPO_ESTIMADO_HORAS") Is DBNull.Value Then
                entidad.TiempoEstimadoHoras = Nothing
            Else
                entidad.TiempoEstimadoHoras = Convert.ToDecimal(dr("TIEMPO_ESTIMADO_HORAS"))
            End If

            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace