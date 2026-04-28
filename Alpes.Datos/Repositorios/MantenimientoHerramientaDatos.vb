Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Produccion

Namespace Repositorios
    Public Class MantenimientoHerramientaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As MantenimientoHerramienta) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_MANTENIMIENTO_HERRAMIENTAS.SP_INSERTAR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_HERRAMIENTA_ID", OracleDbType.Int32).Value = entidad.HerramientaId
                        cmd.Parameters.Add("P_FECHA_MANTENIMIENTO", OracleDbType.Date).Value = entidad.FechaMantenimiento

                        Dim pTipo As New OracleParameter("P_TIPO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Tipo) Then
                            pTipo.Value = DBNull.Value
                        Else
                            pTipo.Value = entidad.Tipo
                        End If
                        cmd.Parameters.Add(pTipo)

                        Dim pCosto As New OracleParameter("P_COSTO", OracleDbType.Decimal)
                        If entidad.Costo.HasValue Then
                            pCosto.Value = entidad.Costo.Value
                        Else
                            pCosto.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pCosto)

                        Dim pObservacion As New OracleParameter("P_OBSERVACION", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Observacion) Then
                            pObservacion.Value = DBNull.Value
                        Else
                            pObservacion.Value = entidad.Observacion
                        End If
                        cmd.Parameters.Add(pObservacion)

                        Dim pEstado As New OracleParameter("P_ESTADO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Estado) Then
                            pEstado.Value = DBNull.Value
                        Else
                            pEstado.Value = entidad.Estado
                        End If
                        cmd.Parameters.Add(pEstado)

                        Dim pId As New OracleParameter("P_ID", OracleDbType.Int32)
                        pId.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pId)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pId.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As MantenimientoHerramienta) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_MANTENIMIENTO_HERRAMIENTAS.SP_ACTUALIZAR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_MANTENIMIENTO_ID", OracleDbType.Int32).Value = entidad.MantenimientoId
                        cmd.Parameters.Add("P_HERRAMIENTA_ID", OracleDbType.Int32).Value = entidad.HerramientaId
                        cmd.Parameters.Add("P_FECHA_MANTENIMIENTO", OracleDbType.Date).Value = entidad.FechaMantenimiento

                        Dim pTipo As New OracleParameter("P_TIPO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Tipo) Then
                            pTipo.Value = DBNull.Value
                        Else
                            pTipo.Value = entidad.Tipo
                        End If
                        cmd.Parameters.Add(pTipo)

                        Dim pCosto As New OracleParameter("P_COSTO", OracleDbType.Decimal)
                        If entidad.Costo.HasValue Then
                            pCosto.Value = entidad.Costo.Value
                        Else
                            pCosto.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pCosto)

                        Dim pObservacion As New OracleParameter("P_OBSERVACION", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Observacion) Then
                            pObservacion.Value = DBNull.Value
                        Else
                            pObservacion.Value = entidad.Observacion
                        End If
                        cmd.Parameters.Add(pObservacion)

                        Dim pEstado As New OracleParameter("P_ESTADO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Estado) Then
                            pEstado.Value = DBNull.Value
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

        Public Function Eliminar(ByVal mantenimientoId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_MANTENIMIENTO_HERRAMIENTAS.SP_ELIMINAR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = mantenimientoId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal mantenimientoId As Integer) As MantenimientoHerramienta
            Dim entidad As MantenimientoHerramienta = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_MANTENIMIENTO_HERRAMIENTAS.SP_OBTENER", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = mantenimientoId
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

        Public Function Listar() As List(Of MantenimientoHerramienta)
            Dim lista As New List(Of MantenimientoHerramienta)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_MANTENIMIENTO_HERRAMIENTAS.SP_LISTAR", conexion)
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

        Public Function Buscar(ByVal valor As String) As List(Of MantenimientoHerramienta)
            Dim lista As New List(Of MantenimientoHerramienta)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_MANTENIMIENTO_HERRAMIENTAS.SP_BUSCAR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

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

        Private Function Mapear(ByVal dr As OracleDataReader) As MantenimientoHerramienta
            Dim entidad As New MantenimientoHerramienta()

            entidad.MantenimientoId = Convert.ToInt32(dr("MANTENIMIENTO_ID"))
            entidad.HerramientaId = Convert.ToInt32(dr("HERRAMIENTA_ID"))
            entidad.FechaMantenimiento = Convert.ToDateTime(dr("FECHA_MANTENIMIENTO"))
            entidad.Tipo = If(dr("TIPO") Is DBNull.Value, String.Empty, dr("TIPO").ToString())

            If dr("COSTO") Is DBNull.Value Then
                entidad.Costo = Nothing
            Else
                entidad.Costo = Convert.ToDecimal(dr("COSTO"))
            End If

            entidad.Observacion = If(dr("OBSERVACION") Is DBNull.Value, String.Empty, dr("OBSERVACION").ToString())

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