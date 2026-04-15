Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Produccion

Namespace Repositorios
    Public Class OrdenProduccionDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As OrdenProduccion) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION.SP_INSERTAR_ORDEN_PRODUCCION", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_NUM_OP", OracleDbType.Varchar2).Value = entidad.NumOp
                        cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                        cmd.Parameters.Add("P_CANTIDAD_PLANIFICADA", OracleDbType.Int32).Value = entidad.CantidadPlanificada
                        cmd.Parameters.Add("P_ESTADO_PRODUCCION_ID", OracleDbType.Int32).Value = entidad.EstadoProduccionId

                        Dim pInicioEstimado As New OracleParameter("P_INICIO_ESTIMADO", OracleDbType.Date)
                        pInicioEstimado.Value = If(entidad.InicioEstimado.HasValue, CType(entidad.InicioEstimado.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pInicioEstimado)

                        Dim pFinEstimado As New OracleParameter("P_FIN_ESTIMADO", OracleDbType.Date)
                        pFinEstimado.Value = If(entidad.FinEstimado.HasValue, CType(entidad.FinEstimado.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pFinEstimado)

                        Dim pInicioReal As New OracleParameter("P_INICIO_REAL", OracleDbType.Date)
                        pInicioReal.Value = If(entidad.InicioReal.HasValue, CType(entidad.InicioReal.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pInicioReal)

                        Dim pFinReal As New OracleParameter("P_FIN_REAL", OracleDbType.Date)
                        pFinReal.Value = If(entidad.FinReal.HasValue, CType(entidad.FinReal.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pFinReal)

                        Dim pEstado As New OracleParameter("P_ESTADO", OracleDbType.Varchar2)
                        pEstado.Value = If(String.IsNullOrWhiteSpace(entidad.Estado), "ACTIVO", entidad.Estado)
                        cmd.Parameters.Add(pEstado)

                        Dim pIdOut As New OracleParameter("P_ORDEN_PRODUCCION_ID", OracleDbType.Int32)
                        pIdOut.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pIdOut)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pIdOut.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As OrdenProduccion) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION.SP_ACTUALIZAR_ORDEN_PRODUCCION", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ORDEN_PRODUCCION_ID", OracleDbType.Int32).Value = entidad.OrdenProduccionId
                        cmd.Parameters.Add("P_NUM_OP", OracleDbType.Varchar2).Value = entidad.NumOp
                        cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                        cmd.Parameters.Add("P_CANTIDAD_PLANIFICADA", OracleDbType.Int32).Value = entidad.CantidadPlanificada
                        cmd.Parameters.Add("P_ESTADO_PRODUCCION_ID", OracleDbType.Int32).Value = entidad.EstadoProduccionId

                        Dim pInicioEstimado As New OracleParameter("P_INICIO_ESTIMADO", OracleDbType.Date)
                        pInicioEstimado.Value = If(entidad.InicioEstimado.HasValue, CType(entidad.InicioEstimado.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pInicioEstimado)

                        Dim pFinEstimado As New OracleParameter("P_FIN_ESTIMADO", OracleDbType.Date)
                        pFinEstimado.Value = If(entidad.FinEstimado.HasValue, CType(entidad.FinEstimado.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pFinEstimado)

                        Dim pInicioReal As New OracleParameter("P_INICIO_REAL", OracleDbType.Date)
                        pInicioReal.Value = If(entidad.InicioReal.HasValue, CType(entidad.InicioReal.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pInicioReal)

                        Dim pFinReal As New OracleParameter("P_FIN_REAL", OracleDbType.Date)
                        pFinReal.Value = If(entidad.FinReal.HasValue, CType(entidad.FinReal.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pFinReal)

                        Dim pEstado As New OracleParameter("P_ESTADO", OracleDbType.Varchar2)
                        pEstado.Value = If(String.IsNullOrWhiteSpace(entidad.Estado), "ACTIVO", entidad.Estado)
                        cmd.Parameters.Add(pEstado)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal ordenProduccionId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION.SP_ELIMINAR_ORDEN_PRODUCCION", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ORDEN_PRODUCCION_ID", OracleDbType.Int32).Value = ordenProduccionId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal ordenProduccionId As Integer) As OrdenProduccion
            Dim entidad As OrdenProduccion = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION.SP_OBTENER_ORDEN_PRODUCCION", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_PRODUCCION_ID", OracleDbType.Int32).Value = ordenProduccionId
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearOrdenProduccion(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of OrdenProduccion)
            Dim lista As New List(Of OrdenProduccion)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION.SP_LISTAR_ORDEN_PRODUCCION", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearOrdenProduccion(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of OrdenProduccion)
            Dim lista As New List(Of OrdenProduccion)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION.SP_BUSCAR_ORDEN_PRODUCCION", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearOrdenProduccion(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearOrdenProduccion(ByVal dr As OracleDataReader) As OrdenProduccion
            Dim entidad As New OrdenProduccion()

            entidad.OrdenProduccionId = Convert.ToInt32(dr("ORDEN_PRODUCCION_ID"))
            entidad.NumOp = dr("NUM_OP").ToString()
            entidad.ProductoId = Convert.ToInt32(dr("PRODUCTO_ID"))
            entidad.CantidadPlanificada = Convert.ToInt32(dr("CANTIDAD_PLANIFICADA"))
            entidad.EstadoProduccionId = Convert.ToInt32(dr("ESTADO_PRODUCCION_ID"))

            If dr("INICIO_ESTIMADO") Is DBNull.Value Then
                entidad.InicioEstimado = Nothing
            Else
                entidad.InicioEstimado = Convert.ToDateTime(dr("INICIO_ESTIMADO"))
            End If

            If dr("FIN_ESTIMADO") Is DBNull.Value Then
                entidad.FinEstimado = Nothing
            Else
                entidad.FinEstimado = Convert.ToDateTime(dr("FIN_ESTIMADO"))
            End If

            If dr("INICIO_REAL") Is DBNull.Value Then
                entidad.InicioReal = Nothing
            Else
                entidad.InicioReal = Convert.ToDateTime(dr("INICIO_REAL"))
            End If

            If dr("FIN_REAL") Is DBNull.Value Then
                entidad.FinReal = Nothing
            Else
                entidad.FinReal = Convert.ToDateTime(dr("FIN_REAL"))
            End If

            If dr("CREATED_AT") Is DBNull.Value Then
                entidad.CreatedAt = Nothing
            Else
                entidad.CreatedAt = Convert.ToDateTime(dr("CREATED_AT"))
            End If

            If dr("UPDATED_AT") Is DBNull.Value Then
                entidad.UpdatedAt = Nothing
            Else
                entidad.UpdatedAt = Convert.ToDateTime(dr("UPDATED_AT"))
            End If

            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace