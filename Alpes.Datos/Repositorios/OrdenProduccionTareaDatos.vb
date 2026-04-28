Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Produccion

Namespace Repositorios
    Public Class OrdenProduccionTareaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As OrdenProduccionTarea) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION_TAREA.SP_INSERTAR_ORDEN_PRODUCCION_TAREA", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ORDEN_PRODUCCION_ID", OracleDbType.Int32).Value = entidad.OrdenProduccionId
                        cmd.Parameters.Add("P_NOMBRE_TAREA", OracleDbType.Varchar2).Value = entidad.NombreTarea

                        Dim pOrden As New OracleParameter("P_ORDEN", OracleDbType.Int32)
                        If entidad.Orden.HasValue Then
                            pOrden.Value = entidad.Orden.Value
                        Else
                            pOrden.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pOrden)

                        Dim pInicioAt As New OracleParameter("P_INICIO_AT", OracleDbType.TimeStamp)
                        If entidad.InicioAt.HasValue Then
                            pInicioAt.Value = entidad.InicioAt.Value
                        Else
                            pInicioAt.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pInicioAt)

                        Dim pFinAt As New OracleParameter("P_FIN_AT", OracleDbType.TimeStamp)
                        If entidad.FinAt.HasValue Then
                            pFinAt.Value = entidad.FinAt.Value
                        Else
                            pFinAt.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pFinAt)

                        Dim pEmpIdResponsable As New OracleParameter("P_EMP_ID_RESPONSABLE", OracleDbType.Int32)
                        If entidad.EmpIdResponsable.HasValue Then
                            pEmpIdResponsable.Value = entidad.EmpIdResponsable.Value
                        Else
                            pEmpIdResponsable.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pEmpIdResponsable)

                        Dim pEstado As New OracleParameter("P_ESTADO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Estado) Then
                            pEstado.Value = "ACTIVO"
                        Else
                            pEstado.Value = entidad.Estado
                        End If
                        cmd.Parameters.Add(pEstado)

                        Dim pId As New OracleParameter("P_OP_TAREA_ID", OracleDbType.Int32)
                        pId.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pId)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pId.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As OrdenProduccionTarea) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION_TAREA.SP_ACTUALIZAR_ORDEN_PRODUCCION_TAREA", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_OP_TAREA_ID", OracleDbType.Int32).Value = entidad.OpTareaId
                        cmd.Parameters.Add("P_ORDEN_PRODUCCION_ID", OracleDbType.Int32).Value = entidad.OrdenProduccionId
                        cmd.Parameters.Add("P_NOMBRE_TAREA", OracleDbType.Varchar2).Value = entidad.NombreTarea

                        Dim pOrden As New OracleParameter("P_ORDEN", OracleDbType.Int32)
                        If entidad.Orden.HasValue Then
                            pOrden.Value = entidad.Orden.Value
                        Else
                            pOrden.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pOrden)

                        Dim pInicioAt As New OracleParameter("P_INICIO_AT", OracleDbType.TimeStamp)
                        If entidad.InicioAt.HasValue Then
                            pInicioAt.Value = entidad.InicioAt.Value
                        Else
                            pInicioAt.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pInicioAt)

                        Dim pFinAt As New OracleParameter("P_FIN_AT", OracleDbType.TimeStamp)
                        If entidad.FinAt.HasValue Then
                            pFinAt.Value = entidad.FinAt.Value
                        Else
                            pFinAt.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pFinAt)

                        Dim pEmpIdResponsable As New OracleParameter("P_EMP_ID_RESPONSABLE", OracleDbType.Int32)
                        If entidad.EmpIdResponsable.HasValue Then
                            pEmpIdResponsable.Value = entidad.EmpIdResponsable.Value
                        Else
                            pEmpIdResponsable.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pEmpIdResponsable)

                        Dim pEstado As New OracleParameter("P_ESTADO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Estado) Then
                            pEstado.Value = "ACTIVO"
                        Else
                            pEstado.Value = entidad.Estado
                        End If
                        cmd.Parameters.Add(pEstado)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal opTareaId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION_TAREA.SP_ELIMINAR_ORDEN_PRODUCCION_TAREA", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_OP_TAREA_ID", OracleDbType.Int32).Value = opTareaId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal opTareaId As Integer) As OrdenProduccionTarea
            Dim entidad As OrdenProduccionTarea = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION_TAREA.SP_OBTENER_ORDEN_PRODUCCION_TAREA", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_OP_TAREA_ID", OracleDbType.Int32).Value = opTareaId
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = Mapear(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of OrdenProduccionTarea)
            Dim lista As New List(Of OrdenProduccionTarea)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION_TAREA.SP_LISTAR_ORDEN_PRODUCCION_TAREA", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of OrdenProduccionTarea)
            Dim lista As New List(Of OrdenProduccionTarea)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_ORDEN_PRODUCCION_TAREA.SP_BUSCAR_ORDEN_PRODUCCION_TAREA", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function Mapear(ByVal dr As OracleDataReader) As OrdenProduccionTarea
            Dim entidad As New OrdenProduccionTarea()

            entidad.OpTareaId = Convert.ToInt32(dr("OP_TAREA_ID"))
            entidad.OrdenProduccionId = Convert.ToInt32(dr("ORDEN_PRODUCCION_ID"))
            entidad.NombreTarea = dr("NOMBRE_TAREA").ToString()

            If dr("ORDEN") Is DBNull.Value Then
                entidad.Orden = Nothing
            Else
                entidad.Orden = Convert.ToInt32(dr("ORDEN"))
            End If

            If dr("INICIO_AT") Is DBNull.Value Then
                entidad.InicioAt = Nothing
            Else
                entidad.InicioAt = Convert.ToDateTime(dr("INICIO_AT"))
            End If

            If dr("FIN_AT") Is DBNull.Value Then
                entidad.FinAt = Nothing
            Else
                entidad.FinAt = Convert.ToDateTime(dr("FIN_AT"))
            End If

            If dr("EMP_ID_RESPONSABLE") Is DBNull.Value Then
                entidad.EmpIdResponsable = Nothing
            Else
                entidad.EmpIdResponsable = Convert.ToInt32(dr("EMP_ID_RESPONSABLE"))
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